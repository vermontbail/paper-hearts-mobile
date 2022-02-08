using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
    // fields
    Rigidbody2D rb;
    BoxCollider2D box;

    float cooldownTime = 0.5f;
    float currentCooldown = 0;

    float baseYCoefficient = 0.000012f;
    float baseXCoefficient = 0.0015f;

    float maxX = 12;
    float minY = 7;
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
    }
    // called when the heart hits the 
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
                // fully right: -90
                // fully left: -270

                case true:
                    // at -270, staight up
                    // at -180, most horizontal
                    // at -90, straight down

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
    }
}
