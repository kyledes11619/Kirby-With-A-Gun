using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingEnemy : Enemy
{
    Rigidbody2D rb;
    public float bounceTime, bounceForce, shockDelay, shockRange, shockForce;
    float bounceTimer;
    bool shocked;
    public GameObject particlesForAttack;
    public bool stunKirbyOnDamage, slowKirbyOnDamage;
    public float kirbyEffectTime;

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
            Instantiate(particlesForAttack, transform.position, transform.rotation);
            shocked = true;
            Collider2D[] intersecting = Physics2D.OverlapCircleAll(transform.position, shockRange);
                foreach(Collider2D c in intersecting) {
                    GameObject kirby = c.gameObject;
                    if(kirby.CompareTag("kirby")) {
                        kirby.GetComponent<Rigidbody2D>().velocity = -(transform.position - kirby.transform.position) * shockForce / (float)System.Math.Pow(Vector2.Distance(transform.position, kirby.transform.position), 2);
                        if(stunKirbyOnDamage) {
                            kirby.GetComponent<KirbyController>().currentlyStunned = true;
                            kirby.GetComponent<KirbyController>().stunTime = kirbyEffectTime;
                        }
                        if(slowKirbyOnDamage) {
                            kirby.GetComponent<KirbyController>().currentlySlowed = true;
                            kirby.GetComponent<KirbyController>().slowTime = kirbyEffectTime;
                        }
                        kirby.GetComponent<KirbyController>().ChangeHealth(-1);
                    }
                }
        }
    }
}
