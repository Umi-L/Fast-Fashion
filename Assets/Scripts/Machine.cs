using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Machine : Interactable
{
    
    [SerializeField] 
    public Items.CraftingItem[] inputs;
    public Items.CraftingItem outputItem;
    public float craftingTime = 5f;
    public bool toggleable = false;
    
    public bool toggled = false;
    
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

    public void UpdateRecipePreview(GameObject parent)
    {
        //clear parent children
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
        
        // get amount of each item in inputItems
        Dictionary<Items.CraftingItem, int> itemCounts = new Dictionary<Items.CraftingItem, int>();
        foreach (var item in inputs)
        {
            if (itemCounts.ContainsKey(item))
            {
                itemCounts[item]++;
            }
            else
            {
                itemCounts.Add(item, 1);
            }
        }
        
        
        float offset = 0f;
        float padding = 0.1f;
        // for each dict item make a new RecipeTemplate object from resources
        foreach (var item in itemCounts)
        {
            var recipeTemplate = Instantiate((GameObject) Resources.Load("Prefabs/RecipeTemplate"), parent.transform);
            recipeTemplate.GetComponent<RecipeTemplate>().SetItem(item.Key, item.Value);
            
            //set position
            recipeTemplate.transform.localPosition = new Vector3(offset, 0f, 0f);
            
            //add renderer width to offset
            offset += recipeTemplate.transform.GetChild(0).GetComponent<RectTransform>().rect.width + padding;
        }
        
        //add arrow
        var arrow = Instantiate((GameObject) Resources.Load("Prefabs/Arrow"), parent.transform);
        arrow.transform.localPosition = new Vector3(offset, 0f, 0f);
        var arrowimage = arrow.transform.GetChild(0).GetComponent<RectTransform>();
        offset += (arrowimage.rect.width * arrow.transform.localScale.x) + padding*4;
        
        //add output item
        var outputTemplate = Instantiate((GameObject) Resources.Load("Prefabs/RecipeTemplate"), parent.transform);
        outputTemplate.GetComponent<RecipeTemplate>().SetItem(outputItem, 1);
        
        //set position
        outputTemplate.transform.localPosition = new Vector3(offset, 0f, 0f);

        //center items in parent
        foreach (Transform child in parent.transform)
        {
            child.localPosition -= new Vector3(offset/2, 0, 0);
        }
        
        //set each element opacity to 50% on its image compnent
        foreach (Transform child in parent.transform)
        {
            Image image = child.GetChild(0).GetComponent<Image>();
            var tempColor = image.color;
            tempColor.a = 0.8f;
            image.color = tempColor;
        }

        parent.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
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

        bool itemAdded = false;
        
        for (int i = inputs.Length-1; i >= 0; i--)
        {
            var item = inputs[i];
            if (items.Contains(item))
            {
                Debug.LogFormat("adding item to machine: {0}", item);
                
                inputInventory.Add(item);
                items.Remove(item);
                
                itemAdded = true;
                
                break;
            }
        }

        if (itemAdded)
        {
            GetComponent<AudioSource>().Play();
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

        if (copy.Count > 0)
        {
            GetComponent<AudioSource>().Play();
        }
        
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
