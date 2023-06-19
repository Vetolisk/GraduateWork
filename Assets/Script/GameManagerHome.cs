using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
public class GameManagerHome : MonoBehaviour
{
     
    public Loader load;
    public SaveSystem ss;
    public NavMeshSurface meshSurface;
    public BuyBuildManager BBM;
    bool yourBool;
    private void Awake()
    {
        for (int i = 0; i < BBM.SetBuild.Count; i++)
        {
            yourBool = (PlayerPrefs.GetInt(i.ToString()) != 0);
            Debug.Log(yourBool);
            BBM.SetBuild[i].SetActive(yourBool);
        }
        meshSurface.BuildNavMesh();
    }
    private void Start()
    {
        

        
        
    }
   
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            for (int i = 0; i < BBM.SetBuild.Count; i++)
            {
                PlayerPrefs.SetInt(i.ToString(), (BBM.SetBuild[i].activeSelf ? 0:0));
            }
            SceneManager.LoadScene("Menu");

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            for (int i = 0; i < BBM.SetBuild.Count; i++)
            {
                PlayerPrefs.SetInt(i.ToString(), (BBM.SetBuild[i].activeSelf ? 1 : 0));
            }

            ss.SaveVilager();

            load.LoadLevel(0);

        }
    }
   
    
       


}
