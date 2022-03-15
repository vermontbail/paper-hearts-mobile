using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LevelManager : MonoBehaviour
{
    //References
    public TextMeshProUGUI level;
    public TextMeshProUGUI prompt;
    //Private vars
    private int countdownCounter = 0;
    private float countdownTimeThird = 30f; //Make the time 1/3 of how long, in frames, you want the player to wait before playing.
    private float countdown = 0f;

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
        if (GameManager.gameplayStarting)
        {
            GameManager.toggleTime();
            GameManager.gameplayStarting = false;
            GameManager.runGame = false;
            prompt.text = "3";
            countdown = countdownTimeThird;
            countdownCounter = 0;
        }
        if (!GameManager.runGame || countdownCounter == 3)
        {
            countdown--;
        }
        if (countdownCounter < 4 && countdown <= 0f)
        {
            countdown = countdownTimeThird;
            countdownCounter++;
            if (countdownCounter < 3)
            {
                prompt.text = (3 - countdownCounter).ToString();
            }
            else if(countdownCounter == 3)
            {
                GameManager.runGame = true;
                prompt.text = "GO!";
                GameManager.toggleTime();
            }
            else
            {
                prompt.text = "Press spacebar to advance level.";
            }
        }
        //***************************DEBUG*********************
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.runGame)
        {
            GameManager.gameplayStarting = true;
            if (PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL) != GameManager.highestLevel)
            {
                PlayerPrefs.SetInt(GameManager.CURRENT_LEVEL, PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL, -1) + 1);
                if (PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL) == (int)GameManager.levels.Tutorial)
                {
                    PlayerPrefs.SetInt(GameManager.CURRENT_LEVEL, (int)GameManager.levels.LvlOne);
                }
                if (PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL) - 1 > PlayerPrefs.GetInt(GameManager.HIGHEST_LEVEL, (int)GameManager.levels.NoLevel) || 
                    PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL) != (int)GameManager.levels.LvlOne) //Update highest level if needed.
                {
                    PlayerPrefs.SetInt(GameManager.HIGHEST_LEVEL, PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL));
                }
                PlayerPrefs.SetInt(GameManager.LEVEL_CHANGE, 1);
            }
        }

        if (PlayerPrefs.GetInt(GameManager.LEVEL_CHANGE, 0) != 0)
        {
            PlayerPrefs.SetInt(GameManager.LEVEL_CHANGE, 0);
            level.text = "Level: " + PlayerPrefs.GetInt(GameManager.CURRENT_LEVEL);
        }
    }
}
