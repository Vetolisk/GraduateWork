using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void HomeButton()
    {
        SceneManager.LoadScene("HomePlayer");
    }
    public void Clear()
    {
        PlayerPrefs.SetInt("Food", 100);
        PlayerPrefs.SetInt("beefSprite", 0);
        PlayerPrefs.SetInt("emeraldSprite", 0);
        PlayerPrefs.SetInt("gunpowderSprite", 0);
        PlayerPrefs.SetInt("rotten_fleshSprite", 0);
        PlayerPrefs.SetInt("spider_eyeSprite", 0);
        PlayerPrefs.SetInt("stringSprite", 0);
        PlayerPrefs.SetInt("CI", 0);
        PlayerPrefs.SetInt("CountVilager", 0);
        PlayerPrefs.SetInt("BuyCountEmeraldUp", 5);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
