using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI prompt;

    //Private vars
    float timer;
    bool advanceTutorial = false;
    enum tutorialState
    {
        welcome,

    }
    // Start is called before the first frame update
    void Start()
    {
        prompt.text = "Welcome to Paper Hearts!";
        ResetTimer(2f);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
    }
    //When resetting the time, reset it for enough time (in seconds) for the player to read the text 3 times.
    void ResetTimer(float readTime)
    {
        timer = readTime * 3;
    }
}
