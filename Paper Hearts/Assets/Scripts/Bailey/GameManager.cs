using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
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
