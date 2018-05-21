using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IPointerClickHandler
{
    Inventory inventory;
    public bool isSelected;
    public bool selected;
    public Color selectedColor;
    public Color defaultColor;
    public int slotID;
    List<GameObject> slot;
    Image c;
    int id;

    void Start()
    {
        inventory = Inventory.instance;
        transform.localScale = new Vector3(1f, 1f, 1f);
        c = GetComponent<Image>();
        selected = false;
        slot = Inventory.instance.slots;
    }

    public void ChangeColor()
    {
        if (selected)
        {
            if (transform.childCount == 1)
            {
                inventory.invenoryInfoPanel.SetActive(true);
            }
            c.color = selectedColor;
        }
        else if (!selected)
        {
            inventory.invenoryInfoPanel.SetActive(false);
            c.color = defaultColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        id = Inventory.instance.idSlot;

        if (id == slotID)
        {
            selected = false;
            inventory.ClearColorSlots(slotID);
            inventory.slots[id].GetComponent<InventorySlot>().ChangeColor();
            ChangeColor();
        }
        else
        {
            inventory.ClearColorSlots(slotID);
            selected = true;
            ChangeColor();
        }

    }

    // makes the slot bigger for some feedback 
    public void OnPointerEnter(PointerEventData eventData)
    {
        isSelected = true;
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    // makes the slot to the original size for some feedback 
    public void OnPointerExit(PointerEventData eventData)
    {
        isSelected = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        GameObject itemtoAdd = eventData.pointerDrag.transform.gameObject;
        if (Inventory.instance.slots[slotID].transform.childCount == 0)
        {
            droppedItem.slot = slotID;
        }
        else
        {
            int addAmount = droppedItem.itemAmount;
            Transform item = this.transform.GetChild(0);
            if (droppedItem.itemI == item.GetComponent<InventoryItem>().itemI)
            {
                if (droppedItem.itemI.isStackable)
                {
                    if (droppedItem.stackFull)
                    {
                        inventory.Add(droppedItem.itemI, addAmount, false);
                        Destroy(itemtoAdd);
                    }
                }
            }
            else
            {
                item.GetComponent<InventoryItem>().slot = droppedItem.slot;
                item.transform.SetParent(Inventory.instance.slots[droppedItem.slot].transform);
                item.transform.position = Inventory.instance.slots[droppedItem.slot].transform.position;
                item.localScale = new Vector3(1, 1, 1);

                droppedItem.slot = slotID;
                droppedItem.transform.SetParent(this.transform);
                droppedItem.transform.position = this.transform.position;
            }

        }
    }
}

