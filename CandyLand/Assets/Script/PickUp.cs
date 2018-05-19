using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable {

    public override void Interact(GameObject interactor)
    {
        print("Picked up");
        Destroy(gameObject);
    }
}
