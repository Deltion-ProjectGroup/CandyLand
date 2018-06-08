using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
    public override void Start()
    {
        agent.enabled = true;
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void RandomPos()
    {
        agent.enabled = true;
        // Pick a random point in the insideUnitCircle for X and Y and set it in a vector3
        Vector3 newPos = transform.position + new Vector3(Random.insideUnitCircle.x * randomUnitCircleRadius, transform.position.y, Random.insideUnitCircle.y * randomUnitCircleRadius);
        // Put the newPos in the setDestination
        agent.SetDestination(newPos);
        // has to jump Jump()
        // disable navmeshAgent
        Jump();
    }

    public override void ThinkTimer()
    {
        base.ThinkTimer();
    }

    void Jump()
    {
        agent.enabled = false;
        GetComponent<Rigidbody>().AddForce(-transform.up * 10);
    }
}
