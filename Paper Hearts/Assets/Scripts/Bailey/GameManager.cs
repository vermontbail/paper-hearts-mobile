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
    private float screenMidWidth;
    private float screenSwipeHeight;
    private Touch firstTouch;
    private Dictionary<int, float> initialTouchYValues;
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


        screenMidWidth = (float)Screen.width / 2.0f;
        screenSwipeHeight = (float)Screen.height / 8.0f;

        initialTouchYValues = new Dictionary<int, float>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (runGame)
        {
            // check keyboard input
            KeyboardInput();
            // check mobile input
            MobileInput();

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
    private void MobileInput()
    {
        // get touches
        Touch[] touchArray = Input.touches;
        
        if (touchArray.Length == 1 && !player.IsSliding) // one finger
        {
            if (touchArray[0].position.x >= screenMidWidth)
            {
                player.MoveRight();
            }
            else
            {
                player.MoveLeft();
            }
            firstTouch = touchArray[0];
        }
        if (touchArray.Length >= 2)
        {
            if (!player.IsSliding)
            {
                player.isAttacking = true;
            }

            foreach (Touch t in touchArray)
            {
                // not counting first finger
                if (t.fingerId != firstTouch.fingerId)
                {
                    // log to see we know the first place it's been
                    if (t.phase == TouchPhase.Began)
                    {
                        if (initialTouchYValues.ContainsKey(t.fingerId))
                        {
                            initialTouchYValues[t.fingerId] = t.position.y;
                        }
                        else
                        {
                            initialTouchYValues.Add(t.fingerId, t.position.y);
                        }
                    }

                    // check for slide with opposite finger
                    if ((initialTouchYValues[t.fingerId] - t.position.y) >= screenSwipeHeight)
                    {
                        player.IsSliding = true;
                        player.isAttacking = false;
                        initialTouchYValues[t.fingerId] = t.position.y;
                    }
                    if ((initialTouchYValues[t.fingerId] - t.position.y) <= -screenSwipeHeight && player.IsSliding)
                    {
                        player.SlideAttack();
                        initialTouchYValues[t.fingerId] = t.position.y;
                    }

                    // remove when finger lifted
                    if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                    {
                        initialTouchYValues.Remove(t.fingerId);
                    }

                }
            }
            
        }
    }
}
