using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionQuest : Quest {
    public RequiredStuff[] requiredItems;
    bool acquired;
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
                if(Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>() != null)
                {
                    if (Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>().itemI == requiredItems[i].requiredItem)
                    {
                        Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>().itemAmount -= requiredItems[i].requiredAmt;
                        if (Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>().itemAmount <= 0)
                        {
                            Inventory.instance.slots.RemoveAt(q);
                        }
                    }
                }
            }
        }
        base.CollectQuest();
    }
    public override void Interact(GameObject interactor)
    {
        hasItems = false;
        acquired = true;
        UIManager.uiManager.questStuff[3].GetComponent<Text>().text = null;
        for (int q = 0; q < requiredItems.Length; q++)
        {
            UIManager.uiManager.questStuff[3].GetComponent<Text>().text += requiredItems[q].requiredAmt + " * " + requiredItems[q].requiredItem.itemName + "\n";
        }
        base.Interact(interactor);
    }
    public void checkIfComplete()
    {
        for (int i = 0; i < Inventory.instance.slots.Count; i++)
        {
            if (Inventory.instance.slots[i].GetComponentInChildren<InventoryItem>() != null)
            {
                hasItems = true;
                break;
            }
        }
        if (hasItems)
        {
            for (int q = 0; q < requiredItems.Length; q++)
            {
                for (int i = 0; i < Inventory.instance.slots.Count; i++)
                {
                    if (Inventory.instance.slots[i].GetComponentInChildren<InventoryItem>().itemI == requiredItems[q].requiredItem)
                    {
                        break;
                    }
                    acquired = false;
                }
                if (!acquired)
                {
                    break;
                }
            }
            if (acquired)
            {
                inProgress = false;
                completed = true;
            }
        }
    }
    public override void AcceptQuest()
    {
        checkIfComplete();
        base.AcceptQuest();
    }
}
