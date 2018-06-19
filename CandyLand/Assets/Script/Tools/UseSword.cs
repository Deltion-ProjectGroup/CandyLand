using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSword : UseItem {
    public int dmg;
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
        base.Use();
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Physics.Raycast(GameObject.FindGameObjectWithTag("MainCamera").transform.position, GameObject.FindGameObjectWithTag("MainCamera").transform.forward, out hit, 10);
            if(hit.transform != null)
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<Enemy>().health -= dmg;
                    if (hit.transform.gameObject.GetComponent<Enemy>().health <= 0)
                    {
                        hit.transform.gameObject.GetComponent<Enemy>().Death();
                    }
                    else
                    {
                        //GameObject blood = Instantiate(bloodParticles, hit.point, Quaternion.identity);
                        // blood.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
                        //Destroy(blood, 1);
                    }
                }
            }
        }
    }
    public override void Equip()
    {
        base.Equip();
    }
}
