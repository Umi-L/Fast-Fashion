using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public int levelIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        var button = transform.GetChild(0);
        button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            //unity load level index
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
        });
        
        button.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "level" + levelIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
