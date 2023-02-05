using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeTemplate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void SetItem(Items.CraftingItem item, int count)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = Items.GetItemIcon(item);
        transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = count.ToString();
    }
}
