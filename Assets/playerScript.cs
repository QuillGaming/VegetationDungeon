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
    bool playerIsHitable = false;
    float animationDelay = 0f;
    float enemyAttackCooldown = 2.5f;
    int enemyHealth = 10;
    int playerHealth = 20;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HUD.text = "Enemy health: " + enemyHealth + "\nPlayer health: " + playerHealth;
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
            animationDelay = 0.9f;
            if (enemyIsHitable)
            {
                enemyHealth -= 2;
                HUD.text = "Enemy health: " + enemyHealth + "\nPlayer health: " + playerHealth;
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
        {   //stops agent from following player after death
            GameObject.Find("Enemy_test").GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
            enemyIsHitable = false;
        }

        if (playerHealth <= 0)
        {   //stops agent from following player after death
            HUD.text = "You have died. The death animation will come later.";
            playerIsHitable = false;
        }

        enemyAttackCooldown -= Time.deltaTime;
        if (playerIsHitable && enemyAttackCooldown <= 0.0f && enemyHealth > 0)
        {
            playerHealth -= 1;
            HUD.text = "Enemy health: " + enemyHealth + "\nPlayer health: " + playerHealth;
            enemyAttackCooldown = 2.5f;
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
        if (other.gameObject.tag == "enemy")
        {
            Debug.Log("enemy is in box");
            enemyIsHitable = true;
        }

        if (other.gameObject.name == "damage_player")
        {
            Debug.Log("enemy can hit player");
            playerIsHitable = true;
        }
    }

    private void OnTriggerExit(Collider other2)
    {
        if (other2.gameObject.tag == "enemy")
        {
            Debug.Log("enemy is not in box");
            enemyIsHitable = false;
        }

        if (other2.gameObject.name == "damage_player")
        {
            Debug.Log("enemy cannot hit player");
            playerIsHitable = false;
        }
    }
}
