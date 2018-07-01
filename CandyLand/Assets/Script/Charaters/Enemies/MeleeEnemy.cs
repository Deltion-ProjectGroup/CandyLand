using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
    [Header("Jump")]
    [SerializeField] float minJumpUp;
    [SerializeField] float maxJumpUp;
    [SerializeField] float minJumpForward;
    [SerializeField] float maxJumpForward;
    [SerializeField] float runjumpForward;
    [SerializeField] float minJumpTime;
    [SerializeField] float maxJumpTime;
    [SerializeField] float minSpeedJumpTime;
    [SerializeField] float maxSpeedJumpTime;

    [SerializeField] float speed;
    Vector3 newPosition;

    Animator anim;

    float jumpUp;
    float runJumpForward;
    float jumpForward;

    bool isjumping;
    float jumpTime;

    [Header("MarshMello Pack")]
    public LayerMask allianceMask;
    public List<Transform> visibleAlliance = new List<Transform>();
    RaycastHit searchAlliance;

    public override void Start()
    {
        anim = GetComponentInChildren<Animator>();

        runJumpForward = runjumpForward;
        jumpUp = Random.Range(minJumpUp, maxJumpUp);
        jumpForward = Random.Range(minJumpForward, maxJumpForward);

        base.Start();
        jumpTime = Random.Range(minJumpTime, maxJumpTime);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void Update()
    {
        base.Update();
        isJumping();
    }


    public override void RandomPos()
    {
        if (!isChasing)
        {

            // Pick a random point in the insideUnitCircle for X and Y and set it in a vector3
            Vector3 newPos = transform.position + new Vector3(Random.insideUnitCircle.x * randomUnitCircleRadius, transform.position.y, Random.insideUnitCircle.y * randomUnitCircleRadius);
            // Put the newPos in the setDestination   
            newPosition = new Vector3(newPos.x, transform.position.y, newPos.z);

            transform.LookAt(newPosition);
            // has to jump Jump()
            // disable navmeshAgent
            isjumping = true;
        }
    }

    public override void ThinkTimer()
    {
        base.ThinkTimer();
    }

    void isJumping()
    {
        if (isjumping)
        {
            jumpTime -= Time.deltaTime;
            if (jumpTime <= 0)
            {
                jumpUp = Random.Range(minJumpUp, maxJumpUp);
                if (isChasing)
                {
                    jumpForward = runJumpForward;
                    jumpTime = Random.Range(minSpeedJumpTime, maxSpeedJumpTime);
                }
                else
                {
                    jumpForward = Random.Range(minJumpForward, maxJumpForward);
                    jumpTime = Random.Range(minJumpTime, maxJumpTime);
                }
                anim.SetTrigger("Jump");
                GetComponent<Rigidbody>().AddForce(transform.up * jumpUp * 3);
                GetComponent<Rigidbody>().AddForce(transform.forward * jumpForward * 3);
            }
        }
    }

    public override void WalkField()
    {
        distance = Vector3.Distance(transform.position, midPoint.position);

        if (distance > maxDistance)
        {
            if (!isChasing)
            {
                transform.LookAt(midPoint);
            }
        }
    }

    public override void FindVisibleTarget()
    {
        base.FindVisibleTarget();

        visibleAlliance.Clear();
        Collider[] allianceInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, allianceMask);

        for (int i = 0; i < allianceInViewRadius.Length; i++)
        {
            Transform alliance = allianceInViewRadius[i].transform;
            Vector3 dirToTarget = (alliance.position - transform.position).normalized;


            if ((Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2))
            {
                float dstToTarget = Vector3.Distance(transform.position, alliance.position);

                if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, this.allianceMask))
                {
                    visibleAlliance.Add(alliance);
                    MarshmelloPack(alliance);

                }
            }
        }
    }

    void MarshmelloPack(Transform alliance)
    {
        if (alliance.GetComponentInParent<MeleeEnemy>().isChasing)
        {

            distance = Vector3.Distance(transform.position, alliance.position);
            if (distance < 5)
            {
                Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                transform.LookAt(targetPosition);
            }
            else
            {
                Vector3 alliancePosition = new Vector3(alliance.transform.position.x, transform.position.y, alliance.transform.position.z);
                transform.LookAt(alliancePosition);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Attack();
        }
    }

    void Attack()
    {
        target.GetComponent<Player>().Health(damage);
    }
}
