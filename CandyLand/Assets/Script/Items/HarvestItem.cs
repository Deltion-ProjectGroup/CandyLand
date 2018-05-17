using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New harvestItem", menuName = "Item/HarvestItem")]
public class HarvestItem : Item {
    [Header("HarvestItem")]
    public int durability;
    public CanMin[] canMine;
    public enum Minin { Stone, Iron};
    public int minHarvest;
    public int maxHarvest;

}
[System.Serializable]
public class CanMin
{
    public enum CanMine {Wood, Stone, Metal, Iron};
    public CanMine canMine;
}
