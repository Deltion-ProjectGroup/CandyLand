using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    public Item item;
    public int amount;
    public bool stackFull;


    #region Test 
    public void Pickup()
    {
        bool wasPickedUp = Inventory.instance.Add(item, amount, stackFull);
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
    #endregion
    /*
    public override void Interact(GameObject interactor)
    {
        bool wasPickedUp = Inventory.instance.Add(item, amount, stackFull);
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
    */
}
