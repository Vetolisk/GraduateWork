using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] Ac;
    public AudioSource As;
    // Start is called before the first frame update
    void Start()
    {
        As=gameObject.GetComponent<AudioSource>();
        As.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
