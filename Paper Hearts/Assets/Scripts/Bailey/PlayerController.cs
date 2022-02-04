using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // fields
    Rigidbody2D rb;
    BoxCollider2D box;
    Transform attackHb;

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
    float attackSwingSpeed = 300;
    private bool lastMovedRight = true;
    private bool attackStarted = false;
    private bool attackSwingingRight = false;

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
        attackHb = this.transform.Find("Attack");
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
                // start the cycle, first move the hitbox to position
                attackHb.GetComponent<SpriteRenderer>().enabled = true;
                attackHb.GetComponent<CapsuleCollider2D>().enabled = true;

                switch (lastMovedRight)
                {
                    case true:
                        attackHb.transform.localPosition = new Vector3(0.8f, 0f, 0f);
                        attackHb.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                        attackSwingingRight = false;
                        break;
                    case false:
                        attackHb.transform.localPosition = new Vector3(-0.8f, 0f, 0f);
                        attackHb.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
                        attackSwingingRight = true;
                        break;
                }
                attackStarted = true;
            }
            {
                Vector3 point = this.transform.position;
                Vector3 axis = new Vector3(0, 0, 1);

                // loop through attacking
             
                switch (attackSwingingRight)
                {
                    case true:
                        axis = new Vector3(0, 0, -1);
                        attackHb.RotateAround(point, axis, Time.deltaTime * attackSwingSpeed);
                        if (attackHb.localEulerAngles.z <= 90)
                        {
                            attackSwingingRight = false;
                        }
                        break;
                    case false:
                        axis = new Vector3(0, 0, 1);
                        attackHb.RotateAround(point, axis, Time.deltaTime * attackSwingSpeed);
                        if (attackHb.eulerAngles.z >= 270)
                        {
                            attackSwingingRight = true;
                        }
                        break;
                }

                // FOR HEART PHYSICS, APPLY FORCE TO THE NORMAL
            }
        }
        else // clean up attacking
        {
            attackHb.GetComponent<SpriteRenderer>().enabled = false;
            attackHb.GetComponent<CapsuleCollider2D>().enabled = false;
            attackStarted = false;
            attackSwingingRight = false;
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
