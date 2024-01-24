using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KirbyController : MonoBehaviour
{
    Rigidbody2D rb;
    public float jumpPower = 5, walkPower = 5, inhalePower = 2, xInhaleRange, yInhaleRange;
    public int jumps = 6;
    public bool jumping = false, facingLeft = false, inhaling = false;
    public Transform inhalePoint;

    public Image[] gunUI;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool doJump = false;
        if(Input.GetButtonDown("Jump") && jumps > 0) {
            jumping = true;
            doJump = true;
            jumps--;
        }
        float x = Input.GetAxis("Horizontal");
        if(x > 0) {
            facingLeft = x < 0;
        }
            rb.velocity = new Vector2(x * walkPower, doJump ? jumpPower : rb.velocity.y);
        inhaling = Input.GetButton("Fire1");
        if(inhaling) {
            Collider2D[] intersecting = Physics2D.OverlapCircleAll(transform.position, xInhaleRange);
                foreach(Collider2D c in intersecting) {
                    GameObject enemy = c.gameObject;
                    if(enemy.CompareTag("enemy") && Math.Abs(enemy.transform.position.y - transform.position.y) < yInhaleRange)
                        enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(enemy.transform.position.x - transform.position.x, enemy.transform.position.y - transform.position.y) * inhalePower);
                }
        }
        if(Input.GetButton("Fire2")) {
            Debug.Log("2");
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.gameObject.CompareTag("ground")) {
            jumps = 6;
            jumping = false;
        }
        if(collision.collider.gameObject.CompareTag("enemy")) {
            if(inhaling)
                Destroy(collision.collider.gameObject);
        }
    }
}
