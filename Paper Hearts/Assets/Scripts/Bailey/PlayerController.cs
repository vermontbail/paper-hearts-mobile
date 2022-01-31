using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // fields
    Rigidbody2D rb;
    BoxCollider2D box;

    // externals
    public bool isAttacking = false;

    public bool IsSliding
    {
        get { return isSliding; }
        set
        {
            if (slideCooldown <= 0f)
            {
                isSliding = value;
            }
        }
    }
    
    // NOTE: SET IS SLIDING PROPERTY TO ENFORCE COOLDOWN

    // internals
    float movingSpeed = 4.0f;
    float slidingSpeed = 8.0f;
    private bool lastMovedRight = true;
    private bool attackStarted = false;
    private float attackTimer = 0f;

    // sliding
    private bool isSliding = false;
    private float slideTimer = 0f;
    private float slideDuration = 0.75f;
    private float slideAttackDuration = 0.75f;
    private float slideAttackTimer = 0f;
    private float slideCooldown = 0f;
    private bool slideAttacking = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // TESTING COLORS, REMOVE WHEN WE GET ART
        if (slideAttacking)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if (isSliding)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (isAttacking)
        {
            GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else { GetComponent<SpriteRenderer>().color = Color.white; }



        // attacking functionality, game manager handles "un-attacking"
        if (isAttacking)
        {
            // check for first side
            if (!attackStarted)
            {
                // start the cycle

                // don't do this after its over
                attackStarted = true;
            }
            else
            {
                // loop through attacking
            }
        }
        else // clean up attacking
        {

        }
        // sliding functionality, this script handles "un-sliding"
        if (isSliding && slideTimer < slideDuration)
        {
            // check for attack
            if (slideAttacking)
            {
                // use the timer as a way to move hitbox and create animation


            }

            switch (lastMovedRight)
            {
                case true:
                    if (!slideAttacking)
                    {
                        rb.velocity = new Vector2(slidingSpeed, 0.0f);
                    }
                    else { MoveRight(); }
                    break;

                case false:
                    if (!slideAttacking)
                    {
                        rb.velocity = new Vector2(-slidingSpeed, 0.0f);
                    }
                    else { MoveLeft(); }
                    break;
            }

            // at the end, increment counter
            slideTimer += Time.deltaTime;
            // if this counter incremented over, start cooldown
            if (slideTimer >= slideDuration)
            {
                slideCooldown = 0.75f;
            }
        }
        else // reset
        { 
            isSliding = false;
            slideAttacking = false;
            slideAttackTimer = 0f;
            slideTimer = 0f;
            // move slide attack hitbox away
        }

        // decrement player cooldowns
        if (slideCooldown > 0)
        {
            slideCooldown -= Time.deltaTime;
        }
    }
    // Movement
    public void MoveRight()
    {
        rb.velocity = new Vector2(movingSpeed, 0.0f);
        lastMovedRight = true;
    }
    public void MoveLeft()
    {
        rb.velocity = new Vector2(-movingSpeed, 0.0f);
        lastMovedRight = false;
    }

    public void SlideAttack()
    {
        if (isSliding)
        {
            // change time remaining on slide directly
            slideDuration = slideAttackDuration;
            slideAttacking = true;
        }
    }
}
