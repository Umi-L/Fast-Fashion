using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Renderer areaBounds;
    
    [SerializeField] public float catSpawnTime = 10;
    
    private float catSpawnTimer = 0f;

    private Player player;

    [FormerlySerializedAs("GUI")] public GameObject HUD;
    
    public float timeLimit;

    public bool roundOver = false;
    
    private Round[] quotas =
    {
        new Round()
        {
            quotas = new[]{
                new Quota() { count = 2, item = Items.CraftingItem.Jeans },
                new Quota() { count = 1, item = Items.CraftingItem.LeatherWallet },
                new Quota() { count = 2, item = Items.CraftingItem.Button },
            },
            timeLimit = 120.0f
        },
        new Round()
        {
            quotas = new[]{
                new Quota() { count = 1, item = Items.CraftingItem.Sweatpants },
                new Quota() { count = 2, item = Items.CraftingItem.Jeans },
                new Quota() { count = 4, item = Items.CraftingItem.Button },
            },
            timeLimit = 120.0f
        },
        new Round()
        {
            quotas = new[]{
                new Quota() { count = 1, item = Items.CraftingItem.Pyjamas },
                new Quota() { count = 1, item = Items.CraftingItem.Hoodie },
                new Quota() { count = 1, item = Items.CraftingItem.Sweatpants },
                new Quota() { count = 2, item = Items.CraftingItem.RedCloth },
            },
            timeLimit = 180.0f
        },
    };
    
    

    public int currentStage = 0;

    struct Quota
    {
        public int count;
        public Items.CraftingItem item;
    }

    struct Round
    {
        public Quota[] quotas;
        public float timeLimit;
    }
    
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
    }

    private void Start()
    {
        areaBounds = GameObject.FindWithTag("AreaBounds").GetComponent<Renderer>();
        
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        HUD = GameObject.FindWithTag("HUD");

        UpdateQuotaDisplay();
        
        timeLimit = quotas[currentStage].timeLimit;
    }

    private void Update()
    {
        //quota countdown
        timeLimit -= Time.deltaTime;

        if (timeLimit <= 0)
        {
            EndRound();
        }

        UpdateTimerDisplay();
        
        //cat spawning
        catSpawnTimer += Time.deltaTime;
        if (catSpawnTimer > catSpawnTime)
        {
            catSpawnTimer = 0f;
            SpawnCat();
        }
    }

    private void UpdateQuotaDisplay()
    {
        var todo = HUD.transform.GetChild(0).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();

        todo.text = "TODO:\n";
        
        foreach (var quota in quotas[currentStage].quotas)
        {

            var prefix = "";
            var suffix = "";
            
            if (quota.count == 0)
            {
                prefix = "<S>";
                suffix = "</S>";
            }
            
            todo.text += prefix + quota.item + ": " + quota.count + suffix + "\n";
        }
    }
    
    public void UpdateTimerDisplay()
    {
        if (roundOver) return;
        var timer = HUD.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        timer.text = FloatToTime(timeLimit);
    }
    
    public string FloatToTime(float time)
    {
        var minutes = Mathf.FloorToInt(time / 60);
        var seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RestartLevel()
    {
        //unity load current level
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    
    public void EndRound()
    {
        if (roundOver) return;

        roundOver = true;
        
        print("ROUND END");
        
        //loop through quotas and check if all counts are 0
        var allDone = true;
        foreach (var quota in quotas[currentStage].quotas)
        {
            if (quota.count > 0)
            {
                allDone = false;
                break;
            }
        }
        
        if (allDone)
        {
            print("ALL DONE");
            var cheque = HUD.transform.GetChild(0).GetChild(3);
            cheque.gameObject.SetActive(true);
            cheque.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "$" + (currentStage + 1) * 100;
            
            GameObject.FindWithTag("MusicLoop").GetComponent<AudioSource>().Stop();
            
            GetComponents<AudioSource>()[0].Play();
        }
        else
        {
            print("NOT DONE");
            var cheque = HUD.transform.GetChild(0).GetChild(4);
            cheque.gameObject.SetActive(true);
            
            GameObject.FindWithTag("MusicLoop").GetComponent<AudioSource>().Stop();

            GetComponents<AudioSource>()[1].Play();
        }
    }

    public void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public List<Items.CraftingItem> DropoffItems(List<Items.CraftingItem> items)
    {
        bool itemDroppedOff = false;
        
        //find items in quota and remove them. return remaining items
        var remainingItems = items;
        for (int i=0; i < items.Count; i++)
        {
            var item = items[i];

            //for loop through quotas[currentStage]
            for (int j=0; j < quotas[currentStage].quotas.Length; j++)
            {
                var quota = quotas[currentStage].quotas[j];
                
                if (quota.item == item && quota.count > 0)
                {
                    quotas[currentStage].quotas[j].count--;
                    remainingItems.Remove(item);
                    
                    itemDroppedOff = true;
                }
            }
        }
        
        if (itemDroppedOff)
        {
            Instantiate(Resources.Load("Prefabs/RewardEffect"), player.transform.position, Quaternion.identity);
        }
        
        bool allDone = true;
        foreach (var quota in quotas[currentStage].quotas)
        {
            if (quota.count > 0)
            {
                allDone = false;
                break;
            }
        }

        if (allDone)
        {
            EndRound();
        }
        
        UpdateQuotaDisplay();
        
        return remainingItems;
    }

    private void SpawnCat()
    {
        
        float posX = UnityEngine.Random.Range(areaBounds.bounds.min.x, areaBounds.bounds.max.x);
        float posZ = UnityEngine.Random.Range(areaBounds.bounds.min.z, areaBounds.bounds.max.z);

        if (!player.PointInCamera(new Vector3(posX, 0, posZ)))
        {

            GameObject cat = Instantiate(Resources.Load("Prefabs/Cat")) as GameObject;

            cat.transform.position = new Vector3(posX, 0, posZ);
        }

        else
        {
            SpawnCat();
        }
    }
}
