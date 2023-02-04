using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Machine : Interactable
{
    
    [SerializeField] 
    public Items.CraftingItem[] inputs;
    public Items.CraftingItem outputItem;
    public float craftingTime = 5f;
    public bool toggleable = false;
    
    private bool toggled = false;
    
    public bool isCrafting = false;
    
    public List<Items.CraftingItem> outputInventory = new List<Items.CraftingItem>();
    public List<Items.CraftingItem> inputInventory = new List<Items.CraftingItem>();

    private float craftingTimer = 0f;
    
    

    public void StartCrafting()
    {
        List<Items.CraftingItem> copy = new List<Items.CraftingItem>(inputInventory);
        
        //check to see if we have all the items we need
        foreach (var input in inputs)
        {
            if (!copy.Contains(input))
            {
                return;
            }
            else
            {
                copy.Remove(input);
            }
        }
        
        bool itemRemoved = false;
        
        //remove only items used in crafting
        foreach (var input in inputs)
        {
            inputInventory.Remove(input);
            itemRemoved = true;
        }
        
        //create small poof effect if item was removed
        if (itemRemoved)
        {
            Instantiate(Resources.Load("Prefabs/SmallPoofEffect"), transform.position, Quaternion.identity);
        }
        
        isCrafting = true;
        
        UpdateDisplay();
        
        //start crafting
        craftingTimer = craftingTime;
    }

    public float GetCraftingProgress()
    {
        return craftingTimer / craftingTime;
    }
    
    public List<Items.CraftingItem> AddItems(List<Items.CraftingItem> items)
    {
        Debug.LogFormat("adding items to machine: {0}", items.Count);
        
        //check if inputs contains items if so add them
        
        for (int i = inputs.Length-1; i >= 0; i--)
        {
            var item = inputs[i];
            if (items.Contains(item))
            {
                Debug.LogFormat("adding item to machine: {0}", item);
                
                inputInventory.Add(item);
                items.Remove(item);
            }
        }
        
        UpdateDisplay();
        
        return items;
    }
    
    public List<Items.CraftingItem> AddItem(List<Items.CraftingItem> items)
    {
        Debug.LogFormat("adding items to machine: {0}", items.Count);
        
        //check if inputs contains items if so add them
        
        for (int i = inputs.Length-1; i >= 0; i--)
        {
            var item = inputs[i];
            if (items.Contains(item))
            {
                Debug.LogFormat("adding item to machine: {0}", item);
                
                inputInventory.Add(item);
                items.Remove(item);
                break;
            }
        }
        
        UpdateDisplay();
        
        return items;
    }


    public void UpdateDisplay()
    {
        for (int i = 0; i < base.InteractionPoints.Length; i++)
        {

            var interactionPoint = base.InteractionPoints[i];
            
            if (interactionPoint.childCount < 2)
            {
                continue;
            }
            
            var inventory = interactionPoint.GetChild(1);
            
            //destroy all children of head
            foreach (Transform child in inventory.transform)
            {
                Destroy(child.gameObject);
            }

            var items = GetInventoryFromInteractionPoint(i);
            
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

                itemPrefab.transform.SetParent(inventory);

                itemPrefab.transform.localPosition = new Vector3(0, height, 0);

                height += combinedBounds.size.y + 0.1f;
            }
        }
    }

    public virtual List<Items.CraftingItem> GetInventoryFromInteractionPoint(int point)
    {
        switch (point)
        {
            case 0:
                return inputInventory;
            case 1:
                return outputInventory;
            default:
                return new List<Items.CraftingItem>();
        }
    }

    private void Craft()
    {
        outputInventory.Add(outputItem);
        
        //create small poof effect
        Instantiate(Resources.Load("Prefabs/SmallPoofEffect"), transform.position, Quaternion.identity);
        
        isCrafting = false;

        UpdateDisplay();
    }

    public void StartCraftingOrToggle()
    {
        if (toggleable)
        {
            toggled = !toggled;
        }
        else
        {
            StartCrafting();
        }
    }
    
    public List<Items.CraftingItem> TakeOutput()
    {
        List<Items.CraftingItem> copy = new List<Items.CraftingItem>(outputInventory);
        outputInventory.Clear();
        
        UpdateDisplay();
        
        return copy;
    }

    public void UpdateTimer()
    {
        if (craftingTimer > 0)
        {
            craftingTimer -= Time.deltaTime;
            if (craftingTimer <= 0)
            {
                craftingTimer = 0;
                Craft();
            }
        }
        else
        {
            if (toggled)
            {
                StartCrafting();
            }
        }
    }
}
