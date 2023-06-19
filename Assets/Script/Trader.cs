using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour, IInteractable
{
    public FirstPersonController fpc; 
    public GameObject Panel;
    public bool isOn;
    public GameObject Rectangle;
    private void Start()
    {
        Panel.SetActive(isOn);
    }
    public void Interact()
    {
        isOn=!isOn;
        if (isOn)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            fpc.enabled = false;
            Rectangle.SetActive(false);
            Panel.SetActive(isOn);
        }
        else 
        {
            fpc.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Rectangle.SetActive(true);
            Panel.SetActive(isOn);
        }
        
        
    }
}
