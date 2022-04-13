using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
            if (slideCooldown <= 0f && !slideAttacking && value == true && currentStun <= 0f)
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
    public PowerUp PowerUp
    {
        get { return currentPowerUp; }
    }

    // NOTE: SET IS SLIDING PROPERTY TO ENFORCE COOLDOWN

    // internals
    HeartScript heart;

    const float movingSpeed = 4.0f;
    const float slidingSpeed = 10.0f;
    const float baseAttackSwingSpeed = 400;
    float attackSwingSpeed = 0;
    private bool lastMovedRight = true;
    private bool attackStarted = false;
    private bool attackSwingingRight = false;
    private const float attackInvulnDuration = 0.25f;
    private const float attackInvulnCooldown = 2;
    private float attackInvulnTimer = 0;

    // sliding
    private bool isSliding = false;
    private float slideTimer = 0f;
    private const float slideDuration = 0.75f;
    private const float slideAttackDuration = 0.5f;
    private float slideCooldown = 0f;
    private bool slideAttacking = false;
    private Vector2 slideHitboxDisplacement = new Vector2(0.5f, 0f);
    private Vector2 slideAttackHitboxDisplacement = new Vector2(0.30f, 0.15f);

    private PowerUp currentPowerUp = PowerUp.None;
    [SerializeField]
    GameObject cardProjectile;
    [SerializeField]
    GameObject explosionProjectile;
    private float cardAttackTimer = 0f;
    private float cardAttackCooldown = 0.4f;
    private const float bombDuration = 10f;
    private const float cardDuration = 10f;
    private const float chargeDuration = 10f;
    private float powerUpTimer = 0f;

    // damage
    private float invulnerabilityTime = 1.5f;
    private float damageStunTime = 1f;
    private float currentStun = 0f;
    private float currentInvuln = 0f;

    public int health = 3;

    //Animator/SpriteRenderer
    private Animator myAnimator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        attackHb = this.transform.Find("Attack");
        slideHb = this.transform.Find("Slide");
        slideAttackHb = this.transform.Find("SlideAttack");
        heart = FindObjectOfType<HeartScript>(); // the first heart
        myAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentStun > 0)
        {
            // cooldowns
            DecrementCooldowns();

            // debug colors
            TestingColors();

            // break out early
            return;
        }
        // attacking functionality, game manager handles "un-attacking"
        if (isAttacking)
        {

            // check for first side
            if (!attackStarted)
            {
                // give attack start invuln
                if (attackInvulnTimer <= 0)
                {
                    currentInvuln = attackInvulnDuration;
                    attackInvulnTimer = attackInvulnCooldown;
                }

                // start the cycle, first move the hitbox to position
                attackHb.GetComponent<SpriteRenderer>().enabled = true;
                attackHb.GetComponent<CapsuleCollider2D>().enabled = true;
                myAnimator.SetBool("isAttacking", true);
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
                // send an attack if powered up
                CreateProjectile();
            }
            {
                Vector3 point = this.transform.position;
                Vector3 axis = new Vector3(0, 0, 1);

                // loop through attacking
                if (currentPowerUp != PowerUp.Charge)
                {
                    attackSwingSpeed = baseAttackSwingSpeed;
                }
                else { attackSwingSpeed = baseAttackSwingSpeed * 1.5f; }

                switch (attackSwingingRight)
                {

                    case true:
                        axis = new Vector3(0, 0, -1);
                        attackHb.RotateAround(point, axis, Time.deltaTime * attackSwingSpeed);
                        if (attackHb.localEulerAngles.z <= 90)
                        {
                            // create attack when switching directions
                            CreateProjectile();
                            attackSwingingRight = false;
                        }
                        break;
                    case false:
                        axis = new Vector3(0, 0, 1);
                        attackHb.RotateAround(point, axis, Time.deltaTime * attackSwingSpeed);
                        if (attackHb.eulerAngles.z >= 270)
                        {
                            CreateProjectile();
                            attackSwingingRight = true;
                        }
                        break;
                }

            }
        }
        else // clean up attacking
        {
            myAnimator.SetBool("isAttacking", false);
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
            myAnimator.SetBool("isKicking", true);
            //myAnimator.SetBool("isSliding", true);
            slideAttackHb.transform.localScale = new Vector2(slideTimer / -(2 * slideAttackDuration) + 1f, slideTimer / (2 * slideAttackDuration) + 1f);


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
            //myAnimator.SetBool("isKicking", true);
            myAnimator.SetBool("isSliding", true);
            switch (lastMovedRight)
            {
                case true:
                    if (!slideAttacking)
                    {
                        rb.velocity = new Vector2((((-1.5f / slideDuration * slideTimer) + 1.5f) * slidingSpeed), 0.0f);

                    }
                    else {
                        MoveRight();
                    }
                    break;

                case false:
                    if (!slideAttacking)
                    {
                        rb.velocity = new Vector2((((-1.5f / slideDuration * slideTimer) + 1.5f) * -slidingSpeed), 0.0f);
                    }
                    else {
                        MoveLeft();
                    }
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
            myAnimator.SetBool("isSliding", false);
            myAnimator.SetBool("isKicking", false);
            isSliding = false;
            slideAttacking = false;
            slideTimer = 0f;

            // move slide and slide attack hitbox away
            slideHb.GetComponent<BoxCollider2D>().enabled = false;
            slideHb.GetComponent<SpriteRenderer>().enabled = false;

            slideAttackHb.GetComponent<SpriteRenderer>().enabled = false;
            slideAttackHb.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (!isSliding & !isAttacking)
        {
            if (Mathf.Abs(rb.velocity.x) > 3.0f)
            {
                myAnimator.SetBool("isWalking", true);
            }
            else
            {
                myAnimator.SetBool("isWalking", false);
            }
        }

        // cooldowns
        DecrementCooldowns();

        // debug colors
        TestingColors();
    }
    // Movement
    public void MoveRight()
    {
        if (currentStun <= 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            rb.velocity = new Vector2(movingSpeed, 0.0f);
            lastMovedRight = true;
        }
    }
    public void MoveLeft()
    {
        if (currentStun <= 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            rb.velocity = new Vector2(-movingSpeed, 0.0f);
            lastMovedRight = false;
        }
    }
    public void SlideAttack()
    {
        if (isSliding && !slideAttacking && currentStun <= 0)
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

            // test for bomb powerup
            CreateExplosion();
        }
    }
    private void TestingColors()
    {
        // TESTING COLORS, REMOVE WHEN WE GET ART
        if (slideAttacking)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if (currentStun > 0f)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (isSliding)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (isAttacking)
        {
            GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else {
            switch (currentPowerUp)
            {
                case PowerUp.Bomb:
                    GetComponent<SpriteRenderer>().color = Color.magenta;
                    break;

                case PowerUp.Charge:
                    GetComponent<SpriteRenderer>().color = Color.gray;
                    break;
                case PowerUp.Card:
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                case PowerUp.None:
                default:
                    GetComponent<SpriteRenderer>().color = Color.white;
                    break;
            }
        }
    }
    public void GainPowerUp(PowerUp p)
    {
        if (p == PowerUp.Split)
        {
            heart.Split();
            return;
        }
        // remove any existing powerup stuff
        currentPowerUp = p;
        powerUpTimer = 0f;
        // play an animation as well

    }
    private void CreateProjectile()
    {
        // guard code
        if (currentPowerUp == PowerUp.Card && cardAttackTimer <= 0f)
        {
            // create projectile here
            GameObject instance = Instantiate(cardProjectile);
            instance.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
            // put on cooldown
            cardAttackTimer = cardAttackCooldown;
        }
    }
    private void CreateExplosion()
    {
        // guard code
        if (currentPowerUp == PowerUp.Bomb)
        {
            // remove powerup
            currentPowerUp = PowerUp.None;
            // create explosion
            GameObject instance = Instantiate(explosionProjectile);
            instance.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
        }
    }
    public void TakeDamage()
    {
        if (currentInvuln <= 0 && !isSliding)
        {
            FindObjectOfType<HealthManager>().TakeDamage();
            health--;
            if (health <= 0)
            {
                // get reference to game manager to help end game
                // placeholder reset
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                currentStun = damageStunTime;
                currentInvuln = invulnerabilityTime;
                myAnimator.Play("IdleAnimation", 0, 0.0f);

                // reset attacking properties
                isSliding = false;
                slideAttacking = false;
                slideTimer = 0f;

                slideHb.GetComponent<BoxCollider2D>().enabled = false;
                slideHb.GetComponent<SpriteRenderer>().enabled = false;

                slideAttackHb.GetComponent<SpriteRenderer>().enabled = false;
                slideAttackHb.GetComponent<BoxCollider2D>().enabled = false;

                isAttacking = false;
                attackHb.GetComponent<SpriteRenderer>().enabled = false;
                attackHb.GetComponent<CapsuleCollider2D>().enabled = false;
                attackStarted = false;
                attackSwingingRight = false;
            }
        }
    }
    private void DecrementCooldowns()
    {
        // decrement player cooldowns
        if (slideCooldown > 0)
        {
            slideCooldown -= Time.deltaTime;
        }

        if (cardAttackTimer > 0)
        {
            cardAttackTimer -= Time.deltaTime;
        }
        if (currentInvuln > 0)
        {
            currentInvuln -= Time.deltaTime;
        }
        if (currentStun > 0)
        {
            currentStun -= Time.deltaTime;
        }
        if (attackInvulnTimer > 0)
        {
            attackInvulnTimer -= Time.deltaTime;
        }

        // increase time spent in the powerup state
        if (currentPowerUp != PowerUp.None)
        {
            powerUpTimer += Time.deltaTime;
        }

        // deactivate expired powerups
        if (
            powerUpTimer >= chargeDuration
         || powerUpTimer >= bombDuration
         || powerUpTimer >= cardDuration)
        {
            currentPowerUp = PowerUp.None;
            powerUpTimer = 0f;
        }
    }
}
