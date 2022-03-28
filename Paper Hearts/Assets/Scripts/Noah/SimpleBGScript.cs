using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleBGScript : MonoBehaviour
{
    Canvas myCanvas;
    // Start is called before the first frame update
    void Awake()
    {
        myCanvas = gameObject.GetComponent<Canvas>();
        myCanvas.sortingLayerName = "Background";
        Debug.Log(myCanvas.sortingLayerName);
    }

    // Update is called once per frame
    void Update()
    {
        myCanvas.sortingLayerName = "Background";
    }
}
