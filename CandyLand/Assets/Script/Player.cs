using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    private Vector3 movePos;
    public float speedMultiplier = 1;
    private Vector3 camRotate;
    public float rotateMultiplier = 1;
    public Vector3 jumpAmt;
    private bool canJump = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RotateCam(camRotate, rotateMultiplier);
        Movement(movePos, speedMultiplier);
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
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
    public void Jump()
    {
        if(canJump)
        {
            canJump = false;
            gameObject.GetComponent<Rigidbody>().velocity = jumpAmt;
        }
    }
    public void OnCollisionEnter(Collision hit)
    {
        if(hit.gameObject.tag == "Terrain")
        {
            canJump = true;
        }
    }
}
