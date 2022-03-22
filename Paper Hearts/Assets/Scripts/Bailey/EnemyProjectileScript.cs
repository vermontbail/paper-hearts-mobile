using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    BoxCollider2D box;
    private float countdown;
    private float lifespan = 10f;
    private float movespeed = 3f;
    private float spinSpeed = 10f;
    private Vector3 launchDirection;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        launchDirection = -(this.transform.position - FindObjectOfType<PlayerController>().transform.position).normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // move towards given point
        transform.position = transform.position + (new Vector3(launchDirection.x, launchDirection.y, 0) * movespeed * Time.deltaTime);

        // rotate the projectile
        this.transform.Rotate(new Vector3(0f, 0f, spinSpeed));

        // destroy if reaches lifespan
        countdown += Time.deltaTime;
        if (countdown >= lifespan)
        {
            Object.Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            col.transform.GetComponent<PlayerController>().TakeDamage();

            // remove object early
            Object.Destroy(this.gameObject);
        }
    }
}
