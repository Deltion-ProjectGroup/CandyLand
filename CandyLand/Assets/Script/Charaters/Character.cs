using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public int health = 100;
    public int maxHealth = 100;
	// Use this for initialization


    public virtual void Movement(Vector3 movePos, float moveSpeed)
    {

    }
    public virtual void Death()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
