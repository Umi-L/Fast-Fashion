using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 5.0f;
    private float gravityValue = -9.81f;
    
    private Camera followerCamera;
    private Vector3 offset = new Vector3(0, 10, -10);
    
    private Animator animator;
    
    public List<Items.CraftingItem> inventory = new List<Items.CraftingItem>();
    
    float interactionDistance = 3.0f;

    private GameObject head;

    private Interactable lastFrameInteractable;
    private int lastFrameInteractionPoint = -1;

    // Start is called before the first frame update
    void Start()
    {
        print("starting player");
        
        controller = gameObject.GetComponent<CharacterController>();

        followerCamera = transform.parent.GetChild(1).gameObject.GetComponent<Camera>();
        
        animator = gameObject.GetComponent<Animator>();
        head = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            DropAllItems();
        }
        
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
        // if the player is moving, play the walk animation
        if (Mathf.Abs(move.x) > 0.1 || Mathf.Abs(move.z) > 0.1)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
        

        // if the camera is too far away, move it closer
        Vector3 distance = ((followerCamera.transform.position - offset) - transform.position);
        
        if (Mathf.Abs(distance.x) > 7 || Mathf.Abs(distance.z) > 5)
        {
            followerCamera.transform.position = Vector3.Lerp(followerCamera.transform.position, transform.position + offset, 1*Time.deltaTime);
        }

        GetNearbyInteractions();
    }

    public void AddItemsToInventory(List<Items.CraftingItem> items)
    {
        //combine items and inventory
        inventory.AddRange(items);

        UpdateInventory();
    }

    public bool PointInCamera(Vector3 point)
    {
        var focalPoint = (followerCamera.transform.position - offset);
        
        var distance = focalPoint - point;
        
        if (Mathf.Abs(distance.x) > 13.5 || Mathf.Abs(distance.z) > 10)
        {
            return false;
        }
        return true;
    }

    public void DropAllItems()
    {
        foreach (var item in inventory)
        {
            var droppedItemPrefab = Resources.Load<GameObject>("Prefabs/DroppedItem");
            var droppedItem = Instantiate(droppedItemPrefab);
            droppedItem.GetComponent<Rigidbody>().AddForce(transform.forward * 5, ForceMode.Impulse);
            droppedItem.transform.position = transform.position + new Vector3(0, 2, 0);
            droppedItem.GetComponent<DroppedItem>().itemType = item;
        }
        
        inventory.Clear();
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        //destroy all children of head
        foreach (Transform child in head.transform)
        {
            Destroy(child.gameObject);
        }
        
        float height = 0;
        foreach (var item in inventory)
        {
            var itemPrefab = Instantiate(Items.GetItemPrefab(item));

            var combinedBounds = new Bounds();
            var renderers = itemPrefab.GetComponentsInChildren<Renderer>();
            foreach (Renderer render in renderers) {
                combinedBounds.Encapsulate(render.bounds);
            }
            
            itemPrefab.transform.SetParent(head.transform);

            itemPrefab.transform.localPosition = new Vector3(0, height, 0);
            
            height += combinedBounds.size.y + 0.5f;
        }
    }
    
    void GetNearbyInteractions()
    {
        // get all the interactable objects in the scene
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
        
        float nearestDistance = float.MaxValue;
        Interactable nearestInteractable = null;
        int nearestInteractableIndex = 0;

        // for each interactable object
        foreach (GameObject interactable in interactables)
        {
            var interactScript = interactable.GetComponent<Interactable>();
            
            foreach (var interactionPoint in interactScript.InteractionPoints)
            {
                //get distance between interactionpoint and player
                float distance = Vector3.Distance(interactionPoint.transform.position, transform.position);
                
                // if the distance is less than interactionDistance and less than the nearest distance
                if (distance < interactionDistance && distance < nearestDistance)
                {
                    //set nearest
                    nearestDistance = distance;
                    nearestInteractable = interactScript;
                    nearestInteractableIndex = System.Array.IndexOf(interactScript.InteractionPoints, interactionPoint);
                }
            }
        }
        
        if (nearestInteractable != null)
        {
            if (lastFrameInteractable != nearestInteractable || lastFrameInteractionPoint != nearestInteractableIndex)
            {
                if (lastFrameInteractable != null)
                {
                    lastFrameInteractable.HideInteractionPoint(lastFrameInteractionPoint);
                }

                // display the nearest interaction point
                nearestInteractable.DisplayInteractionPoint(nearestInteractableIndex);

                lastFrameInteractable = nearestInteractable;
                lastFrameInteractionPoint = nearestInteractableIndex;
            }
        }
        else if (lastFrameInteractable != null)
        {
            lastFrameInteractable.HideInteractionPoint(lastFrameInteractionPoint);
            lastFrameInteractable = null;
            lastFrameInteractionPoint = -1;
        }
        
        // if the player presses the interact button
        if (Input.GetKeyDown(KeyCode.Space) && lastFrameInteractable != null)
        {
            // interact with the nearest interaction point
            lastFrameInteractable.Interact(lastFrameInteractionPoint);
        }
    }
}
