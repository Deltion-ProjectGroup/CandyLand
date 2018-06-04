using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionQuest : Quest {
    public RequiredStuff[] requiredItems;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [System.Serializable]
    public class RequiredStuff
    {
        public Item requiredItem;
        public int requiredAmt;
    }
}
