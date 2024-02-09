using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class WhispyWoods : MonoBehaviour
{
    public CinemachineBrain cinemachine;
    public GameObject bossFightCamerafocus;
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
                    Instantiate(attackProjectile, attackPoint.transform.position, attackPoint.transform.rotation);
                    attackCooldown = attackSpeed;
                }
            } else {
                if(attackCooldown <= 0) {
                    int i = Random.Range(0, 3), j = Random.Range(0, 3);
                    if(!spawnedApples[i]) {
                        spawnedApples[i] = true;
                    }
                    else if(!spawnedApples[j]) {
                        spawnedApples[j] = true;
                    }
                }
            }
        }
    }

    public void DealDamage() {
        if(!fightStarted)
            StartFight();
        health--;
        bossBarFill.fillAmount = (float)health/startingHealth;
    }

    public void StartFight() {
        bossBar.SetActive(true);
        fightStarted = true;
        attackPhase = true;
    }
}
