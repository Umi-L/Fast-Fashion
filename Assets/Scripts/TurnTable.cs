using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTable : MonoBehaviour
{
    [SerializeField] public GameObject[] Objects;
    [SerializeField] public string[] Descriptions;
    [SerializeField] public TMPro.TextMeshProUGUI DescriptionText;
    
    private int currentIndex = 0;

    private void Start()
    {
        if (Descriptions.Length <= 0)
        {
            return;
        }
        
        DescriptionText.text = Descriptions[currentIndex];

        Instantiate(Objects[currentIndex], transform);
    }
    

    public void Next()
    {
        Destroy(transform.GetChild(0).gameObject);
        currentIndex++;
        
        if (currentIndex >= Objects.Length)
        {
            currentIndex = 0;
        }
        
        DescriptionText.text= Descriptions[currentIndex];
        
        Instantiate(Objects[currentIndex], transform);
    }
}
