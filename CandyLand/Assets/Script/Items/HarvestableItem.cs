using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableItem : MonoBehaviour {
    public int requiredID; //Each item has its own ID
    public int health;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Drop()
    {

    }
    public void Death()
    {
        Destroy(gameObject);
    }
}
