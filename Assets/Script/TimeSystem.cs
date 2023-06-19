using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TimeSystem : MonoBehaviour
{

    public TextMeshProUGUI TextAtetion;
    public TextMeshProUGUI TextNewEmeralds;
    #region Singleton
    public static TimeSystem Instance { get; private set; }
    private void InitSingleton()
    {
        Instance = this;
    }
    #endregion
    private void Awake()
    {
        InitSingleton();
        CheckOffline();
        

    }
    private void CheckOffline()
    {
        TimeSpan ts;

        if (PlayerPrefs.HasKey("LastSession"))
        {
            ts = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("LastSession"));
            if (TextAtetion!=null) {
                TextAtetion.text = "Отсуствовал времени:\n" + ts.Days + " дней, " + ts.Hours + " часов, " + ts.Minutes + " минут, " + ts.Seconds + " секунд";
            }
            print(string.Format("", ts.Days, ts.Hours, ts.Minutes, ts.Seconds));
            Debug.Log(ts.TotalSeconds.ToString());

            float value = 0;
            if (float.TryParse(ts.TotalSeconds.ToString(), out value))
            {
               
                int intTime = (int)value;
                int mult = PlayerPrefs.GetInt("CountVilager");
                Debug.Log("mult" + mult);
                int oldemeralds = PlayerPrefs.GetInt("emeraldSprite");
                
               
                ManagerGame.countEmeraldMG = (intTime / 60) * PlayerPrefs.GetInt("CountVilager") + oldemeralds;
                if (TextNewEmeralds!=null) {
                    TextNewEmeralds.text =   (PlayerPrefs.GetInt("CountVilager")* (intTime / 60)).ToString();
                }
                PlayerPrefs.SetInt("emeraldSprite", ManagerGame.countEmeraldMG);
            }
        }
        else
        {
            PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());
        }
        
           
        

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());
    }


}
