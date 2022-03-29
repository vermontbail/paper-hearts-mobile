using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    // Start is called before the first frame update
    List<Transform> parts;
    Rigidbody2D rb;
    BoxCollider2D box;
    private float movespeed = 2f;
    private float distanceBetweenParts = 1f;
    private Vector2 moveDirection;
    private Vector2 lastFrameVelocity;
    private float minVelocity = 1f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        parts = new List<Transform>();
        foreach (Transform child in transform.parent)
        {
            if (child.gameObject.tag == "BossPart")
            {
                parts.Add(child);
                Physics2D.IgnoreCollision(child.GetComponent<BoxCollider2D>(), this.transform.GetComponent<BoxCollider2D>(), true);
            }
        }
        // set head first direction
        moveDirection = new Vector2(Random.Range(-1, 1), 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastFrameVelocity = moveDirection;

        // debug movement
        Move();
    }
    private void Move()
    {
        // debug movement
        // move head
        // check for direction change
        transform.position = (Vector2)transform.position + (moveDirection * movespeed * Time.deltaTime);

        // get the rest of the parts to follow
        for (int i = 0; i < parts.Count; i++)
        {
            if (i == 0)
            {
                while (Vector2.Distance(parts[i].position, this.transform.position) >= distanceBetweenParts)
                {
                    parts[i].position = Vector2.MoveTowards(parts[i].position, this.transform.position, 0.01f);
                }
            }
            else
            {
                while (Vector2.Distance(parts[i].position, parts[i - 1].position) >= distanceBetweenParts)
                {
                    parts[i].position = Vector2.MoveTowards(parts[i].position, parts[i - 1].position, 0.01f);
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag != "BossPart" && col.transform.tag != "Heart")
        {
            Bounce(col.contacts[0].normal);
        }
    }
    private void Bounce(Vector2 collisionNormal)
    {
        var speed = movespeed;
        if (collisionNormal.x == 0)
        {
            if (transform.position.x > 0)
            {
                collisionNormal.x = 0.2f;
            }
            else
            {
                collisionNormal.x = -0.2f;
            }
        }
        if (collisionNormal.y == 0)
        {
            if (transform.position.y > 0)
            {
                collisionNormal.y = 0.2f;
            }
            else
            {
                collisionNormal.y = -0.2f;
            }
        }
        var direction = Vector2.Reflect(moveDirection.normalized, collisionNormal);
        // x
        if (direction.x >= 0 && Mathf.Round(direction.x) != 1)
        {
            direction.x = 0.5f;
        }
        if (direction.x <= 0 && Mathf.Round(direction.x) != -1)
        {
            direction.x = -0.5f;
        }
        // y
        if (direction.y >= 0 && Mathf.Round(direction.y) != 1)
        {
            direction.y = 0.5f;
        }
        if (direction.y <= 0 && Mathf.Round(direction.y) != -1)
        {
            direction.y = -0.5f;
        }


        moveDirection = direction * Mathf.Max(speed, minVelocity);


        if (moveDirection.x == -1 || moveDirection.x == 1)
        {
            if (transform.position.y > 0)
            {
                moveDirection.y = -1f;
            }
            else
            {
                moveDirection.y = 1f;
            }
        }
        Debug.Log(moveDirection);
    }
}
