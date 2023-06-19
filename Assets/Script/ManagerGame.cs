using TMPro;
using UnityEngine;
using System.Collections;

public class ManagerGame : MonoBehaviour
{
    [Header("Time")]
    bool flag=true;
    public bool isGameStopped = false;
    private int timer = 0;
    public TextMeshProUGUI TextMeshProFood;
    public GameObject wolf;
    public  int Food = 100;

    public Loader load;
    [Header("Text")]
    public TextMeshProUGUI[] textItem;


   
    public string SpriteModelMG;
    public static int ContItemDrop;
    public GameObject TextCount;
    private TextMeshProUGUI TextMeshPro;
    public int CountDrop = 0;

    [Header("Data")]
    public static int countBeefMG;
    public static int countEmeraldMG;
    public static int countgunpowderMG;
    public static int countRottenFleshMG;
    public static int countspider_eyeMG;
    public static int countStringMG;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Food"))
        {
            Food = 100;
        }
        else
        {
            Food = PlayerPrefs.GetInt("Food");
        }
        
        countBeefMG = PlayerPrefs.GetInt("beefSprite");
        textItem[2].text = countBeefMG.ToString();
        countgunpowderMG = PlayerPrefs.GetInt("gunpowderSprite");
        textItem[5].text = countgunpowderMG.ToString();
        countRottenFleshMG = PlayerPrefs.GetInt("rotten_fleshSprite");
        textItem[1].text = countRottenFleshMG.ToString();
        countspider_eyeMG = PlayerPrefs.GetInt("spider_eyeSprite");
        textItem[3].text = countspider_eyeMG.ToString();
        countStringMG = PlayerPrefs.GetInt("stringSprite");
        textItem[4].text = countStringMG.ToString();

    }
    private void Start()
    {
        StartCoroutine(WaitAndPrint());
        countEmeraldMG = PlayerPrefs.GetInt("emeraldSprite");
        textItem[0].text = countEmeraldMG.ToString();

        TextMeshPro = TextCount.GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (DropItem.flagDrop)
        {
            for (int i = 0; i < textItem.Length; i++)
            {
                if (textItem[i].name== SpriteModelMG)
                {
                    textItem[i].text = CountDrop.ToString();
                }
            }
            CountDrop = 0;
            DropItem.flagDrop = false;
        }
        TextMeshPro.text = ContItemDrop.ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
        {          
            PlayerPrefs.SetInt("beefSprite", countBeefMG);
            PlayerPrefs.SetInt("emeraldSprite", countEmeraldMG);
            PlayerPrefs.SetInt("gunpowderSprite", countgunpowderMG);
            PlayerPrefs.SetInt("rotten_fleshSprite", countRottenFleshMG);
            PlayerPrefs.SetInt("spider_eyeSprite", countspider_eyeMG);
            PlayerPrefs.SetInt("stringSprite", countStringMG);

            load.LoadLevel(0);
        }
    }
    public void Trade()
    {
        
        if (countRottenFleshMG >0)
        {
            countRottenFleshMG--;
            textItem[1].text = countRottenFleshMG.ToString();
            countEmeraldMG++;
            textItem[0].text = countEmeraldMG.ToString();

        }
        else
        {
            Debug.Log("Error");
        }
        
            
    }
    public void Trade2()
    {
        if (countEmeraldMG > 0)
        {
            countEmeraldMG--;
            textItem[0].text = countEmeraldMG.ToString();
            countBeefMG = PlayerPrefs.GetInt("beefSprite");
            countBeefMG +=2;
            textItem[2].text = countBeefMG.ToString();
            PlayerPrefs.SetInt("beefSprite", countBeefMG);

        }
        else
        {
            Debug.Log("Error");
        }
    }
    IEnumerator WaitAndPrint()
    {
        while (flag)
        {
            if (!isGameStopped)
            {
                
                TextMeshProFood.text = "Еда: " + Food.ToString();                
                if (Food<=0)
                {
                    Destroy(wolf);
                    flag = false;
                    StopCoroutine("WaitAndPrint");
                }
                PlayerPrefs.SetInt("Food", Food);
                Food -= 10;
                yield return new WaitForSeconds(30f);
            }

            yield return null;
        }
    }
}
