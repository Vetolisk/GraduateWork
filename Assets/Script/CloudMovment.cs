using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovment : MonoBehaviour
{
    [SerializeField]
    float _velocity = 10;
    [SerializeField]
    float startPos;
    [SerializeField]
    float EndPos;
    [SerializeField]
    float StartYPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
           

        if (gameObject.transform.position.x <= EndPos)
        {
            transform.position = new Vector3(startPos, StartYPos, 0f);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(EndPos, StartYPos), Time.deltaTime * _velocity);
        }
        
           
           
        
    }
}
