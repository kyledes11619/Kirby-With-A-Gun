using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision) {
        GameObject ob = collision.collider.gameObject;
        Debug.Log("Waddle Dee was SHot");
        if(ob.CompareTag("enemy")) {
            Debug.Log("Waddle Dee was SHot");
            if(ob.GetComponent<Enemy>().diesToBullets) {
                Instantiate(ob.GetComponent<Enemy>().deathParticles, ob.transform, false);
                Destroy(ob);
            }   
        }
        Destroy(gameObject);
    }
}
