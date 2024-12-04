using UnityEngine;

public class Enemy: ScriptableObject
{
    private float damageAmount;
    private float hitPoints;
    private float attackDelay;
    private GameObject enemy;
    private bool hitable;
    private bool canHitPlayer;
    private GameObject healthBar;
    private bool healthDisplayDelay;
    //public Animator anim;
    //public PlayerHealth playerHealth;

    private void Init(float damage, float health, float attackTime, GameObject e, GameObject hb)//, Animator a
    {
        damageAmount = damage;
        hitPoints = health;
        attackDelay = attackTime;
        enemy = e;
        hitable = false;
        canHitPlayer = false;
        healthDisplayDelay = false;
        healthBar = hb;
        //anim = a;

        //playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    public static Enemy CreateEnemy(float da, float hp, float ad, GameObject en, GameObject hb)
    {
        var newEnemy = ScriptableObject.CreateInstance<Enemy>();
        newEnemy.Init(da, hp, ad, en, hb);
        return newEnemy;
    }

    public float getHitPoints()
    {
        return hitPoints;
    }

    public void setHitPoints(float hp)
    {
        hitPoints = hp;
    }

    public GameObject getHealthBar()
    {
        return healthBar;
    }

    public GameObject getEnemy()
    {
        return enemy;
    }

    public bool getHitPlayer()
    {
        return canHitPlayer;
    }

    public void setHitPlayer(bool canHit)
    {
        canHitPlayer = canHit;
    }

    public bool getHitable()
    {
        return hitable;
    }

    public void setHitable(bool inDanger)
    {
        hitable = inDanger;
    }

    public float getAttackDelay()
    {
        return attackDelay;
    }

    public void setAttackDelay(float delay)
    {
        attackDelay = delay;
    }

    public bool getHealthDelay()
    {
        return healthDisplayDelay;
    }

    public void setHealthDelay(bool delay)
    {
        healthDisplayDelay = delay;
    }

}
