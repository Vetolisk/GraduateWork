using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class DetectedEnemyWolf : MonoBehaviour
{
    public GameObject playerRef;
    public Wolf wolf;
    public static bool flagAttack;
    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            flagAttack=true;
            playerRef = other.gameObject;
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Enemy")
    //    {
    //        flagAttack = false;
    //        playerRef = null;
    //    }
    //}
    private void Update()
    {
        if (flagAttack&& playerRef.GetComponent<EnemyAI>().currentHealth>=0)
        {
            wolf.master = playerRef;
            wolf.chaseStartDistance = 0;

        }
        else
        {
            wolf.master = GameObject.FindGameObjectWithTag("Player");
            wolf.chaseStartDistance = 7;
            flagAttack = false;
            playerRef = null;
        }
    }
}
