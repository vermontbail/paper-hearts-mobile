using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
    // fields
    Rigidbody2D rb;
    BoxCollider2D box;

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

                    rb.velocity = new Vector2(x, y);
                    break;
                case false:
                    y = -baseYCoefficient * Mathf.Pow(z - 180, 3) + minY;
                    x = baseXCoefficient * Mathf.Pow(z - 180f, 2) - maxX;

                    rb.velocity = new Vector2(x, y);
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
    }
}
