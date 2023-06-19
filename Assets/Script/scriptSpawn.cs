using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class scriptSpawn : MonoBehaviour
{
    public GameObject[] entiy;

    private void Start()
    {
        StartCoroutine("WaitAndPrint");
    }
    IEnumerator WaitAndPrint()
    {
        while (true)
        {
               /*
                float posY;
                float posX = Random.Range(-132, -62);
                if (posX >=- 104||posX<=-80)
                {
                    posY = 10.53f;
                }
                else
                {
                    posY = 9f;
                }
                
               // GameObject entiys = Instantiate(entiy[0], new Vector3(Random.Range(-132,-62), posY, Random.Range(19, 44)), Quaternion.identity);
              
                if (entiys.GetComponent<EnemyAI>() != null)
                {
                    //entiys.GetComponent<EnemyAI>().enabled = true;
                }*/
                  GameObject entiys = Instantiate(entiy[0], transform.position, Quaternion.identity);



                yield return new WaitForSeconds(60f);
            

            //yield return null;
        }
    }
}
