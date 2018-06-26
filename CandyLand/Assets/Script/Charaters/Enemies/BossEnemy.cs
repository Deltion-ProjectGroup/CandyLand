using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("BossEnemy")]
    [SerializeField] float walkSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] Transform attackPos;
    [SerializeField] float attackRange;
    RaycastHit hit;

    [Header("WalkField")]
    [SerializeField] float maxDistance;
    [SerializeField] Transform midPoint;

    public override void Start()
    {
        base.Start();
        agent.speed = 1.5f;
    }

    public override void Update()
    {
        base.Update();
        ChasePos();
        isAttacking();
    }

    public override void ThinkTimer()
    {
        base.ThinkTimer();
    }

    public override void RandomPos()
    {
        base.RandomPos();
    }

    public override void WalkField()
    {
        distance = Vector3.Distance(transform.position, midPoint.position);

        if (distance > maxDistance)
        {
            if (!isChasing)
            {
                agent.SetDestination(midPoint.position);
            }
        }
    }

    void isAttacking()
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

    void ChasePos()
    {
        if (isChasing)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(target.position);
        }
    }

    public override void LoseEnemy()
    {
        base.LoseEnemy();
        agent.speed = walkSpeed;
    }
}
