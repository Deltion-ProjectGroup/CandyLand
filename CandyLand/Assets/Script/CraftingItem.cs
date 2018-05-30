using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftingItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    bool acquired;
    public Blueprint craftingBlueprint;
    Vector3 backUpScale;
	// Use this for initialization
	void Start () {
        backUpScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnPointerClick(PointerEventData eventData)
    {
        bool canCraft = true;
        List<InventoryItem> foundHolder = new List<InventoryItem>();
        for(int i = 0; i < craftingBlueprint.requiredItems.Length; i++)
        {
            for (int q = 0; q < Inventory.instance.slots.Count; q++)
            {
                if (Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>() != null)
                {
                    if (Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>().itemI == craftingBlueprint.requiredItems[q].requiredItem)
                    {
                        if (Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>().itemAmount >= craftingBlueprint.requiredItems[q].requiredAmt)
                        {
                            acquired = true;
                            foundHolder.Add(Inventory.instance.slots[q].GetComponentInChildren<InventoryItem>());
                            break;
                        }
                    }
                }
            }
            if (!acquired)
            {
                canCraft = false;
            }
            acquired = false;
        }
        if (canCraft)
        {
            Craft(foundHolder);
            print("crafted");
        }
        else
        {
            print("Wtf");
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Crafting.crafting.craftingImg.GetComponent<Image>().enabled = true;
        Vector3 spawner = new Vector3(546.1f, -176);
        transform.localScale = new Vector3(backUpScale.x + 0.05f, backUpScale.y + 0.05f, backUpScale.z + 0.05f);
        Crafting.crafting.craftingImg.GetComponent<Image>().sprite = craftingBlueprint.craftingItem.icon;
        for (int i = 0; i < craftingBlueprint.requiredItems.Length; i++)
        {
            Crafting.crafting.craftingUI.GetComponentInChildren<Text>().text += craftingBlueprint.requiredItems[i].requiredItem.itemName + ": " + craftingBlueprint.requiredItems[i].requiredAmt.ToString() + " \n";
            spawner.x += 5;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Crafting.crafting.craftingImg.GetComponent<Image>().enabled = false;
        Crafting.crafting.craftingUI.GetComponentInChildren<Text>().text = null;
        Crafting.crafting.craftingImg.GetComponent<Image>().sprite = null;
        transform.localScale = backUpScale;
    }
    public void Craft(List<InventoryItem> requiredItem)
    {
        for(int i = 0; i < craftingBlueprint.requiredItems.Length; i++)
        {
            requiredItem[i].itemAmount -= craftingBlueprint.requiredItems[i].requiredAmt;
            requiredItem[i].GetComponentInChildren<Text>().text = requiredItem[i].itemAmount.ToString();
        }
        Inventory.instance.Add(craftingBlueprint.craftingItem, 1, false);
    }
    [System.Serializable]
    public class Itehm
    {
        public Item requiredItem;
        public int requiredAmt;
    }
}
