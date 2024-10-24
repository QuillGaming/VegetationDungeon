using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class mobile1Script : MonoBehaviour
{
    public float enemySpeed;
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
