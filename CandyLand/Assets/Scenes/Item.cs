using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item : ScriptableObject {

    public string itemName;
    public string description;
    public Sprite icon;
    public GameObject itemObject;
    public int amount;
    public int maxAmount;
    public bool isStackable;
    public Animation animation;
}
