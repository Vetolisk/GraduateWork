using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraininScript : MonoBehaviour
{
    public GameObject TraningPanel;


 
 void Awake()
 {
    if(!PlayerPrefs.HasKey("flag")||PlayerPrefs.GetInt("flag")!=0){
             PlayerPrefs.SetInt("flag", 0);
             PlayerPrefs.Save();
             Invoke("OfAnimationPanel",7);
        }else if(PlayerPrefs.GetInt("flag")==0){
               TraningPanel.SetActive(false);
        }
 }
    
    public void OfAnimationPanel(){
        TraningPanel.GetComponent<Animator>().SetBool("FlagOf",true);
    }
}
