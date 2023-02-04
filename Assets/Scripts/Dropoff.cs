using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropoff : Interactable
{
    
    private GameObject dropoffPoint;
    private GameObject dropoffDisplay;
    
    // Start is called before the first frame update
    void Start()
    {
        dropoffPoint = transform.GetChild(0).gameObject;
        
        dropoffDisplay = transform.GetChild(0).GetChild(0).gameObject;
        
        dropoffDisplay.transform.rotation = Camera.main.gameObject.transform.rotation;
        
        
        HideInteractionPoint(0);
    }
    
    public override void Interact(int index)
    {
        base.Interact(index);

        Player player = GameObject.FindObjectOfType<Player>();
        
        player.inventory = GameManager.Instance.DropoffItems(player.inventory);
        player.UpdateInventory();
    }
    
    public override void DisplayInteractionPoint(int index)
    {
        base.DisplayInteractionPoint(index);
        
        // display interaction point
        dropoffDisplay.SetActive(true);
    }
    
    public override void HideInteractionPoint(int index)
    {
        base.HideInteractionPoint(index);
        
        // hide interaction point
        dropoffDisplay.SetActive(false);
    }

}
