using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool ApplyUpgrade(TypedUpgrade typedUpgrade)
    {
        BasicStat affectedStat = GetStat(typedUpgrade.statType);
        if (affectedStat is UpgradableStat)
        {
            (affectedStat as UpgradableStat).AddUpgrade(typedUpgrade);
            return true;
        }
        else if (affectedStat is UpgradableClampedStat)
        {
            (affectedStat as UpgradableClampedStat).AddUpgrade(typedUpgrade);
            return true;
        }
        return false;
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
