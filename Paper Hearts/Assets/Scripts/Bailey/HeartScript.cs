using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
    // fields
    Rigidbody2D rb;
    BoxCollider2D box;

    const float totalCloneTime = 10f;
    float currentTimer = 0f;
    GameObject heartClone = null;
    public bool isClone = false;

    float currentMultiplier = 1f;
    const float chargeMultiplier = 1.5f;

    float cooldownTime = 0.25f;
    float currentCooldown = 0;

    float baseYCoefficient = 0.000012f;
    float baseXCoefficient = 0.0015f;

    float maxX = 12;
    float minY = 7;

    float slideX = 12f;
    float slideY = 8f;

    float slideAttackX = 5f;
    float slideAttackY = 12f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    public void Split()
    {
        // this one is intense
        if (heartClone == null)
        {
            // create clone
            // link to this object and set as clone
            // give an expiration timer
            // 
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // change the velocity of the heart
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        // rotate the heart
        this.transform.Rotate(new Vector3(0f, 0f, -rb.velocity.x));

        // set current power based on player state
        if (FindObjectOfType<PlayerController>().PowerUp == PowerUp.Charge)
        {
            currentMultiplier = chargeMultiplier;
        }
        else currentMultiplier = 1f;
    }
    // called when the heart hits any collision
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);

        // check for regular attack collision
        if (col.transform.tag == "Attack" && currentCooldown <= 0)
        {
            // set cooldown
            currentCooldown = cooldownTime;
            bool swingingRight = col.transform.GetComponentInParent<PlayerController>().AttackSwingingRight;
            float z = col.transform.localEulerAngles.z;
            float x, y;
            switch (swingingRight)
            {
                // fully right: 90
                // fully left: 270

                case true:
                    // at 270, staight up
                    // at 180, most horizontal
                    // at 90, straight down

                    // independant : z variable
                    // dependant : x and y velocity
                    // y = 
                    // 
                    
                    y = baseYCoefficient * Mathf.Pow(z - 180, 3) + minY;
                    x = -baseXCoefficient * Mathf.Pow(z - 180f, 2) + maxX;

                    rb.velocity = new Vector2(x * currentMultiplier, y * currentMultiplier);
                    break;
                case false:
                    y = -baseYCoefficient * Mathf.Pow(z - 180, 3) + minY;
                    x = baseXCoefficient * Mathf.Pow(z - 180f, 2) - maxX;

                    rb.velocity = new Vector2(x * currentMultiplier, y * currentMultiplier);
                    break;
            }
        }
        if (col.transform.tag == "Slide" && currentCooldown <= 0)
        {
            // set cooldown
            currentCooldown = cooldownTime;
            // get position of local transform from colliding box to find player's current facing direction
            if (col.transform.localPosition.x > 0)
            {
                rb.velocity = new Vector2(slideX, slideY);
            }
            else
            {
                rb.velocity = new Vector2(-slideX, slideY);
            }
        }
        if (col.transform.tag == "SlideAttack" && currentCooldown <= 0)
        {
            // set cooldown
            currentCooldown = cooldownTime;
            // get position of local transform from colliding box to find player's current facing direction
            if (col.transform.localPosition.x > 0)
            {
                rb.velocity = new Vector2(slideAttackX, slideAttackY);
            }
            else
            {
                rb.velocity = new Vector2(-slideAttackX, slideAttackY);
            }
        }

        if (col.transform.tag == "Panel" && !col.GetComponent<PanelScript>().Flipped)
        {
            // set color, play animation, however it should work
            col.GetComponent<PanelScript>().Flip();
            // increment score
            FindObjectOfType<GameManager>().AddScore();
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Block")
        {
            // deactivate block
            col.gameObject.SetActive(false);
            // increment score
            FindObjectOfType<GameManager>().AddScore();
        }
        // check for powerup box
        if (col.transform.tag == "PowerUp Block")
        {
            // create the object's correct powerup
            col.transform.GetComponent<PowerUpBlock>().CreatePowerUp();
            // deactivate block
            col.gameObject.SetActive(false);
            // increment score
            FindObjectOfType<GameManager>().AddScore();
        }
    }
}
