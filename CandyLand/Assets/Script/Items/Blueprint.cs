using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Blueprint", menuName = "Item/Pickup/Blueprint")]
public class Blueprint : ScriptableObject {
    public Itehm[] requiredItems;
    public Item craftingItem;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [System.Serializable]
    public class Itehm
    {
        public Item requiredItem;
        public int requiredAmt;
    }
}
