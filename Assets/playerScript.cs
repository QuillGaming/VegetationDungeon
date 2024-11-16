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
    public GameObject Items;
    public GameObject box;

    bool enemyIsHitable = false;
    bool playerIsHitable = false;
    bool healthDisplayDelay = false;
    float animationDelay = 0f;
    float enemyAttackCooldown = 1f; //was 2.5 before but was too slow for tests
    int enemyHealth = 10;
    int playerHealth = 20;
    int damageOuput = 2;
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
                healthDisplayDelay = true;
                enemyIsHitable = false;
            }
        }
        animationDelay -= Time.deltaTime;
        if (animationDelay <= 0.0f)
        {
            if (healthDisplayDelay)
            {
                enemyHealth -= damageOuput;
                HUD.text = "Enemy health: " + enemyHealth + "\nPlayer health: " + playerHealth;
                healthDisplayDelay = false;
            }
            Debug.Log("punch done");
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isPunching", false);
        }

        //enemy death
        if (enemyHealth <= 0)
        {   //stops agent from following player after death
            GameObject.Find("Enemy_test").GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
            enemyIsHitable = false;
        }
        else if (playerIsHitable)
        {
            GameObject.Find("Enemy_test").GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
        }
        else
        {
            GameObject.Find("Enemy_test").GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
        }

        //player death
        if (playerHealth <= 0)
        {   //stops agent from following player after death
            HUD.text = "You have died.";
            playerIsHitable = false;
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isDead", true);
        }

        enemyAttackCooldown -= Time.deltaTime;
        if (playerIsHitable && enemyAttackCooldown <= 0.0f && enemyHealth > 0)
        {
            playerHealth -= 1;
            HUD.text = "Enemy health: " + enemyHealth + "\nPlayer health: " + playerHealth;
            enemyAttackCooldown = 1f; //was 2.5 which was too slow for tests
        }

        

    }

    void OnMove(InputValue move)
    {
        mv = move.Get<Vector2>();
    }

    void FixedUpdate()
    {
        if (playerHealth > 0)
        {
            transform.Rotate(Vector3.up, mv.x * yRot);
            //if (mv.y > 0)
            //{
            transform.position += transform.forward * mv.y * playerSpeed;
            //}
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

        //make the trigger to pick up the sword
        if (other.gameObject.name == "pick_sword" && !GameObject.Find("Player").GetComponent<Animator>().GetBool("hasSword"))
        {
            HUD.text = "Enemy health: " + enemyHealth + "\nPlayer health: " + playerHealth + "\nPress 'e' to pick up sword.";
            if (Input.GetKey("e"))
            {
                GameObject.Find("Player").GetComponent<Animator>().SetBool("hasSword", true);
                Items.GetComponent<itemScript>().swordFloor.SetActive(false);
                Items.GetComponent<itemScript>().swordHeld.SetActive(true);
                //paddle.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                box.transform.localScale = new Vector3(1.6f, 1.5f, 1.4f);
                box.transform.localPosition += new Vector3(0f, 0f, 0.6f);
                damageOuput = 3;
            }
        }
    }

    private void OnTriggerStay(Collider other1)
    {
        if (other1.gameObject.tag == "enemy")
        {
            Debug.Log("enemy is in box");
            enemyIsHitable = true;
        }

        if (other1.gameObject.name == "pick_sword" && !GameObject.Find("Player").GetComponent<Animator>().GetBool("hasSword"))
        {
            HUD.text = "Enemy health: " + enemyHealth + "\nPlayer health: " + playerHealth + "\nPress 'e' to pick up sword.";
            if (Input.GetKey("e"))
            {
                GameObject.Find("Player").GetComponent<Animator>().SetBool("hasSword", true);
                Items.GetComponent<itemScript>().swordFloor.SetActive(false);
                Items.GetComponent<itemScript>().swordHeld.SetActive(true);
                box.transform.localScale = new Vector3(1.6f, 1.5f, 1.4f);
                box.transform.localPosition += new Vector3(0f, 0f, 0.6f);
                damageOuput = 3;
            }
        }
    }

    private void OnTriggerExit(Collider other2)
    {
        /*if (other2.gameObject.tag == "enemy")
        {
            Debug.Log("enemy is not in box");
            enemyIsHitable = false;
        }*/

        if (other2.gameObject.name == "damage_player")
        {
            Debug.Log("enemy cannot hit player");
            playerIsHitable = false;
        }
    }
}
