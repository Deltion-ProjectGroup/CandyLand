using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : Interactable
{
    public bool isCrafting;
    public GameObject craftingUI;
    public GameObject craftingSlotHolder; //To find the components in the UI that will show the cost etc. later
    public GameObject craftingSlot;
    public GameObject[] craftingStuff; // 0 is image, 1 is itemName, 2 is itemDescription, 4 is for the requiredItemText, 
    public static Crafting crafting;
    public AudioClip cannotCraft;
    public AudioClip canCraft;
	// Use this for initialization
	void Start ()
    {
        crafting = this;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}
    public override void Interact(GameObject interactor)
    {
        if(isCrafting != true)
        {
            isCrafting = true;
            craftingUI.SetActive(true);
            Inventory.instance.InventoryOnOff();
        }
    }
    public void AddBlueprint(Blueprint blueprint)
    {
        GameObject theSlot = Instantiate(craftingSlot, Vector3.zero, Quaternion.identity);
        theSlot.GetComponent<CraftingItem>().craftingBlueprint = blueprint;
        theSlot.GetComponent<Image>().sprite = blueprint.craftingItem.icon;
        theSlot.transform.SetParent(craftingSlotHolder.transform);
    }
}
