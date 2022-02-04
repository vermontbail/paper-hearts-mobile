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
        string levelString = "Level: " + PlayerPrefs.GetInt("currentLevel", -1);
        level.text = levelString;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spacebar pressed");
            if (PlayerPrefs.GetInt("currentLevel") != 12) //This 12 should be an enum in the future - JH
            {
                PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel", -1) + 1);
                if (PlayerPrefs.GetInt("currentLevel") == 0)
                {
                    PlayerPrefs.SetInt("currentLevel", 1); //Level 0 is reserved for the tutorial. Level 1 is level 1 (wow!)
                }
                if (PlayerPrefs.GetInt("currentLevel") - 1 > PlayerPrefs.GetInt("highestLevel", -1) || PlayerPrefs.GetInt("currentLevel") != 1) //Update highest level if needed.
                {
                    PlayerPrefs.SetInt("highestLevel", PlayerPrefs.GetInt("currentLevel"));
                }
                PlayerPrefs.SetInt("levelChanged", 1);
            }
        }
        if (PlayerPrefs.GetInt("levelChanged", 0) != 0)
        {
            PlayerPrefs.SetInt("levelChanged", 0);
            level.text = "Level: " + PlayerPrefs.GetInt("currentLevel");
        }
    }
}
