using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LevelTextUpdator : MonoBehaviour
{
    public TextMeshProUGUI level;
    private int finalLevel;
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
        Array levelValues = Enum.GetValues(typeof(GameManager.levels));
        finalLevel = (int)levelValues.GetValue(levelValues.Length - 1);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spacebar pressed");
            if (PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL) != finalLevel)
            {
                PlayerPrefs.SetInt(GameManager.CURRENT_LEVEL, PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL, -1) + 1);
                if (PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL) == (int)GameManager.levels.Tutorial)
                {
                    PlayerPrefs.SetInt(GameManager.CURRENT_LEVEL, (int)GameManager.levels.LvlOne);
                }
                if (PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL) - 1 > PlayerPrefs.GetInt(GameManager.HIGHEST_LEVEL, (int)GameManager.levels.NoLevel) || PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL) != (int)GameManager.levels.LvlOne) //Update highest level if needed.
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
