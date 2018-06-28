using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
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
    public float rotateMultiplier;
    public float rotateMultiplierUpDowm;
    public float rotateMultipierBackUpUpDown;
    public float rotateMultiplierBackUp;
    [Header("Jumping")]
    public Vector3 jumpAmt;
    bool canJump = true;
    [Header("Quest")]
    public bool hasQuest;
    public delegate void delegatVoids();
    public delegatVoids movementDelegate;

    public Transform healthBar;
	// Use this for initialization
	void Start ()
    {
        currentHealth = maxHealth;
        healthBar.GetComponent<Image>().fillAmount = CalculateHealth();
        baseWalkSpeed = walkSpeed;
        rotateMultipierBackUpUpDown = rotateMultiplierUpDowm;
        rotateMultiplierBackUp = rotateMultiplier;
	}
	// Update is called once per frame
	void Update ()
    {
        RotateCam(camRotate, rotateMultiplier);
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
    private void FixedUpdate()
    {
        Movement(movePos, walkSpeed);
    }

        public override void Health(float damage)
    {
        currentHealth -= damage;
        healthBar.GetComponent<Image>().fillAmount = CalculateHealth();
    }

    public override void Movement(Vector3 mover, float speed)
    {
        mover.x = Input.GetAxis("Horizontal");
        mover.z = Input.GetAxis("Vertical");
        transform.Translate(mover * speed * Time.deltaTime);
        if(movementDelegate != null)
        {
            movementDelegate();
        }
    }
    public override IEnumerator Regeneration()
    {
        if(health < maxHealth)
        {
            health += 1;
        }
        UIManager.uiManager.RefreshHealth();
        yield return new WaitForSeconds(2);
        StartCoroutine(Regeneration());
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
