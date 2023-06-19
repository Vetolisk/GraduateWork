using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosions : MonoBehaviour
{
    public float Radius;
    public float Force;
    public bool Active;
    public GameObject Drop;
    public int SpawnCount;
    private void Start()
    {
        Sphere();
        Explode();
    }
   
    public void  Sphere()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            i++;
            Instantiate(Drop, transform.position + Random.insideUnitSphere * Radius, Quaternion.identity);
        }
       
            
            
       
    }
    public void Explode()
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, Radius);
        for (int i = 0; i < overlappedColliders.Length; i++)
        {
            if (overlappedColliders[i].tag!="Wolf") {
                Rigidbody rigidbody = overlappedColliders[i].attachedRigidbody;

                if (rigidbody)
                {
                    rigidbody.isKinematic = false;
                    rigidbody.AddExplosionForce(Force, transform.position, Radius);
                    Explosions explosion = rigidbody.GetComponent<Explosions>();
                    if (explosion)
                    {
                        if (Vector3.Distance(transform.position, rigidbody.position) < Radius / 2f)
                        {
                            explosion.Explode();
                        }
                    }
                }
            }
        }
        Destroy(gameObject);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius/2f);
    }
}
