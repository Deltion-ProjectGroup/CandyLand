﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTool : UseItem
{
    [SerializeField] float raylenght;
    Animator anim;
    RaycastHit hit;
    [SerializeField] GameObject partical;

    public HarvestItem harvestSO;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") || Inventory.instance.inventorySwitch)
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
        Harvest();
        anim.SetTrigger("Pick");
    }

    void Harvest()
    {
        if (Physics.Raycast(GameObject.FindGameObjectWithTag("MainCamera").transform.position, GameObject.FindGameObjectWithTag("MainCamera").transform.forward, out hit, 0.9f))
        {
            print("Raycasted");
            if (hit.transform.gameObject.tag == "Harvestable")
            {
                print("JEEEZZ");    
                if (canUse != true)// set to true after we have animations, right now it is just on collision
                {
                    for (int i = 0; i < harvestSO.mineID.Length; i++)
                    {
                        if (harvestSO.mineID[i] == hit.transform.GetComponent<HarvestableItem>().requiredID) //Each resource has its own ID value
                        {
                            GameObject n = Instantiate(partical, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                            n.transform.parent = hit.transform;
                            hit.transform.GetComponent<HarvestableItem>().Drop(harvestSO.minHarvest, harvestSO.maxHarvest + 1);
                            hit.transform.GetComponent<HarvestableItem>().health -= Random.Range(harvestSO.minHarvest, harvestSO.maxHarvest);
                            if (hit.transform.GetComponent<HarvestableItem>().health <= 0)
                            {
                                hit.transform.GetComponent<HarvestableItem>().Death();
                            }
                            return;
                        }
                    }
                    StartCoroutine(Camerah.camerah.ScreenShake(0.05f));
                }
            }
        }
    }
    /*
    public void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Harvestable")
        {
            if (canUse != false)// set to true after we have animations, right now it is just on collision
            {
                for (int i = 0; i < harvestSO.mineID.Length; i++)
                {
                    if (harvestSO.mineID[i] == hit.gameObject.GetComponent<HarvestableItem>().requiredID) //Each resource has its own ID value
                    {
                        print("Mined");
                        hit.gameObject.GetComponent<HarvestableItem>().Drop(harvestSO.minHarvest, harvestSO.maxHarvest);
                        hit.gameObject.GetComponent<HarvestableItem>().health -= Random.Range(1, 11);
                        if (hit.gameObject.GetComponent<HarvestableItem>().health <= 0)
                        {
                            hit.gameObject.GetComponent<HarvestableItem>().Death();
                        }
                    }
                }
            }
        }
    }
    */
    public override void Equip()
    {
        GameObject tool = Instantiate(gameObject, GameObject.FindGameObjectWithTag("ToolPoint").transform.position,  new Quaternion(-89.98f, 0, 97.86001f, 0), GameObject.FindGameObjectWithTag("MainCamera").transform);
        Inventory.instance.equippedItem = tool;
        base.Equip();
    }
}


