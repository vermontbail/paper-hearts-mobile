using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// powerup enum
public enum PowerUp
{
    None,
    Card,
    Bomb,
    Split,
    Charge
}
public class PowerUpScript : MonoBehaviour
{
    public PowerUp powerUp;
    Rigidbody2D rb;
    BoxCollider2D box;
    CircleCollider2D circle;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        circle = GetComponent<CircleCollider2D>();
        Physics2D.IgnoreCollision(box, GameObject.Find("Player").GetComponent<BoxCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            col.transform.GetComponentInParent<PlayerController>().GainPowerUp(powerUp);
            Object.Destroy(this.gameObject);
        }
    }
}
