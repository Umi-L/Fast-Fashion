using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingReload : MonoBehaviour
{
    // Start is called before the first frame update
    public PostProcessVolume V;  
    void Start(){
        V.enabled = false;
        V.enabled = true;
    }
}
