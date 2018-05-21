using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New harvestItem", menuName = "Item/HarvestItem")]
public class HarvestItem : Item
{
    [Header("HarvestItem")]
    public int durability;
    public int[] mineID; //The ID's of the items that it can mine
    public int minHarvest;
    public int maxHarvest;

}
