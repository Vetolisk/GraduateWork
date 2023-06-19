using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.AI;
public class SaveSystem:MonoBehaviour
{
    [SerializeField] GameObject vilPrefab;

    public static List<GameObject> Vilagerobj=new List<GameObject>();
    const string vil_sub= "/Vilager";
    const string vil_cont_sub = "/Vilager";
    private void Awake()
    {
        VilagerLoader();
    }
    
   public void SaveVilager( )
   {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + vil_sub+SceneManager.GetActiveScene().buildIndex;
        Debug.Log(path);
        string countpath= Application.persistentDataPath + vil_cont_sub + SceneManager.GetActiveScene().buildIndex;
        FileStream countStream = new FileStream(countpath, FileMode.Create);
        formatter.Serialize(countStream, Vilagerobj.Count);
        countStream.Close();
        for (int i = 0; i < Vilagerobj.Count; i++)
        {
            FileStream stream = new FileStream(path+i, FileMode.Create);
            Vilager data = new Vilager(Vilagerobj[i]);
            formatter.Serialize(stream, data);
            stream.Close();
        }
        
       
        
        
   }
    public  void VilagerLoader()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + vil_sub + SceneManager.GetActiveScene().buildIndex;
        string countpath = Application.persistentDataPath + vil_cont_sub + SceneManager.GetActiveScene().buildIndex;
        int vilcount=0;
        if (File.Exists(countpath))
        {
            
            FileStream stream = new FileStream(countpath, FileMode.Open);


            vilcount = (int)formatter.Deserialize(stream);
            stream.Close();
           
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            
        }
        for (int i = 0; i < vilcount; i++)
        {
            if (File.Exists(path+i))
            {
                FileStream stream = new FileStream(countpath+i, FileMode.Open);
                Vilager data = formatter.Deserialize(stream) as Vilager;
                stream.Close();
                Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
                GameObject vil = Instantiate(vilPrefab, position, Quaternion.identity);
            }
            else
            {
                Debug.Log("Save file not found in " + path+i);
            }
               
        }
    }
}
