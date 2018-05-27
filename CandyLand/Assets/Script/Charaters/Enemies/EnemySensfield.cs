using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensfield : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            transform.GetComponentInParent<Enemy>().sensfield = true;
            transform.GetComponentInParent<Enemy>().SensField();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            transform.GetComponentInParent<Enemy>().sensfield = false;
        }
    }
}
