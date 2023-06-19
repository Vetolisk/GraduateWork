using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using static UnityEngine.GraphicsBuffer;


public class AIVilager : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 pos;
    private void Awake()
    {
        SaveSystem.Vilagerobj.Add(gameObject);
    }
    private void Start()
    {
        pos = new Vector3(Random.Range(-50, 60), 2, Random.Range(-43, 38));
    }
    private void OnDestroy()
    {
        SaveSystem.Vilagerobj.Remove(gameObject);
    }
    private void Update()
    {
        if (Vector3.Distance(pos, agent.transform.position) > 1.0f)
        {
            

            agent.SetDestination(pos);

        }
        else
        {
            pos = new Vector3(Random.Range(-50, 60), 2, Random.Range(-43, 38));
        }     
    }

}
