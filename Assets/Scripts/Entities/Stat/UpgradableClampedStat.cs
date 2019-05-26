using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using static Upgrade;

[Serializable]
public class UpgradableClampedStat : ClampedStat
{
    public UpgradableClampedStat(float maxValue) : base(maxValue)
    {
        this._upgradesList = new List<Upgrade>();
        this._upgradedMaxValue = maxValue;
        this._isDirty = false;
    }

    [SerializeField] protected float _upgradedMaxValue;
    [SerializeField] protected bool _isDirty;
    
    protected Action<float> _onUpgradedMaxValueChanged;
    public void RegisterUpgradedMaxValueChangedCallback(Action<float> callback)
    {
        _onUpgradedMaxValueChanged += callback;
    }

    public void UnregisterUpgradedMaxValueChangedCallback(Action<float> callback)
    {
        _onUpgradedMaxValueChanged -= callback;
    }

    #region Public Getters/Setters
    public override float MaxValue
    {
        get
        {
            if (_isDirty)
            {
                float newValue = CalculateMaxValue();
                if (newValue != this._upgradedMaxValue)
                {
                    _onUpgradedMaxValueChanged?.Invoke(newValue);
                    this._upgradedMaxValue = newValue;
                }
                _isDirty = false;
            }
            return this._upgradedMaxValue;
        }
        set
        {
            BaseMaxValue = value;
        }
    }

    public virtual float BaseMaxValue
    {
        get
        {
            return this._maxValue;
        }
        set
        {
            if (value != this._maxValue)
            {
                this._onMaxValueChanged?.Invoke(value);
                this._maxValue = Mathf.Max(value, 0);
                this._isDirty = true;
            }
        }
    }

    public virtual int BaseMaxValueInt
    {
        get
        {
            return Mathf.RoundToInt(this._upgradedMaxValue);
        }
    }
    #endregion

    #region Upgrades
    [SerializeField] List<Upgrade> _upgradesList;
    
    public UpgradableClampedStat AddUpgrade(Upgrade upgrade)
    {
        if (!this._upgradesList.Contains(upgrade))
        {
            this._upgradesList.Add(upgrade);
            this._isDirty = true;
        }

        return this;
    }

    public UpgradableClampedStat RemoveUpgradesFrom(Upgrade upgrade)
    {
        if (this._upgradesList.Contains(upgrade))
        {
            this._upgradesList.Remove(upgrade);
            this._isDirty = true;
        }

        return this;
    }

    public UpgradableClampedStat RemoveUpgradesFrom(object source)
    {
        bool hasBeenModified = false;

        for (int i = this._upgradesList.Count - 1; i <= 0; i++)
        {
            if (this._upgradesList[i].source == source)
            {
                this._upgradesList.RemoveAt(i);
                hasBeenModified = true;
            }
        }

        if (hasBeenModified)
        {
            this._isDirty = true;
        }

        return this;
    }

    private float CalculateMaxValue()
    {
        float result = BaseMaxValue;
        foreach (Upgrade upgrade in this._upgradesList.OrderBy(u => u.type))
        {
            if (upgrade.type == UpgradeType.FLAT)
            {
                result += upgrade.upgradeValue;
            }
            else if (upgrade.type == UpgradeType.PERCENT)
            {
                result *= (upgrade.upgradeValue / 100f) + 1f;
            }
        }
        return result;
    }
    #endregion

    #region Operator Overloads
    public static implicit operator float(UpgradableClampedStat stat)
    {
        return stat.Value;
    }

    public static implicit operator int(UpgradableClampedStat stat)
    {
        return stat.ValueInt;
    }
    #endregion
}