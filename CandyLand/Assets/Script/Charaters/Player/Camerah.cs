using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerah : MonoBehaviour
{
    bool shaking;
    public static Camerah camerah;
    public int maxCamShakes = 10;
    Vector3 camBackupPos;
    private Vector3 rotatePos;
    [SerializeField] float clamp;
    [HideInInspector] public float rotateMultiplier;
    public float interactionRange;
    public RaycastHit hit;
    // Use this for initialization
    void Awake ()
    {
        camerah = this;
        //Cursor.lockState = CursorLockMode.Locked;
        rotateMultiplier = GetComponentInParent<Player>().rotateMultiplierUpDowm;
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*if (Input.GetButtonDown("Fire1"))
        {
            if (shaking)
            {
                StopAllCoroutines();
                gameObject.transform.localPosition = camBackupPos;
                StartCoroutine(ScreenShake());
            }
            else
            {
                StartCoroutine(ScreenShake());
            }
        }*/
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
    public IEnumerator ScreenShake(float intensity = 0.2f)
    {
        shaking = true;
        Debug.Log(transform.position);
        camBackupPos = new Vector3(0, transform.localPosition.y, 0);
        Debug.Log(camBackupPos);
        for(int i = 0; i < Random.Range(20, maxCamShakes + 1); i++)
        {
            Vector3 camShakePos = new Vector3(camBackupPos.x + Random.Range(-intensity, intensity), camBackupPos.y + Random.Range(-intensity, intensity), camBackupPos.z);
            transform.localPosition = camShakePos;
            yield return new WaitForEndOfFrame();
            transform.localPosition = camBackupPos;
        }
        shaking = false;
    }
}
