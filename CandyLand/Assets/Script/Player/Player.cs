using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    [Header("Movement")]
    Vector3 movePos;
    public float walkSpeed;
    public float sprintSpeed;
    float baseWalkSpeed;
    float stamina = 100;
    float maxStamina = 100;
    public float sprintCost;
    [Header("Camera")]
    Vector3 camRotate;
    public float rotateMultiplier = 1;
    [Header("Jumping")]
    public Vector3 jumpAmt;
    bool canJump = true;
	// Use this for initialization
	void Start ()
    {
        baseWalkSpeed = walkSpeed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        RotateCam(camRotate, rotateMultiplier);
        Movement(movePos, walkSpeed);
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButton("Sprint"))
        {
            if(stamina >= sprintCost)
            {
                if(walkSpeed != sprintSpeed)
                {
                    walkSpeed = sprintSpeed;
                }
                stamina -= sprintCost;
            }
        }
        else
        {
            if(walkSpeed != baseWalkSpeed)
            {
                walkSpeed = baseWalkSpeed;
            }
            if(stamina < maxStamina)
            {
                stamina += 0.01f;
            }
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
