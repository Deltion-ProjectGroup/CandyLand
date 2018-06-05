using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public override void CollectQuest()
    {
        for(int i = 0; i < requiredItems.Length; i++)
        {
            for(int q = 0; q < Inventory.instance.slots.Count; q++)
            {
                if(requiredItems[i].requiredItem == Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>().itemI)
                {
                    Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>().itemAmount -= requiredItems[i].requiredAmt;
                    if(Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>().itemAmount <= 0)
                    {
                        Inventory.instance.slots.RemoveAt(q);
                    }
                }
            }
        }
        base.CollectQuest();
    }
    public override void Interact(GameObject interactor)
    {
        UIManager.uiManager.questStuff[3].GetComponent<Text>().text = null;
        for (int q = 0; q < requiredItems.Length; q++)
        {
            UIManager.uiManager.questStuff[3].GetComponent<Text>().text += requiredItems[q].requiredAmt + " * " + requiredItems[q].requiredItem.itemName + "\n";
        }
        base.Interact(interactor);
    }
}
