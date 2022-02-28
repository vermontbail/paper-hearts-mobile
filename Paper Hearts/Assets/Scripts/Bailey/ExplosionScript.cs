using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider2D box;
    float verticalSpeed = 8f;
    float horizonatalGrow = 1.5f;
    float maxWidth = 5f;
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(box, GameObject.Find("Player").GetComponent<BoxCollider2D>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.transform.localPosition.y <= 4)
        {
            transform.localPosition = transform.localPosition + new Vector3(0, verticalSpeed * Time.deltaTime, 0);
        }
        else
        {
            transform.localScale = transform.localScale + new Vector3(horizonatalGrow * Time.deltaTime, 0, 0);
        }
        // destroy when explosion ends
        if (transform.localScale.x >= maxWidth)
        {
            GameObject.Destroy(this.transform.parent.gameObject);
        }
    }

    // explosion breaks blocks and flips panels
    private void OnTriggerStay2D(Collider2D col)
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
        if (col.transform.tag == "Panel" && !col.transform.GetComponent<PanelScript>().Flipped)
        {
            // set color, play animation, however it should work
            col.transform.GetComponent<PanelScript>().Flip();
            // increment score
            FindObjectOfType<GameManager>().AddScore();
        }
    }
}
