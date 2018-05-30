using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : Interactable
{
    public bool isCrafting;
    public GameObject craftingUI;
    public GameObject craftingSlotHolder; //To find the components in the UI that will show the cost etc. later
    public GameObject craftingSlot;
    public GameObject craftingImg;
    public static Crafting crafting;
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
        theSlot.transform.SetParent(craftingSlotHolder.transform);
    }
}
