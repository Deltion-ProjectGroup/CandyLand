using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public RaycastHit searchRay;
    [HideInInspector] public Transform target;
    [HideInInspector] public NavMeshAgent agent;
    Transform pos;

    [Header("isAttacking/isChasing")]
    [SerializeField] float chargeThinkingMax;
    [SerializeField] float chargeThinkingMin;
    [SerializeField] Transform attackPos;
    [SerializeField] float attackRange;
    float mainchargeThinking;
    public bool isChasing = false;
    RaycastHit hitPartical;
    RaycastHit hit;



    [Header("WalkArea")]
    [SerializeField] float randomUnitCircleRadiusMin;
    [SerializeField] float randomUnitCircleRadiusMax;
    [SerializeField] float thinkTimerMin;
    [SerializeField] float thinkTimerMax;
    public float attackTime = 1f;
    public float attackTimeStart;
    float thinkTimer;
    public float randomUnitCircleRadius;

    [Header("FieldOfView")]
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTarget = new List<Transform>();

    public float meshResolution;
    public int edgeResolveInterations;
    public float edgeDstThreshold;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>(); target = GameObject.FindGameObjectWithTag("Player").transform; 
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = 1.5f;

        StartCoroutine("FindTargetWithDelay", 0.2f);
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    public virtual void Update()
    {
        ThinkTimer();
        isAttacking();
        ChasePos();
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }

    public virtual void RandomPos()
    {
        // Think if Not Chasing
        if (isChasing == false)
        {
            // Pick a random point in the insideUnitCircle for X and Y and set it in a vector3
            Vector3 newPos = transform.position + new Vector3(Random.insideUnitCircle.x * randomUnitCircleRadius, transform.position.y, Random.insideUnitCircle.y * randomUnitCircleRadius);
            // Put the newPos in the setDestination
            agent.SetDestination(newPos);
        }
        else if (health <= 0)
        {
            agent.enabled = false;
        }
    }

    public virtual void ThinkTimer()
    {
        // the enemy has to think befor he can charged
        if (isChasing)
        {
            mainchargeThinking -= Time.deltaTime;
            if (mainchargeThinking <= 0)
            {
                print("charge");
                mainchargeThinking = Random.Range(chargeThinkingMin, chargeThinkingMax);
                isChasing = true;
            }
            //isChasing = false;
        }
        else // it pickes a random pos out of the radius  
        {
            isChasing = false;
            thinkTimer -= Time.deltaTime;
            {
                // Subtract ThinkTimer over time.
                if (thinkTimer <= 0)
                {
                    // Give a random value radius
                    randomUnitCircleRadius = Random.Range(randomUnitCircleRadiusMin, randomUnitCircleRadiusMax);
                    RandomPos();
                    // Give a random value thinkTimer
                    thinkTimer = Random.Range(thinkTimerMin, thinkTimerMax);
                }
            }
        }
    }

    public virtual void LoseEnemy()
    {
        transform.GetComponentInChildren<EnemyIsAttack>().LookAt(null);
        isChasing = false;
    }

    public virtual void ChasePos()
    {
        if (isChasing)
        {
            agent.speed = 3;
            agent.SetDestination(target.position);
        }
    }
    public virtual void isAttacking()
    {
        if (Physics.Raycast(attackPos.position, attackPos.forward, out hit, attackRange))
        {
            if (hit.transform.tag == "Player")
            {
                print("hit player");
            }
        }
        Debug.DrawRay(attackPos.position, attackPos.forward * attackRange, Color.red);
    }

    public virtual void FindVisibleTarget()
    {
        visibleTarget.Clear();
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTarget.Add(target);
                    transform.GetComponentInChildren<EnemyIsAttack>().LookAt(target);
                    isChasing = true;
                }
            }
        }

        if (visibleTarget.Count == 0)
        {
            LoseEnemy();
        }
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTarget();
        }
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angel;
        float maxAngle = maxViewCast.angel;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveInterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngel)
    {
        Vector3 dir = DirFromAngel(globalAngel, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngel);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngel);
        }
    }

    public virtual void DrawFieldOfView()
    {
    /*
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angel = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angel);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    */
    }

    public Vector3 DirFromAngel(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angel;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angel)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angel = _angel;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

}
