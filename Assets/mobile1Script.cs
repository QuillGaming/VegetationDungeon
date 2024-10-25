using UnityEngine;
using UnityEngine.AI;

public class mobile1Script : MonoBehaviour
{
    public GameObject target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<NavMeshAgent>().speed *= 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
    }
}
