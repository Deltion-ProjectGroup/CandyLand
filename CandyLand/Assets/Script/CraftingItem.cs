using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftingItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Itehm[] requiredItems;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 spawner = new Vector3(546.1f, -176);
        transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        for(int i = 0; i < requiredItems.Length; i++)
        {
            Crafting.crafting.craftingUI.GetComponentInChildren<Text>().text += requiredItems[i].requiredItem.item.itemName + ": " + requiredItems[i].requiredAmt.ToString() + " /n";
            spawner.x += 5;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Crafting.crafting.craftingUI.GetComponentInChildren<Text>().text = null;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
    [System.Serializable]
    public class Itehm
    {
        public PickUp requiredItem;
        public int requiredAmt;
    }
}
