using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{

    [SerializeField] public Items.CraftingItem itemType;
    
    Player player;
    
    float pickupTime = 1.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Items.GetItemPrefab(itemType), transform);
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (pickupTime < 0)
        {
            // if nearby player add itemType to player inventory and destroy self
            if (player != null)
            {
                var distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance < 2)
                {
                    player.inventory.Add(itemType);
                    player.UpdateInventory();
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            pickupTime -= Time.deltaTime;
        }
    }
}
