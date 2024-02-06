using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingEnemy : Enemy
{
    Rigidbody2D rb;
    public float bounceTime, bounceForce;
    float bounceTimer;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        bounceTimer = bounceTime;
    }

    void Update()
    {
        bounceTimer -= Time.deltaTime;
        if(bounceTimer <= 0) {
            bounceTimer = bounceTime;
            rb.velocity = new Vector2(0, bounceForce);
        }
    }
}
