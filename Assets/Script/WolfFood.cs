using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WolfFood : MonoBehaviour, IInteractable
{
    public ParticleSystem FoodYamy;
    public ManagerGame MG;
    public int beef;
    public TextMeshProUGUI TextMeshProFood;
    public TextMeshProUGUI TextMeshProFoodCount;
    public FirstPersonController fpc;

    
    public void Interact()
    {
        if ( ManagerGame.countBeefMG>0)
        {
            MG.Food += 20;
            PlayerPrefs.SetInt("Food", MG.Food);
            TextMeshProFoodCount.text = "Food: " + MG.Food.ToString();
            Instantiate(FoodYamy,new Vector3( transform.position.x, transform.position.y+0.5f, transform.position.z- 1.075f), Quaternion.identity);
            ManagerGame.countBeefMG--;
            TextMeshProFood.text = ManagerGame.countBeefMG.ToString();
            PlayerPrefs.SetInt("beefSprite", ManagerGame.countBeefMG);
        } 
        
       

    }
}
