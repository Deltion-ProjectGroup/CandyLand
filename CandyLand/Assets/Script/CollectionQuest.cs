using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionQuest : Quest {
    public List<RequiredStuff> requiredItems = new List<RequiredStuff>();
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
        base.CollectQuest();
        for (int i = 0; i < requiredItems.Count; i++)
        {
            for(int q = 0; q < Inventory.instance.slots.Count; q++)
            {
                if(Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>() != null)
                {
                    if (Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>().itemI == requiredItems[i].requiredItem)
                    {
                        Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>().itemAmount -= requiredItems[i].requiredAmt;
                    }
                }
            }
        }
        Inventory.instance.Refresh();
        if (hasStoryEffect)
        {
            StoryLine.storyLine.storyCase = storyEffectIndex;
            StoryLine.storyLine.Story();
        }
    }
    public override void Interact(GameObject interactor)
    {
        hasItems = false;
        acquired = true;
        UIManager.uiManager.questStuff[3].GetComponent<Text>().text = null;
        for (int q = 0; q < requiredItems.Count; q++)
        {
            UIManager.uiManager.questStuff[3].GetComponent<Text>().text += requiredItems[q].requiredAmt + " * " + requiredItems[q].requiredItem.itemName + "\n";
        }
        checkIfComplete();
        base.Interact(interactor);
    }
    public override void checkIfComplete()
    {
        if (inProgress)
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
                hasItems = false;
                for (int q = 0; q < requiredItems.Count; q++)
                {
                    for (int i = 0; i < Inventory.instance.slots.Count; i++)
                    {
                        if(Inventory.instance.slots[i].GetComponentInChildren<InventoryItem>() != null)
                        {
                            if (Inventory.instance.slots[i].GetComponentInChildren<InventoryItem>().itemI == requiredItems[q].requiredItem)
                            {
                                if(Inventory.instance.slots[i].GetComponentInChildren<InventoryItem>().itemAmount >= requiredItems[q].requiredAmt)
                                {
                                    print(requiredItems[q].requiredAmt);
                                    print(Inventory.instance.slots[i].GetComponentInChildren<InventoryItem>().itemAmount);
                                    acquired = true;
                                    break;
                                }
                                acquired = false;
                            }
                            acquired = false;
                        }
                    }
                }
                if (!acquired)
                {
                    completed = false;
                }
                if (acquired)
                {
                    print("Disabled");
                    completed = true;
                }
            }
            else
            {
                completed = false;
            }
        }
    }
}
