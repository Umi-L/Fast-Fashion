using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Machine : Interactable
{
    
    [SerializeField] 
    public CraftingItem[] inputs;
    public CraftingItem outputItem;
    public float craftingTime = 5f;

    private List<CraftingItem> outputInventory = new List<CraftingItem>();
    private List<CraftingItem> inputInventory = new List<CraftingItem>();

    private float craftingTimer = 0f;
    
    

    public void StartCrafting()
    {
        List<CraftingItem> copy = new List<CraftingItem>(inputInventory);
        
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

    public List<CraftingItem> AddItems(List<CraftingItem> items)
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
    
    public List<CraftingItem> TakeOutput()
    {
        List<CraftingItem> copy = new List<CraftingItem>(outputInventory);
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
