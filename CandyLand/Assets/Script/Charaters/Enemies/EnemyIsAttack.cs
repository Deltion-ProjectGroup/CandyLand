using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIsAttack : MonoBehaviour
{
    public void  LookAt(Transform target)
    {
        transform.LookAt(target);
    }
}
