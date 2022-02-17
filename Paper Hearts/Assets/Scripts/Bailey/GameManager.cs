using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController player;
    private int totalScore = 0;
    private int currentScore = 0;
    private int highestLevel = 12; //Somewhat a magic number, change this if level numbers change.
    private bool hasWon = false;
    private bool runGame = true;
    private float countdownTimeThird = .5f; //Make the time 1/3 of how long, in seconds, you want the player to wait before playing.
    private float countdown = 0f;
    private int countdownCounter = 0;
    private TextMeshProUGUI promptText;
    //Consts
    public enum levels
    {
        NoLevel = -1,
        Tutorial = 0,
        LvlOne = 1,
        LvlTwo = 2,
        LvlThree = 3,
        LvlFour = 4,
        LvlFive = 5,
        BossOne = 6,
        LvlSix = 7,
        LvlSeven = 8,
        LvlEight = 9,
        LvlNine = 10,
        LvlTen = 11,
        BossTwo = 12,
    }
    public static string CURRENT_LEVEL = "currentLevel";
    public static string HIGHEST_LEVEL = "highestLevel";
    public static string LEVEL_CHANGE = "levelChanged";
    public static string GAMEPLAY_SCENE = "Gameplay"; //Name of the scene where gameplay takes place.
    //Public gameplay variables
    public static bool gameplayStarting = false;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        // get number of score
        totalScore = GameObject.Find("Scoreblocks").transform.childCount;

    }

    // Update is called once per frame
    void Update()
    {
        if(gameplayStarting)
        {
            gameplayStarting = false;
            runGame = false;
            promptText = GetComponent<TextMeshProUGUI>();
            promptText.text = "3";
            countdown = countdownTimeThird;
            countdownCounter = 0;
        }
        if (runGame)
        {
            // check for attack, set to false if isnt
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) && !player.IsSliding)
            {
                player.isAttacking = true;
            }
            else player.isAttacking = false;

            // check for slide
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                player.IsSliding = true;
                player.isAttacking = false;
            }
            // check for slide attack
            if (Input.GetKeyDown(KeyCode.UpArrow) && player.IsSliding)
            {
                player.SlideAttack();
            }

            // check for normal movement
            if (!player.isAttacking && !player.IsSliding)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    player.MoveRight();
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    player.MoveLeft();
                }
            }

            // placeholder text
            if (hasWon)
            {
                GameObject.Find("Placeholder Next").transform.position = new Vector2(0f, 0f);
            }

            //***************************DEBUG*********************
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameplayStarting = true;
                if (PlayerPrefs.GetInt(CURRENT_LEVEL) != highestLevel)
                {
                    PlayerPrefs.SetInt(CURRENT_LEVEL, PlayerPrefs.GetInt(CURRENT_LEVEL, -1) + 1);
                    if (PlayerPrefs.GetInt(CURRENT_LEVEL) == (int)levels.Tutorial)
                    {
                        PlayerPrefs.SetInt(CURRENT_LEVEL, (int)levels.LvlOne);
                    }
                    if (PlayerPrefs.GetInt(CURRENT_LEVEL) - 1 > PlayerPrefs.GetInt(HIGHEST_LEVEL, (int)levels.NoLevel) || PlayerPrefs.GetInt(CURRENT_LEVEL) != (int)levels.LvlOne) //Update highest level if needed.
                    {
                        PlayerPrefs.SetInt(HIGHEST_LEVEL, PlayerPrefs.GetInt(CURRENT_LEVEL));
                    }
                    PlayerPrefs.SetInt(LEVEL_CHANGE, 1);
                }
            }
        }
        else
        {
            countdown -= Time.deltaTime;
        }
        if(!runGame && countdown <= 0f)
        {
            countdown = countdownTimeThird;
            countdownCounter++;
            if(countdownCounter < 3)
            {
                promptText.text = (3 - countdownCounter).ToString();
            }
            else
            {
                runGame = true;
                promptText.text = "Press spacebar to advance level.";
            }
        }
    }
    public void AddScore()
    {
        currentScore++;
        if (currentScore >= totalScore)
        {
            LevelComplete();
        }
    }
    public void LevelComplete()
    {
        hasWon = true;
        // anything else here
    }
}
