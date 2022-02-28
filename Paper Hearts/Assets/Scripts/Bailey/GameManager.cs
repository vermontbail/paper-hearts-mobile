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
    private bool hasWon = false;
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
    public static bool runGame = true;
    public static int highestLevel = 12; //Somewhat a magic number, change this if level numbers change.
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        // get number of score
        totalScore = GameObject.Find("Scoreblocks").transform.childCount;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (runGame)
        {
            // check keyboard input
            KeyboardInput();
            // check 

            // placeholder text
            if (hasWon)
            {
                GameObject.Find("Placeholder Next").transform.position = new Vector2(0f, 0f);
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
    private void KeyboardInput()
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
    }
}
