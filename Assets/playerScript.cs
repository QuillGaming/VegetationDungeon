using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class playerScript : MonoBehaviour
{
    Vector2 mv;
    public float playerSpeed;
    public float yRot;
    public TMPro.TMP_Text HUD;

    public GameObject Items;
    public GameObject box;
    public GameObject pHealthBar;

    bool halberd = false;
    bool sword = false;
    bool attackMade = false;

    float animationDelay = 0f;
    float weaponDelay = 0.9f;

    float backSpeed = 2f;
    float walkSpeed = 3f;
    float runSpeed = 5.5f;

    int playerHealth = 25;
    int damageOuput = 2;

    //list of enemies
    Enemy e1;
    Enemy e2;
    Enemy e3;
    Enemy e4;
    Enemy e5;
    Enemy e6;
    Enemy e7;
    Enemy e8;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // the enemies are created with separate health variables, but when the damage is applied, it is only applied to enemy #1
        e1 = Enemy.CreateEnemy(1f, 10f, 0f, GameObject.Find("Enemy1"), GameObject.Find("Enemy1").transform.GetChild(2).GetChild(0).gameObject);
        e2 = Enemy.CreateEnemy(1f, 10f, 0f, GameObject.Find("Enemy2"), GameObject.Find("Enemy2").transform.GetChild(2).GetChild(0).gameObject);
        e3 = Enemy.CreateEnemy(1f, 10f, 0f, GameObject.Find("Enemy3"), GameObject.Find("Enemy3").transform.GetChild(2).GetChild(0).gameObject);
        e4 = Enemy.CreateEnemy(1f, 10f, 0f, GameObject.Find("Enemy4"), GameObject.Find("Enemy4").transform.GetChild(2).GetChild(0).gameObject);
        e5 = Enemy.CreateEnemy(1f, 10f, 0f, GameObject.Find("Enemy5"), GameObject.Find("Enemy5").transform.GetChild(2).GetChild(0).gameObject);
        e6 = Enemy.CreateEnemy(1f, 10f, 0f, GameObject.Find("Enemy6"), GameObject.Find("Enemy6").transform.GetChild(2).GetChild(0).gameObject);
        e7 = Enemy.CreateEnemy(1f, 10f, 0f, GameObject.Find("Enemy7"), GameObject.Find("Enemy7").transform.GetChild(2).GetChild(0).gameObject);
        e8 = Enemy.CreateEnemy(1f, 10f, 0f, GameObject.Find("Enemy8"), GameObject.Find("Enemy8").transform.GetChild(2).GetChild(0).gameObject);
        HUD.text = "";
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

        //weapon swapping
        if (sword && halberd && Input.GetKeyDown("f") && animationDelay <= 0)
        {
            if (GameObject.Find("Player").GetComponent<Animator>().GetBool("hasSword"))
            {
                Items.GetComponent<itemScript>().swordHeld.SetActive(false);
                Items.GetComponent<itemScript>().swordBack.SetActive(true);
                GameObject.Find("Player").GetComponent<Animator>().SetBool("hasSword", false);

                Items.GetComponent<itemScript>().halberdHeld.SetActive(true);
                Items.GetComponent<itemScript>().halberdBack.SetActive(false);
                GameObject.Find("Player").GetComponent<Animator>().SetBool("hasHalberd", true);
                weaponDelay = 1.8f;
            }
            else
            {
                Items.GetComponent<itemScript>().halberdHeld.SetActive(false);
                Items.GetComponent<itemScript>().halberdBack.SetActive(true);
                GameObject.Find("Player").GetComponent<Animator>().SetBool("hasHalberd", false);

                Items.GetComponent<itemScript>().swordHeld.SetActive(true);
                Items.GetComponent<itemScript>().swordBack.SetActive(false);
                GameObject.Find("Player").GetComponent<Animator>().SetBool("hasSword", true);
                weaponDelay = 1.3f;
            }
        }

        if (Input.GetMouseButtonDown(0) && animationDelay <= 0.0f)
        {
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isPunching", true);
            animationDelay = weaponDelay;
            attackMade = true;
        }
        if(animationDelay <= 0.0f)
        {
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isPunching", false);
            attackMade = false;
        }

        //player death
        if (playerHealth <= 0)
        {   //stops agent from following player after death
            HUD.text = "You have died.\nPress 'p' to restart.";
            e1.setHitPlayer(false);
            e2.setHitPlayer(false);
            e3.setHitPlayer(false);
            e4.setHitPlayer(false);
            e5.setHitPlayer(false);
            e6.setHitPlayer(false);
            e7.setHitPlayer(false);
            e8.setHitPlayer(false);
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isDead", true);
            pHealthBar.SetActive(false);
            if (Input.GetKeyDown("p"))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        //calls a method for each enemy
        EnemyActions(e1);
        EnemyActions(e2);
        EnemyActions(e3);
        EnemyActions(e4);
        EnemyActions(e5);
        EnemyActions(e6);
        EnemyActions(e7);
        EnemyActions(e8);

        animationDelay -= Time.deltaTime;
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
            GetComponent<Rigidbody>().linearVelocity = transform.forward * mv.y * playerSpeed;
        }

        if (Input.GetKeyDown("p"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "goal")
        {
            SceneManager.LoadScene(0);
        }
        if (other.gameObject.name == e1.getEnemy().name)
        {
            e1.setHitable(true);
        }
        if (other.gameObject.name == e2.getEnemy().name)
        {
            e2.setHitable(true);
        }
        if (other.gameObject.name == e3.getEnemy().name)
        {
            e3.setHitable(true);
        }
        if (other.gameObject.name == e4.getEnemy().name)
        {
            e4.setHitable(true);
        }
        if (other.gameObject.name == e5.getEnemy().name)
        {
            e5.setHitable(true);
        }
        if (other.gameObject.name == e6.getEnemy().name)
        {
            e6.setHitable(true);
        }
        if (other.gameObject.name == e7.getEnemy().name)
        {
            e7.setHitable(true);
        }
        if (other.gameObject.name == e8.getEnemy().name)
        {
            e8.setHitable(true);
        }

        if (other.gameObject == GameObject.Find("Enemy1").transform.GetChild(0).gameObject)
        {
             e1.setHitPlayer(true);
        }
        if (other.gameObject == GameObject.Find("Enemy2").transform.GetChild(0).gameObject)
        {
            e2.setHitPlayer(true);
        }
        if (other.gameObject == GameObject.Find("Enemy3").transform.GetChild(0).gameObject)
        {
            e3.setHitPlayer(true);
        }
        if (other.gameObject == GameObject.Find("Enemy4").transform.GetChild(0).gameObject)
        {
            e4.setHitPlayer(true);
        }
        if (other.gameObject == GameObject.Find("Enemy5").transform.GetChild(0).gameObject)
        {
            e5.setHitPlayer(true);
        }
        if (other.gameObject == GameObject.Find("Enemy6").transform.GetChild(0).gameObject)
        {
            e6.setHitPlayer(true);
        }
        if (other.gameObject == GameObject.Find("Enemy7").transform.GetChild(0).gameObject)
        {
            e7.setHitPlayer(true);
        }
        if (other.gameObject == GameObject.Find("Enemy8").transform.GetChild(0).gameObject)
        {
            e8.setHitPlayer(true);
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
        if (other1.gameObject.name == e1.getEnemy().name)
        {
            e1.setHitable(true);
        }
        if (other1.gameObject.name == e2.getEnemy().name)
        {
            e2.setHitable(true);
        }
        if (other1.gameObject.name == e3.getEnemy().name)
        {
            e3.setHitable(true);
        }
        if (other1.gameObject.name == e4.getEnemy().name)
        {
            e4.setHitable(true);
        }
        if (other1.gameObject.name == e5.getEnemy().name)
        {
            e5.setHitable(true);
        }
        if (other1.gameObject.name == e6.getEnemy().name)
        {
            e6.setHitable(true);
        }
        if (other1.gameObject.name == e7.getEnemy().name)
        {
            e7.setHitable(true);
        }
        if (other1.gameObject.name == e8.getEnemy().name)
        {
            e8.setHitable(true);
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
        if (other2.gameObject.name == e1.getEnemy().name)
        {
            e1.setHitable(false);
        }
        if (other2.gameObject.name == e2.getEnemy().name)
        {
            e2.setHitable(false);
        }
        if (other2.gameObject.name == e3.getEnemy().name)
        {
            e3.setHitable(false);
        }
        if (other2.gameObject.name == e4.getEnemy().name)
        {
            e4.setHitable(false);
        }
        if (other2.gameObject.name == e5.getEnemy().name)
        {
            e5.setHitable(false);
        }
        if (other2.gameObject.name == e6.getEnemy().name)
        {
            e6.setHitable(false);
        }
        if (other2.gameObject.name == e7.getEnemy().name)
        {
            e7.setHitable(false);
        }
        if (other2.gameObject.name == e8.getEnemy().name)
        {
            e8.setHitable(false);
        }


        if (other2.gameObject == GameObject.Find("Enemy1").transform.GetChild(0).gameObject)
        {
            e1.setHitPlayer(false);
        }
        if (other2.gameObject == GameObject.Find("Enemy2").transform.GetChild(0).gameObject)
        {
            e2.setHitPlayer(false);
        }
        if (other2.gameObject == GameObject.Find("Enemy3").transform.GetChild(0).gameObject)
        {
            e3.setHitPlayer(false);
        }
        if (other2.gameObject == GameObject.Find("Enemy4").transform.GetChild(0).gameObject)
        {
            e4.setHitPlayer(false);
        }
        if (other2.gameObject == GameObject.Find("Enemy5").transform.GetChild(0).gameObject)
        {
            e5.setHitPlayer(false);
        }
        if (other2.gameObject == GameObject.Find("Enemy6").transform.GetChild(0).gameObject)
        {
            e6.setHitPlayer(false);
        }
        if (other2.gameObject == GameObject.Find("Enemy7").transform.GetChild(0).gameObject)
        {
            e7.setHitPlayer(false);
        }
        if (other2.gameObject == GameObject.Find("Enemy8").transform.GetChild(0).gameObject)
        {
            e8.setHitPlayer(false);
        }
    }

    private void EnemyActions(Enemy e)
    {
        //enemy death
        if (e.getHitPoints() <= 0)
        {   //stops agent from following player after death
            e.getEnemy().GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
            e.getEnemy().transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("isDead", true);
            e.getHealthBar().SetActive(false);
            e.setHitable(false);
        }
        else if (e.getHitPlayer() || playerHealth <= 0 || !HasLineOfSight(e.getEnemy().transform, GameObject.Find("Player").transform))
        {
            e.getEnemy().GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
            e.getEnemy().transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            //see if the enemy rotation bug can be fixed here
        }
        else
        {
            e.getEnemy().GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
            e.getEnemy().transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("isWalking", true);
        }

        if (attackMade && e.getHitable())
        {
            e.setHitable(false);
            e.setHealthDelay(true);
        }
        
        if (animationDelay <= 0.0f && e.getHealthDelay())
        {
            e.setHitPoints(e.getHitPoints() - damageOuput);
            e.getHealthBar().GetComponent<enemyHealthBar>().UpdateHealthBar(e.getHitPoints(), 10f);
            e.setHealthDelay(false);
        }

        if (e.getAttackDelay() > 0.0f)
        {
            e.setAttackDelay(e.getAttackDelay() - Time.deltaTime);
        }

        if (e.getAttackDelay() <= 0.0f)
        {
            e.getEnemy().transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("isPunching", false);
            if (e.getHitPlayer() && e.getHitPoints() > 0 && playerHealth > 0)
            {
                playerHealth -= 1;
                pHealthBar.GetComponent<playerHealthBar>().UpdateHealthBar(playerHealth, 25f);
                e.setAttackDelay(1.6f); //was 2.5 which was too slow for tests, ideal is 1.8
                e.getEnemy().transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("isPunching", true);
            }
        }
    }

    //perform raycasting for line of sight capability
    private bool HasLineOfSight(Transform enemy, Transform player)
    {
        Vector3 direction = player.position - enemy.position;

        RaycastHit hit;

        if (Physics.Raycast(enemy.position, direction, out hit, Mathf.Infinity))
        {
            return hit.collider.gameObject == player.gameObject;
        }
        return false;
    }
}
