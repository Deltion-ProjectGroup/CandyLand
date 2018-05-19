using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour {
    public Animation anim;
    public bool canUse = true;

    public virtual void Use()
    {
        canUse = false;
        StartCoroutine(Cooldown());
    }
    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(anim.clip.length);
        canUse = true;
    }
}
