using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    private Vector3 movePos;
    public float speedMultiplier = 1;
    private Vector3 camRotate;
    public float rotateMultiplier = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RotateCam(camRotate, rotateMultiplier);
        Movement(movePos, speedMultiplier);
	}
    public override void Movement(Vector3 mover, float speed)
    {
        mover.x = Input.GetAxis("Horizontal");
        mover.z = Input.GetAxis("Vertical");
        transform.Translate(mover * speed * Time.deltaTime);
    }
    public void Interact(RaycastHit hitObj)
    {
        hitObj.transform.gameObject.GetComponent<Interactable>().Interact(gameObject);
    }
    public void RotateCam(Vector3 rotator, float speed)
    {
        rotator.y = Input.GetAxis("Mouse X");
        transform.Rotate(rotator * speed * Time.deltaTime);
    }
}
