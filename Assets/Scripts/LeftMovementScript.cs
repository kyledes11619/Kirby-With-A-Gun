using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftMovementScript : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        spriteRenderer.flipX = body.velocity.x < 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
