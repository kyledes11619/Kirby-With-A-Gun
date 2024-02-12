using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KirbyController : MonoBehaviour
{
    Rigidbody2D rb;
    public float jumpPower = 5, walkPower = 5, inhalePower = 2, knockbackPower = 1, xInhaleRange, yInhaleRange;
    public int jumps = 6;
    public bool jumping = false, facingLeft = false, inhaling = false;
    public float reloadTime, invincibleTimeAfterHit = .5f;
    float reloadTimer, hitInvTimer;
    public Transform gunPointL, gunPointR;
    public int health, maxHealth, score;
    public bool currentlyStunned, currentlySlowed;
    public float stunTime, slowTime;
    Animator animator;

    public Text livesCounter;
    public static int lives = 3;
    public static bool bossCheckpoint;
    public Vector3 bossRespawn;
    public static bool invincibile;
    public AudioSource reloadSrc, inhaleSrc;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        RefreshAmmoUI();
        if(lives <= 0) {
            lives = 3;
            bossCheckpoint = false;
        }
        else if(bossCheckpoint)
            transform.position = bossRespawn;
        livesCounter.text = "" + lives;
    }

    void Update()
    {
        if(currentlyStunned) {
            stunTime -= Time.deltaTime;
            if(stunTime <= 0)
                currentlyStunned = false;
        }
        if(currentlySlowed) {
            slowTime -= Time.deltaTime;
            if(slowTime <= 0)
                currentlySlowed = false;
        }
        if(hitInvTimer > 0)
            hitInvTimer-=Time.deltaTime;
        bool doJump = false;
        if(Input.GetButtonDown("Jump") && jumps > 0 && !currentlyStunned) {
            jumping = true;
            doJump = true;
            animator.SetTrigger("Jumping");
            jumps--;
        }
        float x = currentlyStunned ? 0 : Input.GetAxis("Horizontal") * (currentlySlowed ? walkPower/2 : walkPower);
        animator.SetFloat("Speed", Math.Abs(x));
        if(x != 0) {
            facingLeft = x < 0;
        }
        rb.velocity = new Vector2(x, doJump ? jumpPower : rb.velocity.y);
        inhaling = Input.GetButton("Fire1");
        if(inhaling) {
            if(!inhaleSrc.isPlaying)
                inhaleSrc.Play();
            animator.SetBool("isInhaling", true);
            Collider2D[] intersecting = Physics2D.OverlapAreaAll(new Vector2(transform.position.x, transform.position.y - yInhaleRange/2), new Vector2(transform.position.x + (facingLeft ? -xInhaleRange : xInhaleRange), transform.position.y + yInhaleRange/2));
                foreach(Collider2D c in intersecting) {
                    GameObject enemy = c.gameObject;
                    if(enemy.CompareTag("enemy"))
                        enemy.GetComponent<Rigidbody2D>().AddForce((transform.position - enemy.transform.position) * inhalePower / (float)Math.Pow(Vector2.Distance(transform.position, enemy.transform.position), 2));
                }
        }
        else {
            if(inhaleSrc.isPlaying)
                inhaleSrc.Stop();
            animator.SetBool("isInhaling", false);
        }

        if (reloadTimer > 0) {
            reloadTimer -= Time.deltaTime;
            if(reloadTimer < 0)
                reloadTimer = 0;
            gunUI[0].transform.rotation = Quaternion.AngleAxis(currentAmmo * -60 + reloadTimer / reloadTime * 60, Vector3.forward);
        }
        else if(Input.GetButtonDown("Fire2") && !ammo[currentAmmo].Equals(blank)) {
            reloadSrc.Play();
            reloadTimer += reloadTime;
            GameObject bullet = Instantiate(ammo[currentAmmo].projectile, facingLeft ? gunPointL.position : gunPointR.position, facingLeft ? gunPointL.rotation : gunPointR.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * ammo[currentAmmo].shootForce);
            ammo[currentAmmo] = blank;
            currentAmmo++;
            if(currentAmmo == 6)
                currentAmmo = 0;
            RefreshAmmoUI();
        }
        else if(Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3")) {
            reloadSrc.Play();
            reloadTimer += reloadTime;
            currentAmmo++;
            if(currentAmmo == 6)
                currentAmmo = 0;
            RefreshAmmoUI();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        GameObject ob = collision.collider.gameObject;
        if(ob.CompareTag("ground")) {
            jumps = 6;
            jumping = false;
            animator.SetTrigger("Land");
        }
        if(ob.CompareTag("enemy")) {
            if(inhaling) {
                if(ob.GetComponent<Enemy>().ammoOnKill != null)
                    for(int i = 0; i < 6; i++)
                        if(ammo[i].Equals(blank))
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
        if(invincibile || i < 0 && hitInvTimer > 0)
            return;
        health += i;
        if(i < 0)
            hitInvTimer+=invincibleTimeAfterHit;
        if(health > maxHealth)
            health = maxHealth;
        healthbar.fillAmount = (float)health / maxHealth;
        if(health <= 0) {
            if(lives > 0) {
                lives--;
                SceneManager.LoadScene("Level_Scene");
            }
            else
                SceneManager.LoadScene("GameOver");
        }
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
