using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    private float catSpeed = 3.0f;
    private float turnAmounts;

    private float lifespan = 10f;
    
    private Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        turnAmounts = Random.Range(0, 360);
        
        transform.Rotate(new Vector3(0, turnAmounts, 0));
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * catSpeed;
        
        lifespan -= Time.deltaTime;
        if (lifespan < 0)
        {
            var poof = Instantiate(Resources.Load("Prefabs/PoofEffect")) as GameObject;
            poof.transform.position = transform.position;
            
            Destroy(gameObject);
        }
        
        //when player is within 1.5 units of cat, destroy cat and make player drop items
        if (player != null)
        {
            var distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < 1.5f)
            {
                player.DropAllItems();
                
                // instantiate Poof game object
                var poof = Instantiate(Resources.Load("Prefabs/PoofEffect")) as GameObject;
                poof.transform.position = transform.position;
                
                Destroy(gameObject);
            }
        }
    }
}