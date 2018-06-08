using System.Collections;
using System.Collections.Generic;
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
    [HideInInspector] public bool sensfield = false;
    [HideInInspector] public bool isChasing = false;
    float mainchargeThinking;
    RaycastHit hitPartical;
    RaycastHit hit;


    [Header("SensField")]
    [SerializeField] float sensfieldTimer;
    float mainsensfieldTimer;

    [Header("WalkArea")]
    [SerializeField] float randomUnitCircleRadiusMin;
    [SerializeField] float randomUnitCircleRadiusMax;
    [SerializeField] float thinkTimerMin;
    [SerializeField] float thinkTimerMax;
    public float attackTime = 1f;
    public float attackTimeStart;
    float thinkTimer;
    public float randomUnitCircleRadius;

    public virtual void Start()
    {
        mainsensfieldTimer = sensfieldTimer;
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = 1.5f;
    }

    public virtual void Update()
    {
        ThinkTimer();
        isAttacking();
    }

    #region Normal Script
    /*
    public virtual void SetTarget(GameObject target)
    {
        gameObject.GetComponent<NavMeshAgent>().SetDestination(target.transform.position);//Sets the destination of the navMeshAgent to the sensed Player
        Physics.Raycast(transform.position, transform.forward, out searchRay, 3);//Shoots out a raycast
        if(searchRay.transform != null)//Checks if it at least hit an object with the raycast
        {
            if(searchRay.transform.gameObject.tag == "Player")//Checks if the hit object is a Player
            {
                Attack();// Plays the attack void
            }
        }
    }
    */
    #endregion 



    public virtual void ThinkTimer()
    {
        // if the player is detected
        if (sensfield)
        {
            mainsensfieldTimer -= Time.deltaTime;

            if (mainsensfieldTimer <= 0)
            {
                mainsensfieldTimer = sensfieldTimer;
                sensfield = false;
            }
        }
        // the enemy has to think befor he can charged
        else if (isChasing)
        {
            mainchargeThinking = Random.Range(chargeThinkingMin, chargeThinkingMax);
            mainchargeThinking -= Time.deltaTime;
            if (mainchargeThinking <= 0)
            {
                mainchargeThinking = 0;
                DistanceAttack();
            }
            //isChasing = false;
        }
        else // it pickes a random pos out of the radius  
        {
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

    public virtual void SensField()
    {
        DistanceAttack();
    }

    public virtual void DistanceAttack()
    {
        if (!isChasing)
        {
            agent.isStopped = true;
            isChasing = true;
        }
        agent.isStopped = false;
        agent.speed = 3;
        agent.SetDestination(target.position);
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
}
