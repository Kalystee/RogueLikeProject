using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityProperties
{
    [Header("Stats Settings")]
    public BasicStat Money;
    public EntityStat Stats = new EntityStat();
    
    [Header("Weapons")]
    [SerializeField] public WeaponRange weaponRange;
    [SerializeField] public WeaponMelee weaponMelee;
}
