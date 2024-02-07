using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingEnemy : Enemy
{
    Rigidbody2D rb;
    public float bounceTime, bounceForce, shockDelay, shockRange, shockForce;
    float bounceTimer;
    bool shocked;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        bounceTimer = bounceTime;
    }

    void Update()
    {
        bounceTimer -= Time.deltaTime;
        if(bounceTimer <= 0) {
            bounceTimer = bounceTime;
            shocked = false;
            rb.velocity = new Vector2(0, bounceForce);
        }
        if(!shocked && bounceTimer <= bounceTime - shockDelay) {
            shocked = true;
            Collider2D[] intersecting = Physics2D.OverlapCircleAll(transform.position, shockRange);
                foreach(Collider2D c in intersecting) {
                    GameObject kirby = c.gameObject;
                    if(kirby.CompareTag("kirby")) {
                        kirby.GetComponent<Rigidbody2D>().velocity = -(transform.position - kirby.transform.position) * shockForce / (float)System.Math.Pow(Vector2.Distance(transform.position, kirby.transform.position), 2);
                        kirby.GetComponent<KirbyController>().ChangeHealth(-1);
                    }
                }
        }
    }
}
