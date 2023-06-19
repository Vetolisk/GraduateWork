using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WordsScript : MonoBehaviour
{

     [SerializeField]
    public List<string> WordsList = new List<string>();
    [SerializeField]
    public Wolf w;
    [SerializeField]
    public List<TextMeshProUGUI> WordsTMP = new List<TextMeshProUGUI>();
    [SerializeField]
    public List<int> repNum = new List<int>();

    [SerializeField]
    public SphinxListener Sl;
    private int oldWords;
    private int randomWords;

    public void SetWords(){
         randomWords = Random.Range(0, 21);
        for (int i = 0; i < WordsTMP.Count; i++)
        {
            for (int j = 0; j < repNum.Count; j++)
            {
                if(randomWords==repNum[j]){
                    randomWords = Random.Range(0, 21); 
                }
            }
             if(oldWords!=randomWords){
                repNum.Add(randomWords);
                WordsTMP[i].text=WordsList[randomWords].ToString();
                 w.Words.Add(WordsTMP[i].text);
                 oldWords=randomWords;
            }
            
            
            
        }
    }
    public void ClearList(){
        repNum.Clear();
        
    }

    
}
