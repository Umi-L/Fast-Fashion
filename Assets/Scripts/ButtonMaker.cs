using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMaker : Machine
{
    [SerializeField]
    public Sprite Recipe;

    private GameObject inputDisplay;
    private GameObject progressDisplay;
    private GameObject recipeDisplay;

    private Image recipeImage;

    private GameObject player;
    
    private void Start()
    {
        inputDisplay = transform.GetChild(0).gameObject;
        progressDisplay = transform.GetChild(1).GetChild(0).gameObject;
        recipeDisplay = transform.GetChild(1).GetChild(1).gameObject;

        //inputDisplay.GetComponent<SpriteRenderer>().sprite = WantedItems;
        //outputDisplay.GetComponent<SpriteRenderer>().sprite = OutputItem;
        
        var camRotation = Camera.main.gameObject.transform.rotation;

        inputDisplay.transform.GetChild(0).rotation = camRotation;
        progressDisplay.transform.rotation = camRotation;
        recipeDisplay.transform.rotation = camRotation;

        player = GameObject.FindWithTag("Player");
        
        base.HideAllInteractionPoints();
            
        recipeImage = recipeDisplay.GetComponent<UnityEngine.UI.Image>();
            
        recipeImage.sprite = Recipe;
        recipeImage.SetNativeSize();
    }

    private void Update()
    {
        base.UpdateTimer();
        
        var progress = base.GetCraftingProgress();
        progressDisplay.GetComponent<Image>().fillAmount = progress;
        if (progress == 0)
        {
            recipeDisplay.SetActive(true);
        }
        else
        {
            recipeDisplay.SetActive(false);
        }
    }

    public override void Interact(int point)
    {
        base.Interact(point);

        switch (point)
        {
            case 0:
                // interact with input
                
                //add player inventory to machine inventory and remove from player inventory
                var playerScript = player.GetComponent<Player>();
                if (base.outputInventory.Count == 0 && inputInventory.Count == 0 && !base.isCrafting)
                {
                    playerScript.inventory = base.AddItem(playerScript.inventory);
                    base.StartCraftingOrToggle();
                }
                else if (base.outputInventory.Count > 0 && base.outputInventory[0] == outputItem)
                {
                    // take machine output inventory and add it to player inventory
                    var playerScript2 = player.GetComponent<Player>();
                
                    // merge player inventory with base.TakeOutput();
                    playerScript2.AddItemsToInventory(base.TakeOutput());
                }

                playerScript.UpdateInventory();
                break;
            default:
                print("Invalid interaction point");
                break;
        }
    }
    
    public override void DisplayInteractionPoint(int point)
    {
        base.DisplayInteractionPoint(point);

        switch (point)
        {
            case 0:
                inputDisplay.transform.GetChild(0).gameObject.SetActive(true);
                break;
            default:
                print("Invalid interaction point");
                break;
        }
    }

    public override void HideInteractionPoint(int point)
    {
        base.DisplayInteractionPoint(point);

        switch (point)
        {
            case 0:
                inputDisplay.transform.GetChild(0).gameObject.SetActive(false);
                break;
            default:
                print("Invalid interaction point");
                break;
        }
    }
}
