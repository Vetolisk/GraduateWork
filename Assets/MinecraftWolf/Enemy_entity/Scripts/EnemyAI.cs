using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    public GameObject DropExplosion;
    public GameObject Drop;
    public GameObject target;
    public int countDrop;
    //public HealthBar healthBar;
    public int health;
    //public int startHealth;
    public int currentHealth;
    public float runSpeed = 5;
    public float chaseStartDistance = 1;
    public static bool FlagEnemyDead;
    private float stopTime;
    public float startStopTime;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        currentHealth = health;
    }
    public void TakeDamage(int damage)
    {
        stopTime = startStopTime;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            DetectedEnemyWolf.flagAttack = false;
            
            Instantiate(DropExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            



        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag=="Wolf")
        {
            target = collision.gameObject;
        }
    }
    private void FixedUpdate()
    {
        if (Vector3.Distance(target.transform.position, transform.position) > chaseStartDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, runSpeed * Time.deltaTime);
        }
       
    }
}
