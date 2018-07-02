using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIsAttack : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            print("i hit the player");
            gameObject.transform.GetComponentInParent<BossEnemy>().isChasing = true;
        }
    }

    public void Look(Vector3 target)
    {
        transform.LookAt(target);
    }

}
