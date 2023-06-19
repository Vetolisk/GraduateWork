using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class BuyBuildManager : MonoBehaviour
{
    public TextMeshProUGUI error;
    public Loader load;
    public TextMeshProUGUI CountVilagers;
    public List<GameObject> SetBuild;
    public GameObject Vilager;
    public int i=0;
    public int BuyCountEmeraldUp=5;
    public static int CountVilager=0;

    private void Awake()
    {
        BuyCountEmeraldUp= PlayerPrefs.GetInt("BuyCountEmeraldUp");
    }
    private void Start()
    {
        error.text = null;
        Debug.Log(ManagerGame.countEmeraldMG);
        CountVilager= PlayerPrefs.GetInt("CountVilager");
        CountVilagers.text = CountVilager.ToString();
        i = PlayerPrefs.GetInt("CI");
    }

    public void UpgradeVilagers()
    {
        
        if (ManagerGame.countEmeraldMG >= BuyCountEmeraldUp) {
            
            ManagerGame.countEmeraldMG -= BuyCountEmeraldUp;
            PlayerPrefs.SetInt("emeraldSprite", ManagerGame.countEmeraldMG);
            BuyCountEmeraldUp += 3;
            PlayerPrefs.SetInt("BuyCountEmeraldUp", BuyCountEmeraldUp);
            if (i < SetBuild.Count)
            {
                SetBuild[i].SetActive(true);
                if (SetBuild[i].name== "HomeTwoObj")
                {
                    GameObject VilagerScene = Instantiate(Vilager, SetBuild[i].transform.position,Quaternion.identity);
                    CountVilager++;
                    VilagerScene.tag = "Vilager";
                }
                if (SetBuild[i].name == "BigHomeObj")
                {
                    GameObject VilagerScene = Instantiate(Vilager, SetBuild[i].transform.position, Quaternion.identity);
                    CountVilager+=2;
                    VilagerScene.tag = "Vilager";
                }
                if (SetBuild[i].name == "BigHomeTwoObj")
                {
                    GameObject VilagerScene = Instantiate(Vilager, SetBuild[i].transform.position, Quaternion.identity);
                    CountVilager += 3;
                    VilagerScene.tag = "Vilager";
                }
                PlayerPrefs.SetInt("CountVilager", CountVilager);
                CountVilagers.text=CountVilager.ToString();
            }
            i++;
            PlayerPrefs.SetInt("CI",i);
        }
        else
        {
            error.text = "No Emeralds";
            Invoke("DestroyError",0.7f);
            Debug.Log("No Emeralds");
        }
    }
    public void DestroyError()
    {
        error.text=null;
    }
}
