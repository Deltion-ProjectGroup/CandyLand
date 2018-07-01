using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("Health")]
    public int health = 100;
    public int maxHealth = 100;
    [HideInInspector] public float currentHealth;

    public virtual void Movement(Vector3 movePos, float moveSpeed)
    {


    }

    public virtual void Health(float damage)
    {

    }

    public float CalculateHealth()
    {
        return currentHealth / maxHealth;
    }


    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
