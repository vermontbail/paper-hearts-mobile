using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimationMovement : MonoBehaviour
{
    float verticalSpeed = 100f;
    float moveTimer = 0.55f;
    SpriteRenderer mySpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0)
        {
            mySpriteRenderer.transform.localPosition = mySpriteRenderer.transform.localPosition + new Vector3(0, verticalSpeed * Time.deltaTime, 0);
            moveTimer = 0.55f;
        }
    }
}
