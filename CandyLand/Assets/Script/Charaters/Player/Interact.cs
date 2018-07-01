using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    RaycastHit hit;
    Item itemInfo;
    [SerializeField] Transform cameraPosition;
    [SerializeField] float raycastLength;
    [SerializeField] GameObject press;
    [SerializeField] Text infoItem;
    bool inventoryActive;


    void Update()
    {
        InveractObj();
        inventoryActive = gameObject.GetComponentInChildren<Inventory>().inventorySwitch;
    }

    void InveractObj()
    {
        if (Physics.Raycast(cameraPosition.position, cameraPosition.forward, out hit, raycastLength))
        {
            GameObject item = hit.transform.gameObject;

            if (inventoryActive)
            {
                press.SetActive(false);
                infoItem.text = null;
            }
            else if (!inventoryActive)
            {
                if(item.GetComponent<Interactable>() != null)
                {
                    if (hit.transform.tag == "Interactable")
                    {
                        press.SetActive(true);
                        infoItem.text = item.GetComponent<Interactable>().item.name;
                        if (Input.GetButtonDown("Press[E]"))
                        {
                            print("E");
                            item.GetComponent<Interactable>().Interact(gameObject);
                        }
                    }
                }
                else
                {
                    press.SetActive(false);
                }
            }
        }
        else
        {
            press.SetActive(false);
        }
        //Debug.DrawRay(cameraPosition.position, cameraPosition.forward * 2, Color.red);
    }
}

