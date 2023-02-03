using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Renderer areaBounds;
    
    [SerializeField] public float catSpawnTime = 10;
    
    private float catSpawnTimer = 0f;

    private Player player;
    
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
    }

    private void Update()
    {
        catSpawnTimer += Time.deltaTime;
        if (catSpawnTimer > catSpawnTime)
        {
            catSpawnTimer = 0f;
            SpawnCat();
        }
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
