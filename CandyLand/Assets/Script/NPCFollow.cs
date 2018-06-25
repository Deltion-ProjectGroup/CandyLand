using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCFollow : MonoBehaviour
{
    public Vector3 targetLocation;
    public bool needsPlayer = true;
    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        gameObject.GetComponent<SphereCollider>().radius = 6.5f;
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && needsPlayer)
        {
            StartCoroutine(StartMove());
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && needsPlayer)
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        }
    }
    public IEnumerator StartMove()
    {
        print("Enum");
        yield return new WaitForSeconds(1);
        if (gameObject.GetComponent<NavMeshAgent>().isStopped)
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().SetDestination(targetLocation);
            print("Desu");
        }
    }
}
