using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Tooltip("Add the item for the NPC in, this will let it show the name")]
    public Item item;
    public virtual void Interact(GameObject interactor)
    {

    }
}
