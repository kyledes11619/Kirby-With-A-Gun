using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    Rigidbody2D rb;
    public bool movingLeft = true;
    public float turnAroundTime, walkForce;
    float turnTimer;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        turnTimer = turnAroundTime;
    }

    void Update()
    {
        turnTimer -= Time.deltaTime;
        if(turnTimer <= 0) {
            turnTimer = turnAroundTime;
            movingLeft = !movingLeft;
        }
        rb.AddForce(new Vector2(walkForce * (movingLeft ? -1 : 1), 0));
    }
}