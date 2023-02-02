using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PickupLocation : Interactable
{
    private GameObject image;
    private SpriteRenderer spriteRenderer;
    
    [SerializeField] public Sprite billboard;
    [SerializeField] public CraftingItem item;

    public int ItemCount = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        image = transform.GetChild(0).gameObject;
        spriteRenderer = image.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = billboard;
        
        image.transform.rotation = Camera.main.gameObject.transform.rotation;
    }

    public override void Interact(int point)
    {
        base.Interact(point);
        
        // interact with pickup location
        var player = GameObject.FindWithTag("Player");
        var playerScript = player.GetComponent<Player>();
        playerScript.AddItemsToInventory(new List<CraftingItem>() {item});
    }
    
    public override void DisplayInteractionPoint(int index)
    {
        base.DisplayInteractionPoint(index);
        
        // display interaction point
        transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
    }
    
    public override void HideInteractionPoint(int index)
    {
        base.HideInteractionPoint(index);
        
        // hide interaction point
        transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
    }
}
