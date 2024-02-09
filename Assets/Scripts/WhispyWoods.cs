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

    void Start() {
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
                if(!attackPhase)
                    for(int i = 0; i < 4; i++)
                        spawnedApples[i] = false;
            }
            if(attackPhase) {
                if(attackCooldown <= 0) {
                    Instantiate(attackProjectile, attackPoint.transform.position, attackPoint.transform.rotation);
                    attackCooldown = attackSpeed;
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
