using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void ThinkTimer()
    {
        base.ThinkTimer();
    }

    public override void RandomPos()
    {
        base.RandomPos();
    }

    public override void isAttacking()
    {
        base.isAttacking();
    }

    public override void SensField()
    {
        base.SensField();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            print("enterTrigger");
            sensfield = true;
            SensField();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            print(target);
            transform.GetComponentInChildren<EnemyIsAttack>().LookAt(target);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            transform.GetComponentInChildren<EnemyIsAttack>().LookAt(null);
            sensfield = false;
            isChasing = false;
            print(isChasing);
        }
    }
}
