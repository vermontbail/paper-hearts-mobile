using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // fields
    Rigidbody2D rb;
    BoxCollider2D box;
    Transform attackHb;
    Transform slideHb;
    Transform slideAttackHb;

    // externals
    public bool isAttacking = false;

    public bool IsSliding
    {
        get { return isSliding; }
        set
        {
            if (slideCooldown <= 0f && value == true)
            {
                // enable sliding hitbox
                slideHb.GetComponent<BoxCollider2D>().enabled = true;
                slideHb.GetComponent<SpriteRenderer>().enabled = true;
                // set hitbox position
                if (lastMovedRight)
                {
                    slideHb.transform.localPosition = new Vector2(slideHitboxDisplacement.x, slideHitboxDisplacement.y);
                }
                else
                {
                    slideHb.transform.localPosition = new Vector2(-slideHitboxDisplacement.x, slideHitboxDisplacement.y);
                }

                 isSliding = value;
            }
        }
    }
    public bool AttackSwingingRight
    {
        get { return attackSwingingRight; }
    }
    
    // NOTE: SET IS SLIDING PROPERTY TO ENFORCE COOLDOWN

    // internals
    float movingSpeed = 4.0f;
    float slidingSpeed = 10.0f;
    float attackSwingSpeed = 400;
    private bool lastMovedRight = true;
    private bool attackStarted = false;
    private bool attackSwingingRight = false;

    // sliding
    private bool isSliding = false;
    private float slideTimer = 0f;
    private float slideDuration = 0.75f;
    private float slideAttackDuration = 0.5f;
    private float slideAttackTimer = 0f;
    private float slideCooldown = 0f;
    private bool slideAttacking = false;
    private Vector2 slideHitboxDisplacement = new Vector2(0.5f, 0f);
    private Vector2 slideAttackHitboxDisplacement = new Vector2(0.30f, 0.15f);
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        attackHb = this.transform.Find("Attack");
        slideHb = this.transform.Find("Slide");
        slideAttackHb = this.transform.Find("SlideAttack");
    }

    // Update is called once per frame
    void FixedUpdate()
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
        // first check for the slide attack override
        if (slideAttacking && slideTimer < slideAttackDuration)
        {

            // change slide attack transform
            slideAttackHb.transform.localScale = new Vector2(slideTimer / -(2 * slideAttackDuration) + 1f, slideTimer/(2*slideAttackDuration) + 1f);

            if (lastMovedRight)
            {
                MoveRight();
            }
            else MoveLeft();

            // at the end, increment counter
            slideTimer += Time.deltaTime;
            // if this counter incremented over, start cooldown
            if (slideTimer >= slideAttackDuration)
            {
                slideCooldown = 0.75f;
            }
        }
        else if (isSliding && slideTimer < slideDuration)
        {

            // movement
            switch (lastMovedRight)
            {
                case true:
                    if (!slideAttacking)
                    {
                        rb.velocity = new Vector2((((-1.5f/slideDuration * slideTimer) + 1.5f) * slidingSpeed), 0.0f);
                        
                    }
                    else { MoveRight(); }
                    break;

                case false:
                    if (!slideAttacking)
                    {
                        rb.velocity = new Vector2((((-1.5f / slideDuration * slideTimer) + 1.5f) * -slidingSpeed), 0.0f);
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

            // move slide and slide attack hitbox away
            slideHb.GetComponent<BoxCollider2D>().enabled = false;
            slideHb.GetComponent<SpriteRenderer>().enabled = false;

            slideAttackHb.GetComponent<SpriteRenderer>().enabled = false;
            slideAttackHb.GetComponent<BoxCollider2D>().enabled = false;
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
            slideTimer = 0f;

            slideAttacking = true;
            // enable slide attack hitbox
            slideAttackHb.GetComponent<SpriteRenderer>().enabled = true;
            slideAttackHb.GetComponent<BoxCollider2D>().enabled = true;

            // move into postion
            if (lastMovedRight)
            {
                slideAttackHb.transform.localPosition = new Vector2(slideAttackHitboxDisplacement.x, slideAttackHitboxDisplacement.y);
            }
            else
            {
                slideAttackHb.transform.localPosition = new Vector2(-slideAttackHitboxDisplacement.x, slideAttackHitboxDisplacement.y);
            }

            // disable slide hitbox
            slideHb.GetComponent<SpriteRenderer>().enabled = false;
            slideHb.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
