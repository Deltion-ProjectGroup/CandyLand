using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTool : UseItem {
    public HarvestItem harvestSO;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!Inventory.instance.inventorySwitch)
            {
                Use();
            }
        }
	}
    public override void Use()
    {
        base.Use();
        //anim.Play();
    }
    public void OnTriggerEnter(Collider hit)
    {
        if(hit.gameObject.tag == "Harvestable")
        {
            if(canUse != false)// set to true after we have animations, right now it is just on collision
            {
                for (int i = 0; i < harvestSO.mineID.Length; i++)
                {
                    if(harvestSO.mineID[i] == hit.gameObject.GetComponent<HarvestableItem>().requiredID) //Each resource has its own ID value
                    {
                        print("Mined");
                        hit.gameObject.GetComponent<HarvestableItem>().Drop();
                        hit.gameObject.GetComponent<HarvestableItem>().health -= Random.Range(1, 11);
                        if(hit.gameObject.GetComponent<HarvestableItem>().health <= 0)
                        {
                            hit.gameObject.GetComponent<HarvestableItem>().Death();
                        }
                    }
                }
            }
        }
    }
}
