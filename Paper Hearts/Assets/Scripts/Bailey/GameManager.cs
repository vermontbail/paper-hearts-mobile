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
        if(!GameObject.Find("TutorialManager"))
        {
            TutorialManager.tutState = TutorialManager.TutorialState.complete;
        }
        // get number of score
        try
        {
            totalScore = GameObject.Find("Scoreblocks").transform.childCount;
        }
        catch (NullReferenceException)
        {
            Debug.Log("Scoreblocks not found. Using number of scoreblocks from tutorial...");
            totalScore = TutorialManager.blockParent.transform.childCount; //This error should only occur in the tutorial due to level creation order.
            Debug.Log("totalScore: " + totalScore);
        }


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
            if(TutorialManager.tutState == TutorialManager.TutorialState.blocks)
            {
                TutorialManager.AdvanceTutorial();
            }
            LevelComplete();
        }
    }
    public void LevelComplete()
    {
        hasWon = true;
        // anything else here
    }
    public static void toggleTime()
    {
        if(Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
    private void KeyboardInput()
    {

        // check for attack, set to false if isnt
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) && !player.IsSliding)
        {
            if (TutorialManager.tutState == TutorialManager.TutorialState.learnSwing)
            {
                TutorialManager.AdvanceTutorial();
            }
            player.isAttacking = true;
        }
        else player.isAttacking = false;

        // check for slide
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (TutorialManager.tutState > TutorialManager.TutorialState.learnSwing)
            {
                if (TutorialManager.tutState == TutorialManager.TutorialState.learnSlide)
                {
                    TutorialManager.AdvanceTutorial();
                }
                player.IsSliding = true;
                player.isAttacking = false;
            }
        }
        // check for slide attack
        if (Input.GetKeyDown(KeyCode.UpArrow) && player.IsSliding)
        {
            if (TutorialManager.tutState > TutorialManager.TutorialState.learnSlide)
            {
                player.SlideAttack();
                if (TutorialManager.tutState == TutorialManager.TutorialState.learnKick)
                {
                    TutorialManager.AdvanceTutorial();
                }
            }
        }

        // check for normal movement
        if (!player.isAttacking && !player.IsSliding)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (TutorialManager.tutState > TutorialManager.TutorialState.moveLeft)
                {
                    if (TutorialManager.tutState == TutorialManager.TutorialState.moveRight)
                    {
                        TutorialManager.AdvanceTutorial();
                    }
                    player.MoveRight();
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (TutorialManager.tutState > 0)
                {
                    if (TutorialManager.tutState == TutorialManager.TutorialState.moveLeft)
                    {
                        TutorialManager.AdvanceTutorial();
                    }
                    player.MoveLeft();
                }
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
                if(TutorialManager.tutState > TutorialManager.TutorialState.moveLeft)
                {
                    if(TutorialManager.tutState == TutorialManager.TutorialState.moveRight)
                    {
                        TutorialManager.AdvanceTutorial();
                    }
                    player.MoveRight();
                }
                
            }
            else
            {
                if(TutorialManager.tutState > 0)
                {
                    if(TutorialManager.tutState == TutorialManager.TutorialState.moveLeft)
                    {
                        TutorialManager.AdvanceTutorial();
                    }
                    player.MoveLeft();
                }
                
            }
            firstTouch = touchArray[0];
        }
        if (touchArray.Length >= 2)
        {
            if (!player.IsSliding)
            {
                if(TutorialManager.tutState > TutorialManager.TutorialState.moveRight)
                {
                    if (TutorialManager.tutState == TutorialManager.TutorialState.learnSwing)
                    {
                        TutorialManager.AdvanceTutorial();
                    }
                    player.isAttacking = true;
                }
                
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
                        if(TutorialManager.tutState > TutorialManager.TutorialState.learnSwing)
                        {
                            if(TutorialManager.tutState == TutorialManager.TutorialState.learnSlide)
                            {
                                TutorialManager.AdvanceTutorial();
                            }
                            player.IsSliding = true;
                            player.isAttacking = false;
                            initialTouchYValues[t.fingerId] = t.position.y;
                        }
                    }
                    if ((initialTouchYValues[t.fingerId] - t.position.y) <= -screenSwipeHeight && player.IsSliding)
                    {
                        if(TutorialManager.tutState > TutorialManager.TutorialState.learnSlide)
                        {
                            player.SlideAttack();
                            initialTouchYValues[t.fingerId] = t.position.y;
                            if(TutorialManager.tutState == TutorialManager.TutorialState.learnKick)
                            {
                                TutorialManager.AdvanceTutorial();
                            }
                        } 
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
