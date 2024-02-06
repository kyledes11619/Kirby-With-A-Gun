using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KirbyController : MonoBehaviour
{
    Rigidbody2D rb;
    public float jumpPower = 5, walkPower = 5, inhalePower = 2, knockbackPower = 1, xInhaleRange, yInhaleRange;
    public int jumps = 6;
    public bool jumping = false, facingLeft = false, inhaling = false;
    public float reloadTime;
    float reloadTimer;
    public Transform gunPointL, gunPointR;
    public int health, maxHealth, score;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        RefreshAmmoUI();
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
        if(x != 0) {
            facingLeft = x < 0;
        }
        rb.velocity = new Vector2(x * walkPower, doJump ? jumpPower : rb.velocity.y);
        inhaling = Input.GetButton("Fire1");
        if(inhaling) {
            Collider2D[] intersecting = Physics2D.OverlapAreaAll(new Vector2(transform.position.x, transform.position.y - yInhaleRange/2), new Vector2(transform.position.x + (facingLeft ? -xInhaleRange : xInhaleRange), transform.position.y + yInhaleRange/2));
                foreach(Collider2D c in intersecting) {
                    GameObject enemy = c.gameObject;
                    if(enemy.CompareTag("enemy"))
                        enemy.GetComponent<Rigidbody2D>().AddForce((transform.position - enemy.transform.position) * inhalePower / (float)Math.Pow(Vector2.Distance(transform.position, enemy.transform.position), 2));
                }
        }
        if(reloadTimer > 0) {
            reloadTimer -= Time.deltaTime;
            if(reloadTimer < 0)
                reloadTimer = 0;
            gunUI[0].transform.rotation = Quaternion.AngleAxis(currentAmmo * -60 + reloadTimer / reloadTime * 60, Vector3.forward);
        }
        else if(Input.GetButtonDown("Fire2") && !ammo[currentAmmo].Equals(blank)) {
            reloadTimer += reloadTime;
            GameObject bullet = Instantiate(ammo[currentAmmo].projectile, facingLeft ? gunPointL.position : gunPointR.position, facingLeft ? gunPointL.rotation : gunPointR.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * ammo[currentAmmo].shootForce);
            ammo[currentAmmo] = blank;
            currentAmmo++;
            if(currentAmmo == 6)
                currentAmmo = 0;
            //Animate the spinning?
            RefreshAmmoUI();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        GameObject ob = collision.collider.gameObject;
        if(ob.CompareTag("ground")) {
            jumps = 6;
            jumping = false;
        }
        if(ob.CompareTag("enemy")) {
            if(inhaling) {
                if(ob.GetComponent<Enemy>().ammoOnKill != null)
                    for(int i = 0; i < 6; i++)
                        if(ammo[i + (currentAmmo >= 6 ? currentAmmo - 6 : currentAmmo)].Equals(blank))
                            ammo[i] = ob.GetComponent<Enemy>().ammoOnKill;
                ChangeScore(ob.GetComponent<Enemy>().scoreOnKill);
                RefreshAmmoUI();
                Destroy(ob);
            }
            else {
                GetComponent<Rigidbody2D>().AddForce((transform.position - ob.transform.position) * -knockbackPower);
                ChangeHealth(-1);
            }
        }
    }

    public Image healthbar;
    public Text scoreText;
    
    public void ChangeHealth(int i) {
        health += i;
        if(health > maxHealth)
            health = maxHealth;
        healthbar.fillAmount = (float)health / maxHealth;
    }

    public void ChangeScore(int i) {
        score += i;
        scoreText.text = "" + i;
    }

    public Image[] gunUI;
    public AmmoType[] ammo;
    public int currentAmmo;
    public AmmoType blank;

    public void RefreshAmmoUI() {
        for(int i = 1; i < 7; i++) {
            gunUI[i].sprite = ammo[i-1].uISymbol;
        }
    }
}