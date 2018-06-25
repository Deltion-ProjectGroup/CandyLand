using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSword : UseItem {
    public Melee weaponStats;
    public GameObject bloodParticles;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (canUse)
        {
            Use();
        }
	}
    public override void Use()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            print("Used");
            RaycastHit hit;
            Physics.Raycast(GameObject.FindGameObjectWithTag("MainCamera").transform.position, GameObject.FindGameObjectWithTag("MainCamera").transform.forward, out hit, weaponStats.range);
            if(hit.transform != null)
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<Enemy>().health -= weaponStats.dmg;
                    if (hit.transform.gameObject.GetComponent<Enemy>().health <= 0)
                    {
                        hit.transform.gameObject.GetComponent<Enemy>().Death();
                        StartCoroutine(Camerah.camerah.ScreenShake());
                    }
                    else
                    {
                        GameObject blood = Instantiate(bloodParticles, hit.point, Quaternion.identity);
                        blood.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
                        blood.transform.SetParent(hit.transform);
                        Destroy(blood, 4);
                    }
                }
            }
            base.Use();
        }
    }
    public override void Equip()
    {
        GameObject tool = Instantiate(gameObject, GameObject.FindGameObjectWithTag("ToolPoint").transform.position, Quaternion.identity);
        tool.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform);
        Inventory.instance.equippedItem = tool;
        base.Equip();
    }
}
