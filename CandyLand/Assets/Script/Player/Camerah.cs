﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerah : MonoBehaviour
{
    public enum MyEnum
    {
        wood,
        stone,
        iron,
        done
    }
    private Vector3 rotatePos;
    public float rotateMultiplier;
    public float interactionRange;
    public RaycastHit hit;
    // Use this for initialization
    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rotateMultiplier = GetComponentInParent<Player>().rotateMultiplier;
	}
	
	// Update is called once per frame
	void Update ()
    {
        RotateCam(rotatePos, rotateMultiplier);
        if (Input.GetButtonDown("Fire1"))
        {
            FireRaycast(hit);
        }
        Debug.DrawRay(transform.position, transform.forward, Color.cyan, 10);
	}

    public void RotateCam(Vector3 rotator, float speed)
    {
        rotator.x = - Input.GetAxis("Mouse Y");
        transform.Rotate(rotator * speed * Time.deltaTime);
    }

    public void FireRaycast(RaycastHit hitter)
    {
        Physics.Raycast(transform.position, transform.forward, out hitter, interactionRange);
        if(hitter.transform != null)
        {
            if(hitter.transform.gameObject.tag == "Interactable")
            {
                GetComponentInParent<Player>().Interact(hitter);
            }
        }
    }
}