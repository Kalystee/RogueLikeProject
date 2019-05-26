using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/WeaponRange")]
public class WeaponRange : Weapon
{
    public float projectileSpeed;
    public float accuracy;
    public GameObject projectile;
}
