using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidArea : MonoBehaviour
{
    public void Mid(Transform midPoint)
    {
        transform.LookAt(midPoint);
    }
}
