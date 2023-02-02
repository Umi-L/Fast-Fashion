using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    
    public Transform[] InteractionPoints;

    
    public void HideAllInteractionPoints()
    {
        for (int i = 0; i < InteractionPoints.Length; i++)
        {
            HideInteractionPoint(i);
        }
    }
    
    public virtual void Interact(int point)
    {
        Debug.Log("Interacting with " + gameObject.name + " at point " + point);
    }
    
    public virtual void DisplayInteractionPoint(int index)
    {
        //Debug.Log("Displaying interaction point " + index);
    }
    
    public virtual void HideInteractionPoint(int index)
    {
        //Debug.Log("Hiding interaction point " + index);
    }
}
