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

    [Header("SecondStage")]
    public Transform secondStage;
    [SerializeField] float amountOfEnemies;


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
        if (health <= 0)
        {
            Death();
        }
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

    public override void Death()
    {
        for (int i = 0; i < amountOfEnemies; i++)
        {
            Vector3 randompos = new Vector3(Random.Range(1, 5), Random.Range(1, 5), Random.Range(1, 5));
            Instantiate(secondStage, transform.position + randompos, transform.rotation);
        }
        base.Death();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            print("i hit the player");
            //gameObject.transform.GetComponentInParent<BossEnemy>().isChasing = true;
            isChasing = true;
        }
    }
}
