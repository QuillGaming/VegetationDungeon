using UnityEngine;

public class Enemy: ScriptableObject
{
    public float damageAmount;
    public float hitPoints;
    public float attackDelay;
    public GameObject enemy;
    public bool hitable;
    public bool canHitPlayer;
    public GameObject healthBar;
    //public Animator anim;
    //public PlayerHealth playerHealth;

    public Enemy(float damage, float health, float attackTime, GameObject e, GameObject hb)//, Animator a
    {
        damageAmount = damage;
        hitPoints = health;
        attackDelay = attackTime;
        enemy = e;
        hitable = false;
        healthBar = hb;
        //anim = a;

        //playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }
}
