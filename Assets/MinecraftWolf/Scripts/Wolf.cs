using Pocketsphinx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wolf : MonoBehaviour
{
    [SerializeField]
    public List<string> Words = new List<string>();
    
    [System.Serializable]
    public struct Commands
    {
        public string gnatKeyword;
        public string derzhatKeyword;
        public string smotretKeyword;
        public string videtKeyword;
        public string dishatKeyword;
        public string slishatKeyword;
        public string nenavidetKeyword;
        public string zavisetKeyword;
        public string vertetKeyword;
        public string obidetKeyword;
        public string terpetKeyword;

        public string otvechatKeyword;
        public string glotatKeyword;
        public string boletKeyword;
        public string kupatKeyword;
        public string dorozhitKeyword;
        public string osuzhdatKeyword;
        public string ocenivatKeyword;
        public string zhdatKeyword;
        public string miaukatKeyword;
        public string pozharitKeyword;
        public string chitatKeyword;
    }
    
    public Commands commands;
    
    public AudioClip[] barkSounds;
    public AudioClip[] footstepSounds;
    public Material red;
    public GameObject master;
    public float headTrackAngleLimit = 30;
    public float lookAtMasterToggleProbability = 0.005f;
    public float lookAtMasterTogglePause = 2;
    public float lookAtMasterMaxDistance = 5;
    public float chaseStartDistance = 7;
    public float runSpeed = 5;
    public float randomBarkingProbability = 0.002f;
    public float barkingPause = 1;
    public float lookToOtherObjectProbability = 0.5f;

    public static scriptCollider scriptColliders;
    private SphinxManager sphinx;
    private AudioSource audioSource;
    private AudioSource footstepsAudioSource;
    private Animator animator;
    private Transform headBone;
    private Quaternion headBoneRotation;
    private float lastlookAtMasterToggle;
    private float lastTimeBarked;
    private Transform target;
    //states
    bool lookAtMasterRandomly = false;
    bool lookAtMasterPurposely = false;
    bool interested = false;
    public static bool  flagError=false;
    public static bool SitWolf;

    public GameObject wordsListGO;
    [SerializeField]

    ManagerGame mg;
    

    // Start is called before the first frame update
    
    IEnumerator Start()
    {
        sphinx = FindObjectOfType<SphinxManager>();
        sphinx.OnSpeechRecognized += ExecuteCommand;
        audioSource = GetComponents<AudioSource>()[0];
        footstepsAudioSource = GetComponents<AudioSource>()[1];
        animator = GetComponent<Animator>();
        if (master == null) master = GameObject.FindGameObjectWithTag("Player");
        headBone = findChildRecursive(transform, "head");
        if (headBone != null) headBoneRotation = headBone.rotation;
        lastlookAtMasterToggle = Time.time;
        lastTimeBarked = Time.time;
        animator.SetBool("sitting", true);
        while (sphinx.mic == null)
        {
            yield return null;
        }
    }
    void Update()
    {
        if (animator != null && animator.GetInteger("speed") > 0)
        {
            if (!footstepsAudioSource.isPlaying)
            {
                footstepsAudioSource.clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
                footstepsAudioSource.Play();
            }
        }
        else if (animator.GetInteger("speed") < 0) { 
            footstepsAudioSource.Stop(); 
        }
        if (flagError)
        {
            NothingcVoice();
            
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log();
        //if ((Time.time - lastTimeBarked > barkingPause) &&
        //    ((float)Random.Range(1, 100) / 100 < randomBarkingProbability))
        //{
        //    Bark();
        //    lastTimeBarked = Time.time;
        //}
        if (master == null)
        {
            master= GameObject.FindGameObjectWithTag("Player");
        }
            if (Vector3.Distance(master.transform.position, transform.position) < lookAtMasterMaxDistance)
            {
                lookAtMasterPurposely = false; //не забыть поменять если будут причины смотреть и вблизи
                if (animator != null) animator.SetInteger("speed", 0);
                //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") && !animator.IsInTransition(0)) { animator.SetTrigger("stay"); Debug.Log("here"); }
                //target = master;
                if ((Time.time - lastlookAtMasterToggle > lookAtMasterTogglePause) &&
                    ((float)Random.Range(1, 100 * 100) / 100 < lookAtMasterToggleProbability))
                {
                    TryToSetTarget(chaseStartDistance);
                    lookAtMasterRandomly = !lookAtMasterRandomly;
                    lastlookAtMasterToggle = Time.time;
                }
            }
        
        //else lookAtMasterRandomly = false;
        //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Sit") && !animator.IsInTransition(0))
        if (animator != null && !animator.GetBool("sitting"))
        {
            //random activities
            if (Vector3.Distance(master.transform.position, transform.position) > chaseStartDistance)
            {
                target = master.transform;
                lookAtMasterPurposely = true;
                if (animator != null) animator.SetInteger("speed", 1);
            }
            transform.position += transform.forward * runSpeed * animator.GetInteger("speed") * Time.deltaTime;
        }
        //if (animator.GetAnimatorTransitionInfo(0).IsName("runToStay")) transform.position += transform.forward * runSpeed * animator.GetAnimatorTransitionInfo(0).normalizedTime * Time.deltaTime;
    }
    void LateUpdate()
    {
    
        if (!flagError) {
            LookAt(target);
        }
           
    }
    private void OnDestroy()
    {
        sphinx.OnSpeechRecognized -= ExecuteCommand;
        Debug.Log("Sven destroyed");
    }
    void ExecuteCommand(string str)
    {
        bool executed = false;
       /* if (str.Contains(commands.sitKeyword)){
            executed = Sit(); 
            SitWolf = true;
        }
        else if (str.Contains(commands.barkKeyword)){
            executed = Bark();
        }
        else if(str.Contains(commands.znachimostKeyword)){
            executed = znachimost();
        }
        else if (str.Contains(commands.standKeyword)) {
            executed = Stay();
            SitWolf = false;
        }

        else if (str.Contains(commands.searchKeyword))
        {
            executed = Search();
        }
        if (SitWolf) {

             if (str.Contains(commands.enemyKeyword)) executed = Enemy();
        }
        */    
        Debug.Log(str);  
        if(str.Contains(commands.gnatKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="гнать"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.green;
                    ManagerGame.countRottenFleshMG+=5;
                    PlayerPrefs.SetInt("rotten_fleshSprite", ManagerGame.countRottenFleshMG);
                    mg.textItem[1].text =ManagerGame.countRottenFleshMG.ToString();
                }
            }
        }
        
        if(str.Contains(commands.smotretKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="смотреть"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.green;
                    ManagerGame.countRottenFleshMG+=5;
                    PlayerPrefs.SetInt("rotten_fleshSprite", ManagerGame.countRottenFleshMG);
                    mg.textItem[1].text =ManagerGame.countRottenFleshMG.ToString();
                }
            }      
        }

        if(str.Contains(commands.derzhatKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="держать"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.green;
                    ManagerGame.countRottenFleshMG+=5;
                    PlayerPrefs.SetInt("rotten_fleshSprite", ManagerGame.countRottenFleshMG);
                    mg.textItem[1].text =ManagerGame.countRottenFleshMG.ToString();
                }
            }
        }

         if(str.Contains(commands.videtKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="видеть"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.green;
                    ManagerGame.countRottenFleshMG+=5;
                    PlayerPrefs.SetInt("rotten_fleshSprite", ManagerGame.countRottenFleshMG);
                    mg.textItem[1].text =ManagerGame.countRottenFleshMG.ToString();
                }
            }
        }
        if(str.Contains(commands.dishatKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="дышать"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.green;
                    ManagerGame.countRottenFleshMG+=5;
                    PlayerPrefs.SetInt("rotten_fleshSprite", ManagerGame.countRottenFleshMG);
                    mg.textItem[1].text =ManagerGame.countRottenFleshMG.ToString();
                }
            }
        }
        if(str.Contains(commands.slishatKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="слышать"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.green;
                    ManagerGame.countRottenFleshMG+=5;
                    PlayerPrefs.SetInt("rotten_fleshSprite", ManagerGame.countRottenFleshMG);
                    mg.textItem[1].text =ManagerGame.countRottenFleshMG.ToString();
                }
            }
        }
        if(str.Contains(commands.nenavidetKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="ненавидеть"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.green;
                    ManagerGame.countRottenFleshMG+=5;
                    PlayerPrefs.SetInt("rotten_fleshSprite", ManagerGame.countRottenFleshMG);
                    mg.textItem[1].text =ManagerGame.countRottenFleshMG.ToString();
                }
            }
        }
        if(str.Contains(commands.zavisetKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="зависеть"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.green;
                    ManagerGame.countRottenFleshMG+=5;
                    PlayerPrefs.SetInt("rotten_fleshSprite", ManagerGame.countRottenFleshMG);
                    mg.textItem[1].text =ManagerGame.countRottenFleshMG.ToString();
                }
            }
        }
        if(str.Contains(commands.vertetKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="вертеть"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.green;
                    ManagerGame.countRottenFleshMG+=5;
                    PlayerPrefs.SetInt("rotten_fleshSprite", ManagerGame.countRottenFleshMG);
                    mg.textItem[1].text =ManagerGame.countRottenFleshMG.ToString();
                }
            }
        }
        if(str.Contains(commands.obidetKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="обидеть"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.green;
                    ManagerGame.countRottenFleshMG+=5;
                    PlayerPrefs.SetInt("rotten_fleshSprite", ManagerGame.countRottenFleshMG);
                    mg.textItem[1].text =ManagerGame.countRottenFleshMG.ToString();
                }
            }
        }
        if(str.Contains(commands.terpetKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="терпеть"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.green;
                    ManagerGame.countRottenFleshMG+=5;
                    PlayerPrefs.SetInt("rotten_fleshSprite", ManagerGame.countRottenFleshMG);
                    mg.textItem[1].text =ManagerGame.countRottenFleshMG.ToString();
                }
            }
        }
        if(str.Contains(commands.otvechatKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="отвечать"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.red;
                }
            }

        }
        if(str.Contains(commands.glotatKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="глотать"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.red;
                }
            }
        }
        if(str.Contains(commands.boletKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="болеть"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.red;
                }
            }
        }
        if(str.Contains(commands.kupatKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="купать"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.red;
                }
            }
        }
        if(str.Contains(commands.dorozhitKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="дорожить"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.red;
                }
            }
        }
        if(str.Contains(commands.ocenivatKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="оценивать"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.red;
                }
            }
        }
        if(str.Contains(commands.zhdatKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="ждать"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.red;
                }
            }
        }
        if(str.Contains(commands.miaukatKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="мяукать"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.red;
                }
            }
        }
        if(str.Contains(commands.pozharitKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="пожарить"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.red;
                }
            }
        }
        if(str.Contains(commands.chitatKeyword)){
             foreach (Transform item in wordsListGO.transform)
            {
                if(item.GetComponent<TextMeshProUGUI>().text=="читать"){
                    item.GetComponent<TextMeshProUGUI>().color=Color.red;
                }
            }
        }

        if (executed)
        {
            target = master.transform;
            lookAtMasterRandomly = true;
            lastlookAtMasterToggle = Time.time;
        }

    }
    bool Search()
    {
       
        
        Debug.Log("Search");
        return true;
    }
    bool Enemy()
    {
        if (scriptColliders.Entity != null)
        {
            scriptColliders.Entity.tag = "Enemy";
            scriptColliders.Entity.layer = 3;

            Debug.Log("Enemy");
        }
        return true;
    }
    bool Friend()
    {
        Debug.Log("Friend");
        return true;
    }
    void NothingcVoice()
    {
        animator.SetLayerWeight(1, 1);
        //flagError = false;
        // Invoke("animError", 1);      
        Debug.Log("Error");
    }
    void animError()
    {
        //animator.SetLayerWeight(1, 0);
        //flagError = false;
    }
    bool ability(){
        Debug.Log("Правильно");
        return true;
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
    bool Sit()
    {
        if (animator == null) return false;
        animator.SetBool("sitting", true);
        Debug.Log("Sit");
        return true;
    }
    bool Stay()
    {
        if (animator == null) return false;
        animator.SetBool("sitting", false);
        return true;
    }
 
    void LookAt(Transform target)
    {
        if (headBone != null)
        {
            if (target != null)
            {
                if (Vector3.Angle(headBone.up, target.position - headBone.position) > headTrackAngleLimit && (lookAtMasterPurposely || lookAtMasterRandomly))
                {
                    Vector3 lookPos = target.position - transform.position;
                    lookPos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2f);
                }
                if (lookAtMasterPurposely || lookAtMasterRandomly)
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
    void TryToSetTarget(float maxDistance)
    {
        Rigidbody[] allRb = Object.FindObjectsOfType<Rigidbody>();
        List<Transform> nearestRb = new List<Transform>();
        foreach (Rigidbody rb in allRb)
        {
            if (Vector3.Distance(rb.position, transform.position) < maxDistance && rb.transform != transform) nearestRb.Add(rb.transform);
        }
        if (nearestRb.Count > 0 && ((float)Random.Range(1, 100) / 100 < lookToOtherObjectProbability)) target = nearestRb[Random.Range(0, nearestRb.Count)];
        else target = master.transform;
    }
}
