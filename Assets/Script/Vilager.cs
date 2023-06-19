using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Vilager 
{
    public float[] position;
    public Vilager(GameObject vilager)
    {
        position = new float[3];
        Vector3 vilpos = vilager.transform.position;
        position = new float[]
        {
            vilpos.x,vilpos.y,vilpos.z
        };
    }
}
