using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using static UnityEngine.GraphicsBuffer;

public class WolfMenu : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip[] barkSounds;
    public float lookToOtherObjectProbability = 0.5f;
    public float chaseStartDistance = 7;
    public float headTrackAngleLimit = 30;
    public float lookAtMasterToggleProbability = 0.005f;
    public float lookAtMasterTogglePause = 2;
    // поворот
    private Transform target;
    public Transform master;
    private Transform headBone;
    bool lookAtMasterRandomly = true;

    bool interested = false;
    private Quaternion headBoneRotation;
    private float lastlookAtMasterToggle;

    //
    public GameObject objMouse;
    private Vector3 pointScreen;
    private Vector3 offset;
    public float distance = 5f;
    public ParticleSystem effect;
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        animator.SetBool("sitting", true);
        StartCoroutine(ExecuteAfterTime());
        headBone = findChildRecursive(transform, "head");
        if (headBone != null) headBoneRotation = headBone.rotation;
        lastlookAtMasterToggle = Time.time;

    }
    private void OnMouseDown()
    {
        Instantiate(effect,new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y+1.5f, gameObject.transform.position.z), Quaternion.identity);
        Debug.Log("Love");
    }
    private void Update()
    {
       Vector3 mousePos = Input.mousePosition;
        objMouse.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distance));

    }
    private void FixedUpdate()
    {
        TryToSetTarget(chaseStartDistance);



    }
    private void LateUpdate()
    {
        LookAt(target);
    }

    bool Bark()
    {
        if (audioSource == null) return false;
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = barkSounds[Random.Range(0, barkSounds.Length)];
        audioSource.Play();
        Debug.Log("Bark");
        return true;
    }
    IEnumerator ExecuteAfterTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 10));
            Bark();
        }
       
    }
   
    void LookAt(Transform target)
    {
        if (headBone != null)
        {
            if (target != null)
            {
                if (Vector3.Angle(headBone.up, target.position - headBone.position) > headTrackAngleLimit && ( lookAtMasterRandomly))
                {
                    Vector3 lookPos = target.position - transform.position;
                    lookPos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2f);
                }
                if ( lookAtMasterRandomly)
                {
                    headBone.LookAt(target);
                    headBone.Rotate(90f, 0, 0, Space.Self);
                    if (interested) headBone.Rotate(0, 20, 0, Space.Self);
                }
            }
            headBone.rotation = Quaternion.Slerp(headBoneRotation, headBone.rotation, Time.deltaTime * 5f);
            headBoneRotation = headBone.rotation;
            //Quaternion lookRotation = Quaternion.LookRotation(master.position - headBone.position, Vector3.up);
            //headBone.rotation *= lookRotation;
        }
    }
    void TryToSetTarget(float maxDistance)
    {
        Rigidbody[] allRb = Object.FindObjectsOfType<Rigidbody>();
        List<Transform> nearestRb = new List<Transform>();
        foreach (Rigidbody rb in allRb)
        {
            if (Vector3.Distance(rb.position, transform.position) < maxDistance && rb.transform != transform) nearestRb.Add(rb.transform);
        }
        if (nearestRb.Count > 0 && ((float)Random.Range(1, 100) / 100 < lookToOtherObjectProbability)) target = nearestRb[Random.Range(0, nearestRb.Count)];
        else target = objMouse.transform;
    }
    Transform findChildRecursive(Transform t, string name)
    {
        if (t.childCount != 0)
        {
            foreach (Transform child in t)
            {
                if (child.name == name) return child;
                Transform f = findChildRecursive(child, name);
                if (f != null) return f;
            }
        }
        return null;
    }

}
