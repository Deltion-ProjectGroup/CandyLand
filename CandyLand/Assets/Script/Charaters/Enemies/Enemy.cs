using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public GameObject target;
    public RaycastHit searchRay;
    NavMeshAgent agent;
    public bool isChasing = false;

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
    }

    void Update()
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
                randomUnitCircleRadius = Random.Range(randomUnitCircleRadiusMin, randomUnitCircleRadiusMax);
                Think();
                thinkTimer = Random.Range(thinkTimerMin, thinkTimerMax);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            target = other.gameObject;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = null;
        }
    }
    #region Normal Script
    /*
    public virtual void SetTarget(GameObject target)
    {
        gameObject.GetComponent<NavMeshAgent>().SetDestination(target.transform.position);//Sets the destination of the navMeshAgent to the sensed Player
        //agent.SetDestination(target.transform.position);
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


    private void Think()
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

    public virtual void Attack()
    {
        print("Attack Player");//Attacks THE FUCKING PLAYER
    }
}
