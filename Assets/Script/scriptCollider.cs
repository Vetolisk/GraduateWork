using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scriptCollider : MonoBehaviour
{
    public static bool flagTrigger;
    public TextMeshPro textMeshPro;
    public  GameObject Entity;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wolf")
        {
            Wolf.scriptColliders = gameObject.GetComponent<scriptCollider>();
            Debug.Log(Wolf.scriptColliders.name);
            textMeshPro.text = "Ready";
            textMeshPro.color = Color.green;
            flagTrigger = true;

            Debug.Log(flagTrigger);
            if (Entity != null) { 
            Debug.Log(Entity.name);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wolf")
        {
            
            textMeshPro.text = "None";
            textMeshPro.color = Color.white;
            flagTrigger = false;
            Debug.Log(flagTrigger);
        }
    }
    
}
