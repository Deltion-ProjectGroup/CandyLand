using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIsAttack : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player2")
        {
            transform.GetComponentInParent<BossEnemy>().isChasing = true;
        }
    }

    public void Look(Vector3 target)
    {
        transform.LookAt(target);
    }

}
