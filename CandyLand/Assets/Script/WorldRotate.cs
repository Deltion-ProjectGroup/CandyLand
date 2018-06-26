using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRotate : MonoBehaviour
{
    public List<Transform> world = new List<Transform>();
    private void Awake()
    {
        for (int i = 0; i < world.Count; i++)
        {
            world[i].transform.localEulerAngles = new Vector3(Mathf.RoundToInt(-90), transform.rotation.y, 0);
        }
    }
}
