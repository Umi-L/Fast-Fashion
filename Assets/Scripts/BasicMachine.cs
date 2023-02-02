using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

public class BasicMachine : Machine
{
    [SerializeField]
    public Sprite WantedItems;
    public Sprite OutputItem;

    private GameObject inputDisplay;
    private GameObject outputDisplay;
    private GameObject interactableDisplay;

    private GameObject player;
    
    private void Start()
    {
        inputDisplay = transform.GetChild(0).gameObject;
        outputDisplay = transform.GetChild(1).gameObject;
        interactableDisplay = transform.GetChild(2).gameObject;
        
        inputDisplay.GetComponent<SpriteRenderer>().sprite = WantedItems;
        outputDisplay.GetComponent<SpriteRenderer>().sprite = OutputItem;
        
        var camRotation = Camera.main.gameObject.transform.rotation;

        inputDisplay.transform.rotation = camRotation;
        outputDisplay.transform.rotation = camRotation;
        interactableDisplay.transform.rotation = camRotation;

        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        base.UpdateTimer();
    }

    public override void Interact(int point)
    {
        base.Interact(point);

        switch (point)
        {
            case 0:
                // interact with input
                
                //add player inventory to machine inventory
                var playerScript = player.GetComponent<Player>();
                playerScript.inventory = base.AddItems(playerScript.inventory);
                break;
            case 1:
                // interact with output
                
                // take machine output inventory and add it to player inventory
                var playerScript2 = player.GetComponent<Player>();
                
                // merge player inventory with base.TakeOutput();
                playerScript2.inventory.AddRange(base.TakeOutput());
                
                print(playerScript2.inventory);
                
                break;
            case 2:
                // interact with machine
                
                print("Starting crafting");
                
                base.StartCrafting();
                
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
            case 1:
                outputDisplay.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 2:
                interactableDisplay.transform.GetChild(0).gameObject.SetActive(true);
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
            case 1:
                outputDisplay.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case 2:
                interactableDisplay.transform.GetChild(0).gameObject.SetActive(false);
                break;
            default:
                print("Invalid interaction point");
                break;
        }
    }
}
