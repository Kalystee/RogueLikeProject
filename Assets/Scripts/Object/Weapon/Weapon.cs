using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float range;
    public float delayBetweenUses;
    public float knockbackPower;
    public List<TypedUpgrade> stats;

}
