using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphinxListener : MonoBehaviour
{
    public enum language
    {
        en_US,
        ru_RU,
    }
    [System.Serializable]
    public struct keyword
    {
        public string Word;
        public string Trancription;
    }
    [Header("Language")]
    public language lang = language.ru_RU;
    [Header("Keywords")]
     public List<keyword> keywords = new List<keyword>();
}
