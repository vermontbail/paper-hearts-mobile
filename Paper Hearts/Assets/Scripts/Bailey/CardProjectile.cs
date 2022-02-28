using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardProjectile : MonoBehaviour
{
    BoxCollider2D box;
    private float countdown;
    private float lifespan = 3f;
    private float movespeed = 10f;
    private float verticalPush = 10f;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // move up
        transform.position = transform.position + new Vector3(0, movespeed * Time.deltaTime, 0);


        // destroy if reaches lifespan
        countdown += Time.deltaTime;
        if (countdown >= lifespan)
        {
            Object.Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Heart")
        {
            // add upward force
            col.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(col.transform.GetComponent<Rigidbody2D>().velocity.x, verticalPush);

            // remove object early
            Object.Destroy(this.gameObject);
        }
    }
}
