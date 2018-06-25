﻿using System.Collections;
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

    float jumpUp;
    float runJumpForward;
    float jumpForward;

    bool isjumping;
    float jumpTime;

    [Header("WalkField")]
    [SerializeField] float maxDistance;
    [SerializeField] Transform midPoint;
    float distance;

    [Header("MarshMello Pack")]
    public LayerMask allianceMask;
    public List<Transform> visibleAlliance = new List<Transform>();
    RaycastHit searchAlliance;

    public override void Start()
    {
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
        WalkField();
        isJumping();
    }

    public override void RandomPos()
    {
        if (!isChasing)
        {
            // Pick a random point in the insideUnitCircle for X and Y and set it in a vector3
            Vector3 newPos = transform.position + new Vector3(Random.insideUnitCircle.x * randomUnitCircleRadius, transform.position.y, Random.insideUnitCircle.y * randomUnitCircleRadius);
            // Put the newPos in the setDestination   
            Vector3 newPosition = new Vector3(newPos.x, transform.position.y, newPos.z);
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
                GetComponent<Rigidbody>().AddForce(transform.up * jumpUp * 3);
                GetComponent<Rigidbody>().AddForce(transform.forward * jumpForward * 3);
            }
        }
    }

    void WalkField()
    {
        //transform.GetComponentInChildren<MidArea>().Mid(midPoint);

        distance = Vector3.Distance(transform.position, midPoint.position);

        if (distance > maxDistance)
        {
            if (!isChasing)
            {
                Vector3 midPosition = new Vector3(midPoint.transform.position.x, transform.position.y, midPoint.transform.position.z);
                transform.LookAt(midPosition);
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
    public override void ChasePos()
    {
        //nothing
    }
    public override void isAttacking()
    {
        //nothing
    }

    void MarshmelloPack(Transform alliance)
    {
        if (isChasing)
        {
            Vector3 alliancePosition = new Vector3(alliance.transform.position.x, transform.position.y, alliance.transform.position.z);
            transform.LookAt(alliancePosition);
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
        print("Hit");
    }
}
