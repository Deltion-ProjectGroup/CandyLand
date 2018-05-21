using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pickup", menuName = "Item/Pickup")]
public class Item : ScriptableObject
{
    [Header("Item")]
    public string itemName;
    public string description;
    public Sprite icon;
    public GameObject itemObject;
    public int amount;
    public int maxAmount;
    public bool isStackable;
    
    // is not necessary ??
    public bool isDefaultItem = false;
}
