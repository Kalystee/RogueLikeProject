using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static EntityStat;

[Serializable]
public class TypedUpgrade : Upgrade
{
    public EntityStatsType statType;

    public TypedUpgrade(EntityStatsType statType, UpgradeType type, float upgradeValue, object source) : base (type, upgradeValue, source)
    {
        this.statType = statType;
        this.type = type;
        this.upgradeValue = upgradeValue;
        this.source = source;
    }
}
