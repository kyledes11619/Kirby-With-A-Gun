using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AmmoType : ScriptableObject
{
    public GameObject projectile;
    public Sprite uISymbol;
    public float shootForce;
    public bool slow, stun;
    public float effectLength;
}
