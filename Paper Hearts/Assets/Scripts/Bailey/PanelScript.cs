using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScript : MonoBehaviour
{
    // Start is called before the first frame update
    private bool flipped = false;

    public bool Flipped
    {
        get { return flipped;}
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Flip()
    {
        if (!flipped)
        {
            flipped = true;
            // play animation
            // placeholder color switch for now
            GetComponent<SpriteRenderer>().color = new Color(207f, 79f, 77f);
        }
    }
}
