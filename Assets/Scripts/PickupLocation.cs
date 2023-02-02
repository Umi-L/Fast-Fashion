using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupLocation : Interactable
{
    private GameObject image;
    private SpriteRenderer spriteRenderer;
    
    private GameObject progressDisplay;

    private List<Items.CraftingItem> inventory = new List<Items.CraftingItem>();

    [SerializeField]
    public Sprite billboard;
    public Items.CraftingItem item;
    public float refreshTime = 1f;
    public  int maxItems = 5;

    float refreshTimer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        base.HideAllInteractionPoints();
        
        image = transform.GetChild(0).GetChild(0).gameObject;
        spriteRenderer = image.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = billboard;
        
        image.transform.rotation = Camera.main.gameObject.transform.rotation;
        transform.GetChild(1).rotation = Camera.main.gameObject.transform.rotation;

        progressDisplay = transform.GetChild(1).GetChild(0).gameObject;
        
        //add one item on start
        inventory.Add(item);
        UpdateDisplay();
    }
    
    public void UpdateDisplay()
    {
        var inventoryDisplay = transform.GetChild(0).GetChild(1);
        
        //destroy all children of head
        foreach (Transform child in inventoryDisplay.transform)
        {
            Destroy(child.gameObject);
        }

        var items = inventory;
        
        float height = 0;
        foreach (var item in items)
        {
            var itemPrefab = Instantiate(Items.GetItemPrefab(item));

            var combinedBounds = new Bounds();
            var renderers = itemPrefab.GetComponentsInChildren<Renderer>();
            foreach (Renderer render in renderers)
            {
                combinedBounds.Encapsulate(render.bounds);
            }

            itemPrefab.transform.SetParent(inventoryDisplay);

            itemPrefab.transform.localPosition = new Vector3(0, height, 0);

            height += combinedBounds.size.y + 0.1f;
        }
        
    }

    public override void Interact(int point)
    {
        base.Interact(point);

        if (inventory.Count <= 0)
        {
            return;
        }
        
        // interact with pickup location
        var player = GameObject.FindWithTag("Player");
        var playerScript = player.GetComponent<Player>();
        
        playerScript.AddItemsToInventory(new List<Items.CraftingItem>() {inventory[0]});
        inventory.RemoveAt(0);
        UpdateDisplay();
    }

    private void Update()
    {
        if (inventory.Count >= maxItems)
        {
            return;
        }
        refreshTimer += Time.deltaTime;
        if (refreshTimer >= refreshTime)
        {
            refreshTimer = 0f;
            
            inventory.Add(item);
            UpdateDisplay();
        }
        
        progressDisplay.GetComponent<Image>().fillAmount = GetRefreshProgress();
    }

    public float GetRefreshProgress()
    {
        return refreshTimer / refreshTime;
    }
    
    public override void DisplayInteractionPoint(int index)
    {
        base.DisplayInteractionPoint(index);
        
        // display interaction point
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }
    
    public override void HideInteractionPoint(int index)
    {
        base.HideInteractionPoint(index);
        
        // hide interaction point
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }
}
