using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class DropScriptOther : MonoBehaviour
{
    private AudioSource audioSource;
    public Sprite SpriteModel;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            DropUoif();
        }
    }
     
    public void DropUoif()
    {
        audioSource.Play();
        Destroy(gameObject, 0.4f);
    }
}
