using System.Collections;
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
    [SerializeField] float clamp;
    [HideInInspector] public float rotateMultiplier;
    public float interactionRange;
    public RaycastHit hit;
    // Use this for initialization
    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rotateMultiplier = GetComponentInParent<Player>().rotateMultiplierUpDowm;
	}
	
	// Update is called once per frame
	void Update ()
    {
        RotateCam(rotatePos, rotateMultiplier);
        //Debug.DrawRay(transform.position, transform.forward, Color.cyan, 10);
	}

    public void RotateCam(Vector3 rotator, float speed)
    {
        rotator.x -= Input.GetAxis("Mouse Y") * speed;
        rotator.x = Mathf.Clamp(rotator.x, -clamp, clamp);
        transform.eulerAngles = (new Vector3(rotator.x, transform.eulerAngles.y, 0.0f));
        rotatePos.x = rotator.x;
    }
}
