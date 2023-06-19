using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    public ManagerGame game;
    public static bool flagDrop = false;
    public Sprite SpriteModel;
    // Start is called before the first frame update
    private void Awake()
    {
        game = GameObject.FindGameObjectWithTag("Manager").GetComponent<ManagerGame>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Drop")
        {
            flagDrop = true;
            if (other.gameObject.GetComponentInChildren<DropScriptOther>().SpriteModel.name == "beefSprite")
            {
                ManagerGame.countBeefMG++;
                game.SpriteModelMG = "beef";
                PlayerPrefs.SetInt("beefSprite", ManagerGame.countBeefMG);
                game.CountDrop = ManagerGame.countBeefMG;
            }
            if (other.gameObject.GetComponentInChildren<DropScriptOther>().SpriteModel.name == "emeraldSprite")
            {
                ManagerGame.countEmeraldMG++;
                PlayerPrefs.SetInt("emeraldSprite", ManagerGame.countEmeraldMG);
                game.SpriteModelMG = "Emerald";
                game.CountDrop = ManagerGame.countEmeraldMG;
            }
            if (other.gameObject.GetComponentInChildren<DropScriptOther>().SpriteModel.name == "gunpowderSprite")
            {
                ManagerGame.countgunpowderMG++;
                game.SpriteModelMG = "gunpowder";
                game.CountDrop = ManagerGame.countgunpowderMG;
            }
            if (other.gameObject.GetComponentInChildren<DropScriptOther>().SpriteModel.name == "rotten_fleshSprite")
            {
                ManagerGame.countRottenFleshMG++;
                game.SpriteModelMG = "RottenFlesh";
                game.CountDrop = ManagerGame.countRottenFleshMG;
            }
            if (other.gameObject.GetComponentInChildren<DropScriptOther>().SpriteModel.name == "spider_eyeSprite")
            {
                ManagerGame.countspider_eyeMG++;
                game.SpriteModelMG = "spider_eye";
                game.CountDrop = ManagerGame.countspider_eyeMG;
            }
            if (other.gameObject.GetComponentInChildren<DropScriptOther>().SpriteModel.name == "stringSprite")
            {
                ManagerGame.countStringMG++;
                game.SpriteModelMG = "string";
                game.CountDrop = ManagerGame.countStringMG;
            }
            ManagerGame.ContItemDrop++;
        }
    }
}
