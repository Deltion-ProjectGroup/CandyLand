using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    # region Singleton
    public static Inventory instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Prefabs")]
    [SerializeField] GameObject inventoryPanel;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    [Header("InfoPanel")]
    public GameObject invenoryInfoPanel;
    public Image icon;
    public Text nameInfo;
    public Text info;

    //Camera Movement
    Player playerSpeed;
    Camerah camemraSpeed;

    //[HideInInspector] public delegate void OnItemChanged();
    //[HideInInspector] public OnItemChanged onItemChangedCallback;
    [HideInInspector] public List<GameObject> slots = new List<GameObject>();
    [HideInInspector] public bool inventorySwitch;
    [HideInInspector] public int idSlot;

    GameObject slotPanel;
    InventoryItem ivi;
    Item itemCal;
    Item itemSlot;
    int slotAmount;
    int itemAmount;
    int amountI;
    int addAmount;
    bool stackFull;
    bool slotAvailable;
    bool maxAmount;
    bool switchSlot;
    bool beginswitch;

    void Start()
    {
        switchSlot = false;
        beginswitch = true;
        slotAmount = 15;
        CreateSlots(slotAmount);
        playerSpeed = GetComponent<Player>();
        camemraSpeed = gameObject.GetComponentInChildren<Camerah>();
        inventoryPanel.SetActive(false);
        invenoryInfoPanel.SetActive(false);
        slotAvailable = true;
        maxAmount = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Tab"))
        {
            if (Crafting.crafting.isCrafting)
            {
                Crafting.crafting.craftingUI.SetActive(false);
                Crafting.crafting.isCrafting = false;
            }
            if (Quest.interactedQuest)
            {
                Quest.interactedQuest = false;
                UIManager.uiManager.questStuff[UIManager.uiManager.questStuff.Length - 1].SetActive(false);
            }
            InventoryOnOff();
        }
    }

    public void InventoryOnOff()
    {
        inventorySwitch = !inventorySwitch;

        if (inventorySwitch)
        {
            inventoryPanel.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            playerSpeed.rotateMultiplier = 0;
            camemraSpeed.rotateMultiplier = 0;
        }
        else if (!inventorySwitch)
        {
            inventoryPanel.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            playerSpeed.rotateMultiplier = playerSpeed.rotateMultiplierBackUp;
            camemraSpeed.rotateMultiplier = playerSpeed.rotateMultiplierBackUp;
            switchSlot = true;
            ClearColorSlots(idSlot);
        }
    }

    public void ClearColorSlots(int id)
    {
        #region ClearAllSlots
        if (!inventoryPanel)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].GetComponent<InventorySlot>().selected)
                {
                    slots[i].GetComponent<InventorySlot>().selected = false;
                    slots[i].GetComponent<InventorySlot>().ChangeColor();
                }
            }
        }
        if (switchSlot)
        {
            slots[idSlot].GetComponent<InventorySlot>().selected = false;
            slots[idSlot].GetComponent<InventorySlot>().ChangeColor();
            switchSlot = false;
        }
        #endregion

        else
        {
            if (beginswitch)
            {
                idSlot = id;
                slots[idSlot].GetComponent<InventorySlot>().selected = true;
                beginswitch = false;
            }
            else
            {
                slots[idSlot].GetComponent<InventorySlot>().selected = false;
                slots[idSlot].GetComponent<InventorySlot>().ChangeColor();
                idSlot = id;
                slots[idSlot].GetComponent<InventorySlot>().selected = true;
                slots[idSlot].GetComponent<InventorySlot>().ChangeColor();
            }
        }
    }

    public int amountCalculation(int f)
    {
        f = itemAmount + amountI;
        return f;
    }

    public int slotCalculatoin(int c)
    {
        c = (itemAmount + amountI - itemCal.maxAmount);
        return c;
    }


    public bool Add(Item item, int amount, bool full)
    {
        stackFull = full;
        amountI = amount;
        itemCal = item;
        itemAmount = 0;

        if (!item.isDefaultItem)
        {
            for (int sl = 0; sl < slots.Count; sl++)
            {
                if (slots[sl].transform.childCount == 1)
                {
                    itemSlot = slots[sl].transform.GetComponentInChildren<InventoryItem>().itemI;
                    itemAmount = slots[sl].transform.GetComponentInChildren<InventoryItem>().itemAmount;
                    ivi = slots[sl].transform.GetComponentInChildren<InventoryItem>();

                    if (itemSlot == item)
                    {
                        slotAvailable = false;
                        if (item.isStackable)
                        {
                            if (!ivi.stackFull)
                            {
                                if (amountCalculation(amountI) <= itemSlot.maxAmount)
                                {
                                    stackFull = false;
                                    inventoryItem.GetComponent<InventoryItem>().slot = sl;
                                    slots[sl].GetComponentInChildren<InventoryItem>().AddItem(item, amountCalculation(amountI), stackFull);
                                    maxAmount = false;
                                    break;
                                }

                                if (amountCalculation(amountI) >= itemSlot.maxAmount)
                                {
                                    stackFull = true;
                                    slots[sl].GetComponentInChildren<InventoryItem>().AddItem(item, itemSlot.maxAmount, stackFull);
                                    maxAmount = true;
                                    slotAvailable = true;
                                    break;
                                }
                            }
                            else
                            {
                                slotAvailable = true;
                            }
                        }
                        else
                        {
                            slotAvailable = true;
                        }
                    }
                    else
                    {
                        slotAvailable = true;
                    }
                }
                else
                {
                    slotAvailable = true;
                }
            }
            if (slotAvailable)
            {
                for (int sl = 0; sl < slots.Count; sl++)
                {
                    if (slots[sl].transform.childCount == 0)
                    {
                        if (maxAmount)
                        {
                            stackFull = false;
                            inventoryItem.GetComponent<InventoryItem>().AddItem(item, slotCalculatoin(addAmount), stackFull);
                            maxAmount = false;
                        }
                        else
                        {
                            stackFull = false;
                            inventoryItem.GetComponent<InventoryItem>().AddItem(item, amountI, stackFull);
                            maxAmount = false;
                        }
                        inventoryItem.GetComponent<InventoryItem>().slot = sl;
                        GameObject itemObj = Instantiate(inventoryItem);
                        itemObj.transform.SetParent(slots[sl].transform, false);
                        break;
                    }
                }
            }

            #region necessary!?
            /*
            // i have to take a look if it is even necessary
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
            */
            #endregion
        }
        return true;
    }


    public void Remove(Item finalItem, GameObject obj, int finalAmount, bool full)
    {
        GameObject itemRemove = Instantiate(finalItem.itemObject, transform.position, transform.rotation);
        itemRemove.GetComponent<PickUp>().item = finalItem;
        itemRemove.GetComponent<PickUp>().amount = finalAmount;
        Destroy(obj);
    }

    void CreateSlots(int slotAmount)
    {
        inventoryPanel = GameObject.FindGameObjectWithTag("InventoryPanel");
        slotPanel = GameObject.FindGameObjectWithTag("SlotPanel");
        for (int s = 0; s < slotAmount; s++)
        {
            slots.Add(Instantiate(inventorySlot));
            slots[s].GetComponent<InventorySlot>().slotID = s;
            slots[s].transform.SetParent(slotPanel.transform);
        }
    }
}

