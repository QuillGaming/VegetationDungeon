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
    public GameObject pHealthBar;

    bool healthDisplayDelay = false;
    bool halberd = false;
    bool sword = false;
    bool attackMade = false;

    float animationDelay = 0f;
    float weaponDelay = 0.9f;

    float backSpeed = 0.035f;
    float walkSpeed = 0.05f;
    float runSpeed = 0.085f;

    int playerHealth = 25;
    int damageOuput = 2;

    //list of enemies
    Enemy e1;
    Enemy e2;
    Enemy e3;
    Enemy e4;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        e1 = CreateEnemy(1f, 10f, 1f, GameObject.Find("Enemy1"), GameObject.Find("Enemy1").transform.GetChild(2).GetChild(0).gameObject);
        e2 = CreateEnemy(1f, 10f, 1f, GameObject.Find("Enemy2"), GameObject.Find("Enemy2").transform.GetChild(2).GetChild(0).gameObject);
        e3 = CreateEnemy(1f, 10f, 1f, GameObject.Find("Enemy3"), GameObject.Find("Enemy3").transform.GetChild(2).GetChild(0).gameObject);
        e4 = CreateEnemy(1f, 10f, 1f, GameObject.Find("Enemy4"), GameObject.Find("Enemy4").transform.GetChild(2).GetChild(0).gameObject);
    }

    Enemy CreateEnemy(float da, float hp, float ad, GameObject en, GameObject hb)
    {
        Enemy newEnemy = ScriptableObject.CreateInstance<Enemy>();
        newEnemy.damageAmount = da;
        newEnemy.hitPoints = hp;
        newEnemy.attackDelay = ad;
        newEnemy.enemy = en;
        newEnemy.hitable = false;
        newEnemy.canHitPlayer = false;
        newEnemy.healthBar = hb;
        Debug.Log(hb.name);
        return newEnemy;
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
            HUD.text = "You have died.";
            e1.canHitPlayer = false;
            e2.canHitPlayer = false;
            e3.canHitPlayer = false;
            e4.canHitPlayer = false;
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isDead", true);
            pHealthBar.SetActive(false);
        }

        //calls a method for each enemy
        EnemyActions(e1);
        EnemyActions(e2);
        EnemyActions(e3);
        EnemyActions(e4);
        
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
            transform.position += transform.forward * mv.y * playerSpeed;
        }
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == e1.enemy.name)
        {
            e1.hitable = true;
        }
        if (other.gameObject.name == e2.enemy.name)
        {
            e2.hitable = true;
        }
        if (other.gameObject.name == e3.enemy.name)
        {
            e3.hitable = true;
        }
        if (other.gameObject.name == e4.enemy.name)
        {
            e4.hitable = true;
        }

        if (other.gameObject == GameObject.Find("Enemy1").transform.GetChild(0).gameObject)
        {
             e1.canHitPlayer = true;
        }
        if (other.gameObject == GameObject.Find("Enemy2").transform.GetChild(0).gameObject)
        {
            e2.canHitPlayer = true;
        }
        if (other.gameObject == GameObject.Find("Enemy3").transform.GetChild(0).gameObject)
        {
            e3.canHitPlayer = true;
        }
        if (other.gameObject == GameObject.Find("Enemy4").transform.GetChild(0).gameObject)
        {
            e4.canHitPlayer = true;
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
        if (other1.gameObject.name == e1.enemy.name)
        {
            e1.hitable = true;
        }
        if (other1.gameObject.name == e2.enemy.name)
        {
            e2.hitable = true;
        }
        if (other1.gameObject.name == e3.enemy.name)
        {
            e3.hitable = true;
        }
        if (other1.gameObject.name == e4.enemy.name)
        {
            e4.hitable = true;
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

        if (other2.gameObject == GameObject.Find("Enemy1").transform.GetChild(0).gameObject)
        {
            e1.canHitPlayer = false;
        }
        if (other2.gameObject == GameObject.Find("Enemy2").transform.GetChild(0).gameObject)
        {
            e2.canHitPlayer = false;
        }
        if (other2.gameObject == GameObject.Find("Enemy3").transform.GetChild(0).gameObject)
        {
            e3.canHitPlayer = false;
        }
        if (other2.gameObject == GameObject.Find("Enemy4").transform.GetChild(0).gameObject)
        {
            e4.canHitPlayer = false;
        }
    }

    private void EnemyActions(Enemy e)
    {
        //enemy death
        if (e.hitPoints <= 0)
        {   //stops agent from following player after death
            e.enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
            e.healthBar.SetActive(false);
            e.hitable = false;
        }
        else if (e.canHitPlayer)
        {
            e.enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
            //see if the enemy rotation bug can be fixed here
        }
        else
        {
            e.enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
        }

        if (attackMade && e.hitable)
        {
            healthDisplayDelay = true;
            e.hitable = false;
        }

        //Debug.Log(animationDelay);
        Debug.Log(healthDisplayDelay);
        if (animationDelay <= 0.0f && healthDisplayDelay)
        {
            e.hitPoints -= damageOuput;
            Debug.Log(e1.hitPoints + "   " + e2.hitPoints + "   " + e3.hitPoints + "   " + e4.hitPoints);
            e.healthBar.GetComponent<enemyHealthBar>().UpdateHealthBar(e.hitPoints, 10f);
            healthDisplayDelay = false;
        }

        e.attackDelay -= Time.deltaTime;
        if (e.canHitPlayer && e.attackDelay <= 0.0f && e.hitPoints > 0)
        {
            playerHealth -= 1;
            pHealthBar.GetComponent<playerHealthBar>().UpdateHealthBar(playerHealth, 20f);
            e.attackDelay = 1.2f; //was 2.5 which was too slow for tests
        }
    }
}
