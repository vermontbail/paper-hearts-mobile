using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTextUpdator : MonoBehaviour
{
    public TextMeshProUGUI level;
    // Start is called before the first frame update
    void Start()
    {
        //***********DEBUG**********
        if (level)
        {
            Debug.Log("Found score text");
        }
        else
        {
            Debug.Log("Did not find score text.");
        }
        //End DEBUG
        string levelString = "Level: " + PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL, -1);
        level.text = levelString;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spacebar pressed");
            if (PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL) != 12) //This 12 should be an enum in the future - JH
            {
                PlayerPrefs.SetInt(GameManager.CURRENT_LEVEL, PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL, -1) + 1);
                if (PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL) == 0)
                {
                    PlayerPrefs.SetInt(GameManager.CURRENT_LEVEL, 1); //Level 0 is reserved for the tutorial. Level 1 is level 1 (wow!)
                }
                if (PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL) - 1 > PlayerPrefs.GetInt(GameManager.HIGHEST_LEVEL, -1) || PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL) != 1) //Update highest level if needed.
                {
                    PlayerPrefs.SetInt(GameManager.HIGHEST_LEVEL, PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL));
                }
                PlayerPrefs.SetInt("levelChanged", 1);
            }
        }
        if (PlayerPrefs.GetInt("levelChanged", 0) != 0)
        {
            PlayerPrefs.SetInt("levelChanged", 0);
            level.text = "Level: " + PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL);
        }
    }
}
