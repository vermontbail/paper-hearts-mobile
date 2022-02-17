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
        if (PlayerPrefs.GetInt(GameManager.LEVEL_CHANGE, 0) != 0)
        {
            PlayerPrefs.SetInt(GameManager.LEVEL_CHANGE, 0);
            level.text = "Level: " + PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL);
        }
    }
}
