using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision) {
        GameObject ob = collision.collider.gameObject;
        if(ob.CompareTag("enemy")) {
            if(ob.GetComponent<Enemy>().diesToBullets) {
                Instantiate(ob.GetComponent<Enemy>().deathParticles, ob.transform.position, ob.transform.rotation);
                Destroy(ob);
            }   
        }
        if(ob.CompareTag("whispy")) {
            ob.GetComponent<WhispyWoods>().DealDamage();
        }
        Destroy(gameObject);
    }
}
