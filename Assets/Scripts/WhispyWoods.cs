using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WhispyWoods : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;
    public Transform bossFightCamerafocus;
    public int startingHealth;
    int health;
    public GameObject bossBar;
    public Image bossBarFill;
    public bool fightStarted;
    public float attackPhaseLength, applePhaseLength, attackSpeed, appleSpeed;
    float phaseTimer, attackCooldown;
    public bool attackPhase;
    public GameObject attackProjectile;
    public GameObject[] applesToDrop;
    public Transform attackPoint;
    public Transform[] appleSpawns;
    bool[] spawnedApples = {false, false, false, false};
    public float faceY, faceYHigh, faceYLow, faceSpeed;

    void Start() {
        transform.position = new Vector3(transform.position.x, faceY, transform.position.z);
        health = startingHealth;
    }

    void Update()
    {
        if(fightStarted) {
            phaseTimer -= Time.deltaTime;
            attackCooldown -= Time.deltaTime;
            if(phaseTimer <= 0) {
                attackPhase = !attackPhase;
                attackCooldown = attackPhase ? attackSpeed : appleSpeed;
                phaseTimer = attackPhase ? attackPhaseLength : applePhaseLength;
                if(!attackPhase) {
                    transform.position = new Vector3(transform.position.x, faceY, transform.position.z);
                    for(int i = 0; i < 4; i++)
                        spawnedApples[i] = false;
                }
            }
            if(attackPhase) {
                transform.position = new Vector3(transform.position.x, faceYLow + Mathf.PingPong(Time.time * faceSpeed, faceYHigh - faceYLow), transform.position.z);
                if(attackCooldown <= 0) {
                    GameObject bullet = Instantiate(attackProjectile, attackPoint.transform.position, attackPoint.transform.rotation);
                    bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * 1000);
                    attackCooldown = attackSpeed;
                }
            } else {
                if(attackCooldown <= 0) {
                    int i = Random.Range(0, 3), j = Random.Range(0, 3);
                    if(!spawnedApples[i]) {
                        spawnedApples[i] = true;
                        Instantiate(applesToDrop[Random.Range(0, applesToDrop.Length)], appleSpawns[i]);
                    }
                    else if(!spawnedApples[j]) {
                        spawnedApples[j] = true;
                        Instantiate(applesToDrop[Random.Range(0, applesToDrop.Length)], appleSpawns[j]);
                    }
                    attackCooldown = appleSpeed;
                }
            }
        }
    }

    public void DealDamage() {
        if(!fightStarted)
            StartFight();
        health--;
        bossBarFill.fillAmount = ((float)health)/startingHealth;
        if(health <= 0)
            SceneManager.LoadScene("Win");
    }

    public void StartFight() {
        KirbyController.bossCheckpoint = true;
        cinemachine.m_Follow = bossFightCamerafocus;
        bossBar.SetActive(true);
        fightStarted = true;
        attackPhase = true;
    }
}
