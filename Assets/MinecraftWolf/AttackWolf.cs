using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackWolf : MonoBehaviour
{
    public GameObject target;
    public int DamageAttack;
    public float startStopTime;
    private bool attack;
    private void Update()
    {
        if (DetectedEnemyWolf.flagAttack) {
            if (startStopTime < 0)
            {
                Debug.Log("1");
                Attack();
                startStopTime = 1;
            }
            else
            {
                startStopTime -= Time.deltaTime;

            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            target = other.gameObject;

        }
    }
    public void Attack()
    {
        Debug.Log("Attack!");
        if (target!=null) {
            target.GetComponent<EnemyAI>().TakeDamage(DamageAttack);
        }
    }
    

}
