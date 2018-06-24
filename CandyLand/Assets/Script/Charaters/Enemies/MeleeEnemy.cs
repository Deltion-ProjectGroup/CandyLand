using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
    [Header("Jump")]
    [SerializeField] float jumpUp;
    [SerializeField] float jumpForward;
    [SerializeField] float jumptime;
    bool isjumping;
    float jumpTime;

    [Header("WalkField")]
    [SerializeField] float maxDistance;
    [SerializeField] Transform midPoint;
    float distance;

    [Header("MarshMello Pack")]
    RaycastHit searchAlliance;

    public override void Start()
    {
        jumpTime = jumptime;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void Update()
    {
        base.Update();
        WalkField();
        isJumping();
    }

    public override void RandomPos()
    {
        // Pick a random point in the insideUnitCircle for X and Y and set it in a vector3
        Vector3 newPos = transform.position + new Vector3(Random.insideUnitCircle.x * randomUnitCircleRadius, transform.position.y, Random.insideUnitCircle.y * randomUnitCircleRadius);
        // Put the newPos in the setDestination      
        transform.LookAt(newPos);
        // has to jump Jump()
        // disable navmeshAgent
        isjumping = true;
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
                jumpTime = jumptime;
                GetComponent<Rigidbody>().AddForce(transform.up * jumpUp * 3);
                GetComponent<Rigidbody>().AddForce(transform.forward * jumpForward * 3);
            }
        }
    }

    void WalkField()
    {
        transform.GetComponentInChildren<MidArea>().Mid(midPoint);

        distance = Vector3.Distance(transform.position, midPoint.position);

        if (distance > maxDistance)
        {
            transform.LookAt(midPoint);
        }
    }

    public override void FindVisibleTarget()
    {
        base.FindVisibleTarget();
    }

    void MarshmelloPack()
    {

    }
}
