using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTool : UseItem
{
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
        if (Input.GetButtonDown("Fire1"))
        {
            if (!Inventory.instance.inventorySwitch)
            {
                print("Fire");
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
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2))
        {
            if (hit.transform.tag == "Harvestable")
            {
                print(hit);
                GameObject n = Instantiate(partical, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                n.transform.parent = hit.transform;
                if (canUse != false)// set to true after we have animations, right now it is just on collision
                {
                    for (int i = 0; i < harvestSO.mineID.Length; i++)
                    {
                        if (harvestSO.mineID[i] == hit.transform.GetComponent<HarvestableItem>().requiredID) //Each resource has its own ID value
                        {
                            print("Mined");
                            hit.transform.GetComponent<HarvestableItem>().Drop();
                            hit.transform.GetComponent<HarvestableItem>().health -= Random.Range(1, 11);
                            if (hit.transform.GetComponent<HarvestableItem>().health <= 0)
                            {
                                hit.transform.GetComponent<HarvestableItem>().Death();
                            }
                        }
                    }
                }
            }
            Debug.DrawRay(transform.position, transform.forward * 2, Color.yellow);
        }
    }

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
                        hit.gameObject.GetComponent<HarvestableItem>().Drop();
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
}

