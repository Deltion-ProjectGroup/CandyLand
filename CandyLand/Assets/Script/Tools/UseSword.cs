using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSword : UseItem {

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
            Physics.Raycast(GameObject.FindGameObjectWithTag("Camera").transform.position, GameObject.FindGameObjectWithTag("Camera").transform.forward, out hit, 10);
        }
    }
    public override void Equip()
    {
        base.Equip();
    }
}
