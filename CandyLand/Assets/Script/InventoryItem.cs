using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
    public Item itemI;
    public int itemAmount;
    public bool stackFull;
    public int slot;
    bool isSelected;
    public Sprite icon;
    public Text amountText;

    public void AddItem(Item item, int amount, bool full)
    {
        itemI = item;
        stackFull = full;
        itemAmount = amount;
        icon = itemI.icon;
        gameObject.GetComponent<Image>().sprite = icon;
        amountText.text = itemAmount.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemI != null)
        {
            Inventory.instance.nameInfo.text = itemI.name;
            Inventory.instance.icon.GetComponent<Image>().sprite = itemI.icon;
            Inventory.instance.info.text = itemI.description;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemI != null)
        {
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemI != null)
        {
            this.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(Inventory.instance.slots[slot].transform);
        this.transform.position = Inventory.instance.slots[slot].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true; ;
        transform.localScale = new Vector3(1, 1, 1);

        bool selected = gameObject.GetComponentInParent<InventorySlot>().isSelected;
        if (eventData.pointerDrag == !selected)
        {
            Inventory.instance.Remove(itemI, gameObject, itemAmount, stackFull);
        }
    }

}

