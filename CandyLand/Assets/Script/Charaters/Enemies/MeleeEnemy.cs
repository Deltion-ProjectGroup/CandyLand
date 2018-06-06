using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Character
{
    NavMeshAgent agent;
    [SerializeField] float randomUnitCircleRadiusMin;
    [SerializeField] float randomUnitCircleRadiusMax;
    [SerializeField] float thinkTimerMin;
    [SerializeField] float thinkTimerMax;
    float thinkTimer;
    float randomUnitCircleRadius;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        ThinkTimer();
    }

    void ThinkTimer()
    {
        thinkTimer -= Time.deltaTime;
        {
            // Subtract ThinkTimer over time.
            if (thinkTimer <= 0)
            {
                // Give a random value radius
                randomUnitCircleRadius = Random.Range(randomUnitCircleRadiusMin, randomUnitCircleRadiusMax);
                // Give a random value thinkTimer
                Walking();
                thinkTimer = Random.Range(thinkTimerMin, thinkTimerMax);
            }
        }
    }

    void Walking()
    {
        // Pick a random point in the insideUnitCircle for X and Y and set it in a vector3
        Vector3 newPos = transform.position + new Vector3(Random.insideUnitCircle.x * randomUnitCircleRadius, transform.position.y, Random.insideUnitCircle.y * randomUnitCircleRadius);
        // Put the newPos in the setDestination
        agent.SetDestination(newPos);
        // has to jump Jump()
        // disable navmeshAgent
    }

    void Jump()
    {

    }
}
