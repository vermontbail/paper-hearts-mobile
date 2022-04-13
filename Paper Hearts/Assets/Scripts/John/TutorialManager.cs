using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TextMeshProUGUI prompt;
    public static TutorialState tutState;
    public static GameObject blockParent;
    public static TextMeshProUGUI leftPrompt;
    public static TextMeshProUGUI rightPrompt;
    public static GameObject enemy;
    private static PowerUpBlock pub;

    //Private vars
    static float timer;
    static bool timerActive = false;
    static GameObject heart;
    static GameObject tutCircle;
    static ToNewScene sceneManager;
    public enum TutorialState
    {
        welcome,
        moveLeft,
        moveRight,
        learnSwing,
        learnSlide,
        learnKick,
        damage,
        heart1,
        heart2,
        blocks,
        blocks2,
        enemy,
        powerups,
        powerups2,
        complete
    }
    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<ToNewScene>();
        prompt = GameObject.Find("PromptMiddle").GetComponent<TextMeshProUGUI>();
        leftPrompt = GameObject.Find("PromptLeft").GetComponent<TextMeshProUGUI>();
        rightPrompt = GameObject.Find("PromptRight").GetComponent<TextMeshProUGUI>();
        pub = GameObject.Find("SplitBlock").GetComponent<PowerUpBlock>();
        pub.gameObject.SetActive(false);
        enemy = GameObject.Find("FullEnemy");
        enemy.SetActive(false);
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
        timer = readTime * 10f; 
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
                prompt.text = "";
                rightPrompt.text = "This is the heart. Use it to break blocks.";
                ResetTimer(105f);
                tutState = TutorialState.damage;
                break;
            case TutorialState.damage:
                rightPrompt.text = "The heart is not yours to keep, so you will take damage if it hits you.";
                ResetTimer(150f);
                tutState = TutorialState.heart1;
                break;
            case TutorialState.heart1:
                rightPrompt.text = "You can launch the heart by swinging or kicking.";
                ResetTimer(120f);
                timerActive = true;
                tutState = TutorialState.heart2;
                break;
            case TutorialState.heart2:
                rightPrompt.text = "Use the heart to break these two blocks, and activate the panel in the middle.";
                blockParent.SetActive(true);
                for(int a = 0; a < blockParent.transform.childCount; a++)
                {
                    blockParent.transform.GetChild(a).gameObject.SetActive(true);
                }
                // fix 
                GameObject.FindObjectOfType<GameManager>().totalScore = 3;
                tutState = TutorialState.blocks;
                break;
            case TutorialState.blocks:
                rightPrompt.text = "";
                prompt.text = "Well done!";
                ResetTimer(90f);
                tutState = TutorialState.blocks2;
                break;
            case TutorialState.blocks2:
                Circle(enemy.transform.position);
                prompt.text = "";
                leftPrompt.text = "This is an enemy. Hit them with the heart to defeat them.";
                GameObject.FindObjectOfType<GameManager>().totalScore = 5;
                enemy.SetActive(true);
                tutState = TutorialState.enemy;
                break;
            case TutorialState.enemy:
                Circle(pub.transform.position);
                pub.gameObject.SetActive(true);
                pub.CreatePowerUp();
                leftPrompt.text = "This is a powerup. Different powerups have different abilities. They will drop from blocks.";
                ResetTimer(240f);
                tutState = TutorialState.powerups;
                break;
            case TutorialState.powerups:
                leftPrompt.text = "Powerups drop from colored blocks. We won't go deep into them here, but keep a lookout. Some powerups only activate when you attack, or kick.";
                ResetTimer(300f);
                tutState = TutorialState.powerups2;
                break;
            case TutorialState.powerups2:
                leftPrompt.text = "";
                prompt.text = "Congratulations! You've completed the tutorial. You're all set for Paper Hearts.";
                ResetTimer(210f);
                //Go to the first level here.
                tutState = TutorialState.complete;
                break;
            case TutorialState.complete:
                sceneManager.LoadLevel1();
                break;
            default:
                Debug.Log("Tutorial advanced without advance case. Enum: " + tutState);
                break;
        }
    }
}
