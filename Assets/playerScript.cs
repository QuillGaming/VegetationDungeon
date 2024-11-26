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
    public GameObject eHealthBar;
    public GameObject pHealthBar;

    bool enemyIsHitable = false;
    bool playerIsHitable = false;
    bool healthDisplayDelay = false;

    bool halberd = false;
    bool sword = false;

    float animationDelay = 0f;
    float weaponDelay = 0.9f;
    float enemyAttackCooldown = 1f; //was 2.5 before but was too slow for tests

    float backSpeed = 0.035f;
    float walkSpeed = 0.05f;
    float runSpeed = 0.085f;

    int enemyHealth = 10;
    int playerHealth = 20;
    int damageOuput = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (mv.y > 0)
        {
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isRunning", false);
            GameObject.Find("Player").GetComponent<Animator>().SetBool("backwards", false);
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isWalking", true);
            playerSpeed = walkSpeed;
            if (Input.GetKey("left shift"))
            {
                GameObject.Find("Player").GetComponent<Animator>().SetBool("isRunning", true);
                playerSpeed = runSpeed;
            }
        }
        else if (mv.y < 0)
        {
            GameObject.Find("Player").GetComponent<Animator>().SetBool("backwards", true);
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isWalking", true);
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isRunning", false);
            playerSpeed = backSpeed;
        }
        else
        {
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isWalking", false);
            GameObject.Find("Player").GetComponent<Animator>().SetBool("backwards", false);
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isRunning", false);
            playerSpeed = walkSpeed;
        }

        //enemy death
        if (enemyHealth <= 0)
        {   //stops agent from following player after death
            GameObject.Find("Enemy_test").GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
            eHealthBar.SetActive(false);
            enemyIsHitable = false;
        }
        else if (playerIsHitable)
        {
            GameObject.Find("Enemy_test").GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
            //see if the enemy rotation bug can be fixed here
        }
        else
        {
            GameObject.Find("Enemy_test").GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
        }

        if (Input.GetMouseButtonDown(0) && animationDelay <= 0.0f)
        {
            Debug.Log("You punched");
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isPunching", true);
            animationDelay = weaponDelay;
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
                eHealthBar.GetComponent<enemyHealthBar>().UpdateHealthBar(enemyHealth, 10f);
                healthDisplayDelay = false;
            }
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isPunching", false);

            //weapon swapping
            if (sword && halberd && Input.GetKeyDown("f"))
            {
                if (GameObject.Find("Player").GetComponent<Animator>().GetBool("hasSword"))
                {
                    Items.GetComponent<itemScript>().swordHeld.SetActive(false);
                    Items.GetComponent<itemScript>().swordBack.SetActive(true);
                    GameObject.Find("Player").GetComponent<Animator>().SetBool("hasSword", false);

                    Items.GetComponent<itemScript>().halberdHeld.SetActive(true);
                    Items.GetComponent<itemScript>().halberdBack.SetActive(false);
                    GameObject.Find("Player").GetComponent<Animator>().SetBool("hasHalberd", true);
                }
                else
                {
                    Items.GetComponent<itemScript>().halberdHeld.SetActive(false);
                    Items.GetComponent<itemScript>().halberdBack.SetActive(true);
                    GameObject.Find("Player").GetComponent<Animator>().SetBool("hasHalberd", false);

                    Items.GetComponent<itemScript>().swordHeld.SetActive(true);
                    Items.GetComponent<itemScript>().swordBack.SetActive(false);
                    GameObject.Find("Player").GetComponent<Animator>().SetBool("hasSword", true);
                }
            }
        }

        
        //player death
        if (playerHealth <= 0)
        {   //stops agent from following player after death
            HUD.text = "You have died.";
            playerIsHitable = false;
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isDead", true);
            pHealthBar.SetActive(false);
        }

        enemyAttackCooldown -= Time.deltaTime;
        if (playerIsHitable && enemyAttackCooldown <= 0.0f && enemyHealth > 0)
        {
            playerHealth -= 1;
            pHealthBar.GetComponent<playerHealthBar>().UpdateHealthBar(playerHealth, 20f);
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
            transform.position += transform.forward * mv.y * playerSpeed;
        }
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            enemyIsHitable = true;
        }

        if (other.gameObject.name == "damage_player")
        {
            playerIsHitable = true;
        }

        //pick up the sword
        if (other.gameObject.name == "pick_sword" && !sword)
        {
            HUD.text = "Press 'e' to pick up sword.";
            if (Input.GetKey("e"))
            {
                GameObject.Find("Player").GetComponent<Animator>().SetBool("hasSword", true);
                Items.GetComponent<itemScript>().swordFloor.SetActive(false);
                Items.GetComponent<itemScript>().swordHeld.SetActive(true);
                box.transform.localScale = new Vector3(1.6f, 1.5f, 1.4f);
                box.transform.localPosition += new Vector3(0f, 0f, 0.6f);
                weaponDelay = 1.3f;
                damageOuput = 3;
                sword = true;
                HUD.text = "";
                if (halberd)
                {
                    Items.GetComponent<itemScript>().halberdHeld.SetActive(false);
                    Items.GetComponent<itemScript>().halberdBack.SetActive(true);
                    GameObject.Find("Player").GetComponent<Animator>().SetBool("hasHalberd", false);
                }
            }
        }

        //pick up the halberd
        if (other.gameObject.name == "pick_halberd" && !halberd)
        {
            HUD.text = "Press 'e' to pick up halberd.";
            if (Input.GetKey("e"))
            {
                GameObject.Find("Player").GetComponent<Animator>().SetBool("hasHalberd", true);
                Items.GetComponent<itemScript>().halberdFloor.SetActive(false);
                Items.GetComponent<itemScript>().halberdHeld.SetActive(true);
                weaponDelay = 1.8f;
                damageOuput = 4;
                halberd = true;
                HUD.text = "";
                if (sword)
                {
                    Items.GetComponent<itemScript>().swordHeld.SetActive(false);
                    Items.GetComponent<itemScript>().swordBack.SetActive(true);
                    GameObject.Find("Player").GetComponent<Animator>().SetBool("hasSword", false);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other1)
    {
        if (other1.gameObject.tag == "enemy")
        {
            enemyIsHitable = true;
        }

        if (other1.gameObject.name == "pick_sword" && !sword)
        {
            HUD.text = "Press 'e' to pick up sword.";
            if (Input.GetKey("e"))
            {
                GameObject.Find("Player").GetComponent<Animator>().SetBool("hasSword", true);
                Items.GetComponent<itemScript>().swordFloor.SetActive(false);
                Items.GetComponent<itemScript>().swordHeld.SetActive(true);
                box.transform.localScale = new Vector3(1.6f, 1.5f, 1.4f);
                box.transform.localPosition += new Vector3(0f, 0f, 0.6f);
                weaponDelay = 1.3f;
                damageOuput = 3;
                sword = true;
                HUD.text = "";
                if(halberd)
                {
                    Items.GetComponent<itemScript>().halberdHeld.SetActive(false);
                    Items.GetComponent<itemScript>().halberdBack.SetActive(true);
                    GameObject.Find("Player").GetComponent<Animator>().SetBool("hasHalberd", false);
                }
            }
        }

        //pick up the halberd
        if (other1.gameObject.name == "pick_halberd" && !halberd)
        {
            HUD.text = "Press 'e' to pick up halberd.";
            if (Input.GetKey("e"))
            {
                GameObject.Find("Player").GetComponent<Animator>().SetBool("hasHalberd", true);
                Items.GetComponent<itemScript>().halberdFloor.SetActive(false);
                Items.GetComponent<itemScript>().halberdHeld.SetActive(true);
                halberd = true;
                weaponDelay = 1.8f;
                damageOuput = 4;
                HUD.text = "";
                if (sword)
                {
                    Items.GetComponent<itemScript>().swordHeld.SetActive(false);
                    Items.GetComponent<itemScript>().swordBack.SetActive(true);
                    GameObject.Find("Player").GetComponent<Animator>().SetBool("hasSword", false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other2)
    {

        if (other2.gameObject.name == "damage_player")
        {
            playerIsHitable = false;
        }
    }
}
