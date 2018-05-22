using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    
	// Use this for initialization
	void Start ()
    {
        GetComponent<NavMeshAgent>().SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
