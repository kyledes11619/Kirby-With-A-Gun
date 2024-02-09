using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    Rigidbody2D rb;
    public bool movingLeft = true;
    public float turnAroundTime, walkForce, animSpeed;
    float turnTimer;
    Animator animator;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        turnTimer = turnAroundTime;
    }

    void Update()
    {
        if(currentlyStunned) {
            animator.SetFloat("Speed", 0);
            return;
        }
        turnTimer -= Time.deltaTime;
        if(turnTimer <= 0) {
            turnTimer = turnAroundTime;
            movingLeft = !movingLeft;
        }
        rb.AddForce(new Vector2(walkForce * (movingLeft ? -1 : 1) * (currentlySlowed ? .25 : 1), 0));
        animator.SetFloat("Speed", rb.velocity.x);
    }
}
