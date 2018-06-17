using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class CraftingItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    bool acquired;
    public Blueprint craftingBlueprint;
    Vector3 backUpScale;
    Crafting workbench;
	// Use this for initialization
	void Start () {
        workbench = Crafting.crafting;
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
            Crafting.crafting.gameObject.GetComponent<AudioSource>().clip = Crafting.crafting.canCraft;
            Craft(foundHolder);
        }
        else
        {
            Crafting.crafting.gameObject.GetComponent<AudioSource>().clip = Crafting.crafting.cannotCraft;
        }
        Crafting.crafting.gameObject.GetComponent<AudioSource>().Play();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        for(int i = 0; i < Crafting.crafting.craftingStuff.Length; i++)
        {
            Crafting.crafting.craftingStuff[i].SetActive(true);
        }
        transform.localScale = new Vector3(backUpScale.x + 0.05f, backUpScale.y + 0.05f, backUpScale.z + 0.05f);
        Crafting.crafting.craftingStuff[0].GetComponentInChildren<Image>().sprite = craftingBlueprint.craftingItem.icon;
        Crafting.crafting.craftingStuff[1].GetComponent<Text>().text = "Name: " + craftingBlueprint.craftingItem.itemName;
        Crafting.crafting.craftingStuff[2].GetComponent<Text>().text = "Description: " + craftingBlueprint.craftingItem.description;
        Crafting.crafting.craftingStuff[4].GetComponent<Text>().text = null;
        for (int i = 0; i < craftingBlueprint.requiredItems.Length; i++)
        {
            Crafting.crafting.craftingStuff[4].GetComponent<Text>().text += craftingBlueprint.requiredItems[i].requiredItem.itemName + ": " + craftingBlueprint.requiredItems[i].requiredAmt.ToString() + " \n";
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < Crafting.crafting.craftingStuff.Length; i++)
        {
            Crafting.crafting.craftingStuff[i].SetActive(false);
        }
    }
    public void Craft(List<InventoryItem> requiredItem)
    {
        for(int i = 0; i < craftingBlueprint.requiredItems.Length; i++)
        {
            requiredItem[i].itemAmount -= craftingBlueprint.requiredItems[i].requiredAmt;
            requiredItem[i].GetComponentInChildren<Text>().text = requiredItem[i].itemAmount.ToString();
        }
        Inventory.instance.Refresh();
        Inventory.instance.Add(craftingBlueprint.craftingItem, 1, false);
        if (Crafting.crafting.firstAxeCraft && craftingBlueprint.craftingItem.itemName == "StarterAxe")
        {
            Crafting.crafting.firstAxeCraft = false;
            StoryLine.storyLine.storyCase = 9;
            StoryLine.storyLine.Story();
            Inventory.instance.OnTab();
        }
        if (Crafting.crafting.firstPicCraft && craftingBlueprint.craftingItem.itemName == "StarterPickaxe")
        {
            Crafting.crafting.firstPicCraft = false;
            StoryLine.storyLine.storyCase = 15;
            StoryLine.storyLine.Story();
            Inventory.instance.OnTab();
        }
    }
    [System.Serializable]
    public class Itehm
    {
        public Item requiredItem;
        public int requiredAmt;
    }
}
