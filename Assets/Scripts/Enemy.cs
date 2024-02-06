using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int scoreOnKill;
    public AmmoType ammoOnKill;
    public bool diesToBullets;
    public GameObject deathParticles;
}
