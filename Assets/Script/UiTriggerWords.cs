using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTriggerWords : MonoBehaviour
{
    [SerializeField]
    public GameObject panel;
    [SerializeField]
    public WordsScript ws;
   void OnTriggerEnter(Collider other)
   {
    if(other.tag=="Player"){
        ws.SetWords();
        panel.SetActive(true);
    }
   }
   void OnTriggerExit(Collider other)
   {
    if(other.tag=="Player"){
        ws.ClearList();
        panel.SetActive(false);
    }
   }
}
