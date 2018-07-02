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
    public bool canInteract;
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
    public bool isInStory;
    public delegate void delegatVoids();
    public delegatVoids movementDelegate;

    public Transform healthBar;
    [SerializeField] bool test;
    [Header("Die")]
    [SerializeField] GameObject diePanel;
    bool paused;

    public virtual void Awake()
    {
        // StartCoroutine(Regeneration());
        currentHealth = maxHealth;
        healthBar.GetComponent<Image>().fillAmount = CalculateHealth();
        baseWalkSpeed = walkSpeed;
        rotateMultipierBackUpUpDown = rotateMultiplierUpDowm;
        rotateMultiplierBackUp = rotateMultiplier;
    }

    // Use this for initialization
	void Start ()
    {

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
            if (!isInStory)
            {
                if (stamina >= sprintCost)
                {
                    if (walkSpeed != sprintSpeed)
                    {
                        walkSpeed = sprintSpeed;
                    }
                    stamina -= sprintCost;
                }
            }
        }
        else
        {
            if (!isInStory)
            {
                if (walkSpeed != baseWalkSpeed)
                {
                    if (walkSpeed != baseWalkSpeed)
                    {
                        walkSpeed = baseWalkSpeed;
                    }
                    if (stamina < maxStamina)
                    {
                        stamina += 0.01f;
                    }
                    walkSpeed = baseWalkSpeed;
                }
                if (stamina < (maxStamina))
                {
                    stamina += 0.01f;
                    walkSpeed = baseWalkSpeed;
                }
                if (stamina < maxStamina)
                {
                    stamina += 0.01f;
                }
            }
        }

        if (currentHealth <= 0)
        {
            print("You are dead");
            paused = !paused;
            if (paused)
            {
                Time.timeScale = 0;
            }
            currentHealth = 0;
            diePanel.SetActive(true);
            Cursor.visible = true;
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
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
    /*
    public IEnumerator Regeneration()
    {
        if(currentHealth > maxHealth * 0.6)
        {
            currentHealth += 1;
        }
        UIManager.uiManager.RefreshHealth();
        yield return new WaitForSeconds(2);
        StartCoroutine(Regeneration());
    }
    */

    IEnumerator RefreshHP()
    {
        yield return new WaitForEndOfFrame();
        if (currentHealth < health * 0.6)
        {
            for (int i = 0; healthBar.GetComponent<Image>().fillAmount > ((float)(1 / maxHealth) * currentHealth); i++)
            {
                healthBar.GetComponent<Image>().fillAmount -= 0.003f;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (healthBar.GetComponent<Image>().fillAmount < ((float)(1 / maxHealth) * currentHealth))
            {
                healthBar.GetComponent<Image>().fillAmount += 0.003f;
                yield return new WaitForEndOfFrame();
            }
        }
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
    public void Freeze()
    {
        gameObject.GetComponent<Player>().isInStory = true;
        gameObject.GetComponent<Player>().walkSpeed = 0;
        gameObject.GetComponent<Player>().rotateMultiplier = 0;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camerah>().rotateMultiplier = 0;
    }
    
    public void UnFreeze()
    {
            if (StoryLine.storyLine.destroyCam)
            {
                gameObject.GetComponent<Player>().isInStory = false;
                gameObject.GetComponent<Player>().walkSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().baseWalkSpeed;
                gameObject.GetComponent<Player>().rotateMultiplier = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().rotateMultiplierBackUp;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camerah>().rotateMultiplier = 1;
            }
    }

    public void ResetLevel()
    {
        Time.timeScale = 1;
    }
}
