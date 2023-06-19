using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Slider slider;
    public Text proggresText;
    public void LoadLevel(int sceneIndex)
    {
        LoadingScreen.SetActive(true); 
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation=false;


        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            proggresText.text = progress*100f + "%";
            if (operation.progress>=.9f&&!operation.allowSceneActivation)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
