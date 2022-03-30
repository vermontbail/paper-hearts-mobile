using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollowScript : MonoBehaviour
{
    public GameObject target;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    Rigidbody2D rb;
    BoxCollider2D box;
    [SerializeField] float speedMultiplier;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        
        Vector3 difference = target.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        Vector2 toTarget = (target.transform.position - transform.position).normalized;
        rb.velocity = toTarget * speedMultiplier;

        if (Vector3.Distance(transform.position, target.transform.position) < 1.15f)
        {
            rb.velocity = Vector3.zero;
        }

    }
}
