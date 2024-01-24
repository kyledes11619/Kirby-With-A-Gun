using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbySuction : MonoBehaviour
{
    KirbyController kirby;

    void Start() {
        kirby = GetComponentInParent<KirbyController>();
    }

    void OnCollisionStay2D(Collision2D collision) {
        GameObject enemy = collision.collider.gameObject;
        if(enemy.CompareTag("enemy")) {
            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(enemy.transform.position.x - kirby.transform.position.x, enemy.transform.position.y - kirby.transform.position.y) * kirby.inhalePower);
        }
    }
}
