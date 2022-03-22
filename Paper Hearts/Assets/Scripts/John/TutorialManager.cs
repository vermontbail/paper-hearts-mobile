using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TextMeshProUGUI prompt;
    public static TutorialState tutState;
    public static GameObject blockParent;

    //Private vars
    static float timer;
    static bool timerActive = false;
    static GameObject heart;
    static GameObject tutCircle;
    public enum TutorialState
    {
        /*
        welcome = 0,
        moveLeft = 1,
        moveRight = 2,
        learnSwing = 3,
        learnSlide = 4,
        learnKick = 5,
        heart1 = 6,
        heart2 = 7,
        blocks = 8,
        powerups = 9,
        powerups2 = 10,
        complete = 11
        */
        welcome,
        moveLeft,
        moveRight,
        learnSwing,
        learnSlide,
        learnKick,
        heart1,
        heart2,
        blocks,
        powerups,
        powerups2,
        complete
    }
    // Start is called before the first frame update
    void Start()
    {
        prompt = GameObject.Find("Prompt").GetComponent<TextMeshProUGUI>();
        tutCircle = GameObject.Find("tutCircle");
        tutCircle.SetActive(false);
        prompt.text = "Welcome to Paper Hearts!";
        ResetTimer(120f);
        tutState = TutorialState.welcome;
        heart = GameObject.Find("Heart");
        heart.SetActive(false);
        blockParent = GameObject.Find("Scoreblocks");
        blockParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(timerActive)
        {
            timer--;
            if(timer <= 0)
            {
                timerActive = false;
                AdvanceTutorial();
                GameManager.toggleTime();
                if (tutCircle.activeInHierarchy)
                {
                    tutCircle.SetActive(false);
                }
            }
        }
    }
    //When resetting the time, reset it for enough time (in frames (60 frames = 1 second)) for the player to read the text 3 times.
    private static void ResetTimer(float readTime)
    {
        timerActive = true;
        timer = readTime * 0; //Change back to 3 when done debugging.
        GameManager.toggleTime();
    }

    private static void Circle(Vector2 pos)
    {
        Vector3 circlePos = new Vector3(pos.x, pos.y, tutCircle.transform.position.z); 
        tutCircle.transform.position = circlePos;
        tutCircle.SetActive(true);
    }

    public static void AdvanceTutorial()
    {
        timerActive = false;
        switch(tutState)
        {
            case TutorialState.welcome:
                prompt.text = "Press on the left side of the screen to move left.";
                tutState = TutorialState.moveLeft;
                break;
            case TutorialState.moveLeft:
                prompt.text = "Press on the right side of the screen to move right.";
                tutState = TutorialState.moveRight;
                break;
            case TutorialState.moveRight:
                prompt.text = "While moving, hold on the opposite side of the screen to swing.";
                tutState = TutorialState.learnSwing;
                break;
            case TutorialState.learnSwing:
                prompt.text = "While moving, swipe down with your free finger to slide.";
                tutState = TutorialState.learnSlide;
                break;
            case TutorialState.learnSlide:
                prompt.text = "While sliding, swipe up with your free finger to kick.";
                tutState = TutorialState.learnKick;
                break;
            case TutorialState.learnKick:
                heart.SetActive(true);
                Circle(heart.transform.position);
                prompt.text = "This is the heart. Use it to break blocks.";
                ResetTimer(105f);
                tutState = TutorialState.heart1;
                break;
            case TutorialState.heart1:
                prompt.text = "You can launch the heart by swinging or kicking.";
                ResetTimer(120f);
                timerActive = true;
                tutState = TutorialState.heart2;
                break;
            case TutorialState.heart2:
                prompt.text = "Use the heart to break these two blocks, and activate the panel in the middle.";
                blockParent.SetActive(true);
                for(int a = 0; a < blockParent.transform.childCount; a++)
                {
                    blockParent.transform.GetChild(a).gameObject.SetActive(true);
                }
                tutState = TutorialState.blocks;
                break;
            case TutorialState.blocks:
                prompt.text = "Congratulations! One more thing...";
                ResetTimer(90f);
                tutState = TutorialState.powerups;
                break;
            case TutorialState.powerups:
                prompt.text = "This is a powerup. Different powerups have different abilities. They will drop from blocks.";
                ResetTimer(240f);
                tutState = TutorialState.complete;
                break;
            case TutorialState.complete:
                break;
            default:
                Debug.Log("Tutorial advanced without advance case. Enum: " + tutState);
                break;
        }
    }
}
