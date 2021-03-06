﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [Header("Attack")]
    public float damage;

    public RaycastHit searchRay;
    [HideInInspector] public Transform target;
    [HideInInspector] public NavMeshAgent agent;
    Transform pos;

    [Header("isAttacking/isChasing")]
    [SerializeField] float chargeThinkingMin;
    [SerializeField] float chargeThinkingMax;
    float mainchargeThinking;
    public bool isChasing;
    RaycastHit hitPartical;



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

    [HideInInspector] public float distance;

    [Header("WalkField")]
    public float maxDistance;
    public Transform midPoint;

    public virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>(); target = GameObject.FindGameObjectWithTag("Player").transform; 
        isChasing = false;
        midPoint = GameObject.FindGameObjectWithTag("MidPoint").transform;
        StartCoroutine("FindTargetWithDelay", 0.2f);
    }

    public virtual void Update()
    {
        ThinkTimer();
        WalkField();
    }

    public override void Health(float damage)
    {
        base.Health(damage);
    }

    public virtual void WalkField()
    {

    }

    public virtual void RandomPos()
    {
        // Think if Not Chasing
        if (!isChasing)
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
        transform.LookAt(null);
        isChasing = false;
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

                    Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                    transform.GetComponentInChildren<EnemyIsAttack>().Look(targetPosition);

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

    public Vector3 DirFromAngel(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
