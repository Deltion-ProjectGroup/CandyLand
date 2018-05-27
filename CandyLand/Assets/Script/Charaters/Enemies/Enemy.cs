using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [HideInInspector] public Transform target;
    public RaycastHit searchRay;
    NavMeshAgent agent;
    Transform pos;

    [Header("isAttacking/isChasing")]
    [HideInInspector] public bool sensfield = false;
    public bool isChasing = false;
    [SerializeField] GameObject chargePos;
    RaycastHit hitPartical;


    [Header("WalkArea")]
    [SerializeField] float randomUnitCircleRadiusMin;
    [SerializeField] float randomUnitCircleRadiusMax;
    [SerializeField] float thinkTimerMin;
    [SerializeField] float thinkTimerMax;
    float thinkTimer;
    public float attackTime = 1f;
    public float attackTimeStart;
    float randomUnitCircleRadius;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        ThinkTimer();
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



    void ThinkTimer()
    {
        thinkTimer -= Time.deltaTime;
        {

            if (attackTime <= 0)
            {
                Attack();
                attackTime = attackTimeStart;
            }

            if (thinkTimer <= 0) // Subtract ThinkTimer over time.
            {
                randomUnitCircleRadius = Random.Range(randomUnitCircleRadiusMin, randomUnitCircleRadiusMax); // Give a random value radius
                RandomPos();
                thinkTimer = Random.Range(thinkTimerMin, thinkTimerMax); // Give a random value thinkTimer
            }
        }
    }
    void RandomPos()
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

    public void SensField()
    {
        Attack();
    }

    public virtual void Attack()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out hitPartical))
        {
            if (hitPartical.transform.tag == ("Terrain"))
            {
                pos = Instantiate(chargePos, target.position, target.rotation).transform;
                Vector3 currentPosition = pos.transform.position;
                currentPosition.y = currentPosition.y + -0.3f; // make a Y pos offset
                pos.transform.position = currentPosition;
            }
        }
        //agent.SetDestination(target.position);
    }
}
