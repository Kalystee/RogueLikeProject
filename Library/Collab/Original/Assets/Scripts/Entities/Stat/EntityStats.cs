using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityStatsType
{
    Health,
    Damage,
    MoveSpeed,
    KnockbackResistance,
    JumpHeight,
    JumpQuantity,
    SprintMultiplier
}

[Serializable]
public class EntityStat
{
    [Header("Classics Stats")]
    public UpgradableClampedStat Health = new UpgradableClampedStat(50f);
    public UpgradableStat Damage = new UpgradableStat(5);
    public UpgradableStat MoveSpeed = new UpgradableStat(3f);
    public UpgradableStat KnockbackResistance = new UpgradableStat(1f);

    [Header("Deeper Stats")]
    public UpgradableStat JumpHeight = new UpgradableStat(1.5f);
    public UpgradableStat JumpQuantity = new UpgradableStat(1);
    public UpgradableStat SprintMultiplier = new UpgradableStat(1.5f);

    

    private Dictionary<EntityStatsType, BasicStat> statsDictionnary;

    public EntityStat()
    {
        this.statsDictionnary = new Dictionary<EntityStatsType, BasicStat>
        {
            { EntityStatsType.Health, Health },
            { EntityStatsType.Damage, Damage },
            { EntityStatsType.MoveSpeed, MoveSpeed },
            { EntityStatsType.KnockbackResistance, KnockbackResistance },
            { EntityStatsType.JumpHeight, JumpHeight },
            { EntityStatsType.JumpQuantity, JumpQuantity },
            { EntityStatsType.SprintMultiplier, SprintMultiplier }
        };
    }

    public BasicStat GetStat(EntityStatsType type)
    {
        if(statsDictionnary.ContainsKey(type))
        {
            return statsDictionnary[type];
        }

        Debug.LogError("Trying to access an undefined stat ! returning null");
        return null;
    }
}
