using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableItem : MonoBehaviour
{
    public int requiredID; //Each item has its own ID
    public int health;
    public Item item;

    public void Drop(int minAmt, int maxAmt)
    {
        Inventory.instance.Add(item, Random.Range(minAmt, maxAmt), false);
    }
    public void Death()
    {
        Destroy(gameObject);
    }
}
