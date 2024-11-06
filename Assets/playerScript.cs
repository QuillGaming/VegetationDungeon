using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class playerScript : MonoBehaviour
{
    Vector2 mv;
    public float playerSpeed;
    public float yRot;
    public TMPro.TMP_Text HUD;

    bool enemyIsHitable = false;
    float animationDelay = 0f;
    int enemyHealth = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mv.y > 0)
        {
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isWalking", true);
        }
        else
        {
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isWalking", false);
        }

        if (Input.GetMouseButtonDown(0) && animationDelay <= 0.0f) // should be left click
        {
            Debug.Log("You punched");
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isPunching", true);
            animationDelay = 1f;
            if (enemyIsHitable)
            {
                enemyHealth -= 2;
                HUD.text = "enemy health: " + enemyHealth;
            }
        }
        animationDelay -= Time.deltaTime;
        if (animationDelay <= 0.0f)
        {
            Debug.Log("punch done");
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isPunching", false);
        }

        //enemy death
        if (enemyHealth <= 0)
        {
            GameObject.Find("Enemy_test").GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true; //this should stop the agent's movement upon death
        }
    }

    void OnMove(InputValue move)
    {
        mv = move.Get<Vector2>();
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, mv.x * yRot);
        if (mv.y > 0)
        {
            transform.position += transform.forward * mv.y * playerSpeed;
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "to_Hit_Box")
        {
            Debug.Log("enemy is in box");
            enemyIsHitable = true;
        }
    }

    /*void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.relativeVelocity.magnitude > 2)
        {
            Debug.Log("enemy is in box");
            enemyIsHitable = true;
        }
    }*/

    private void OnTriggerExit(Collider other2)
    {
        if (other2.gameObject.name == "to_Hit_Box")
        {
            Debug.Log("enemy is not in box");
            enemyIsHitable = false;
        }
    }
    /*void onTriggerStay (Collider hitArea)
    {
        if (hitArea.gameObject.name == "to_Hit_Box")
        {
            enemyIsHitable = true;
            Debug.Log("enemy is in box");
        }
    }*/
}
