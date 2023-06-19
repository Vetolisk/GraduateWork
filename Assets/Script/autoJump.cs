using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class autoJump : MonoBehaviour
{
    public Rigidbody wolfrb;
    public float jumpAmount;
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Ground")
        {
            wolfrb.AddForce(Vector2.up * jumpAmount, ForceMode.Impulse);
            Debug.Log("Jump");
        }
    }
}
