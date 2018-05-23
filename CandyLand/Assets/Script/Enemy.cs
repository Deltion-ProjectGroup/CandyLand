using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public GameObject target;
    public RaycastHit searchRay;

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
    public virtual void Attack()
    {
        print("Attack Player");//Attacks THE FUCKING PLAYER
    }
}
