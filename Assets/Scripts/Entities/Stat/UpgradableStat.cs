using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using static Upgrade;

[Serializable]
public class UpgradableStat : BasicStat
{
    public UpgradableStat(float value) : base(value)
    {
        this._upgradesList = new List<Upgrade>();
        this._baseValue = value;
        this._isDirty = false;
    }

    [SerializeField] protected float _baseValue;
    [SerializeField] protected bool _isDirty;

    protected Action<float> _onBaseValueChanged;
    public void RegisterBaseValueChangedCallback(Action<float> callback)
    {
        _onBaseValueChanged += callback;
    }

    public void UnregisterBaseValueChangedCallback(Action<float> callback)
    {
        _onBaseValueChanged -= callback;
    }

    #region Public Getters/Setters
    public override float Value
    {
        get
        {
            if (_isDirty)
            {
                float newValue = CalculateValue();
                if (newValue != this._value)
                {
                    _onValueChanged?.Invoke(newValue);
                    this._value = newValue;
                }
                _isDirty = false;
            }
            return this._value;
        }
        set
        {
            BaseValue = value;
        }
    }

    public virtual float BaseValue
    {
        get
        {
            return this._baseValue;
        }
        set
        {
            if (value != this._baseValue)
            {
                this._onBaseValueChanged?.Invoke(value);
                this._baseValue = Mathf.Max(value, 0);
                this._isDirty = true;
            }
        }
    }

    public virtual int BaseValueInt
    {
        get
        {
            return Mathf.RoundToInt(this._baseValue);
        }
    }
    #endregion

    #region Upgrades
    [SerializeField] List<Upgrade> _upgradesList;
    
    public UpgradableStat AddUpgrade(Upgrade upgrade)
    {
        if (!this._upgradesList.Contains(upgrade))
        {
            this._upgradesList.Add(upgrade);
            this._isDirty = true;
        }

        return this;
    }

    public UpgradableStat RemoveUpgradesFrom(Upgrade upgrade)
    {
        if (this._upgradesList.Contains(upgrade))
        {
            this._upgradesList.Remove(upgrade);
            this._isDirty = true;
        }

        return this;
    }

    public UpgradableStat RemoveUpgradesFrom(object source)
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

    private float CalculateValue()
    {
        float result = BaseValue;
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
    public static implicit operator float(UpgradableStat stat)
    {
        return stat.Value;
    }

    public static implicit operator int(UpgradableStat stat)
    {
        return stat.ValueInt;
    }
    #endregion
}

public class Upgrade
{
    public UpgradeType type;
    public float upgradeValue;
    public object source;

    public Upgrade(UpgradeType type, float upgradeValue, object source)
    {
        this.type = type;
        this.upgradeValue = upgradeValue;
        this.source = source;
    }

    public enum UpgradeType
    {
        FLAT = 0,
        PERCENT = 5
    }
}
