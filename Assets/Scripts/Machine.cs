using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : Interactable
{
    
    [SerializeField] 
    public Items.CraftingItem[] inputs;
    public Items.CraftingItem outputItem;
    public float craftingTime = 5f;

    private List<Items.CraftingItem> outputInventory = new List<Items.CraftingItem>();
    private List<Items.CraftingItem> inputInventory = new List<Items.CraftingItem>();

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
        
        //remove only items used in crafting
        foreach (var input in inputs)
        {
            inputInventory.Remove(input);
        }
        
        //start crafting
        craftingTimer = craftingTime;
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
        return items;
    }

    private void Craft()
    {
        outputInventory.Add(outputItem);
    }
    
    public List<Items.CraftingItem> TakeOutput()
    {
        List<Items.CraftingItem> copy = new List<Items.CraftingItem>(outputInventory);
        outputInventory.Clear();
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
    }
}
