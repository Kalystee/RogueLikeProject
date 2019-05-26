using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClampedStat : BasicStat
{
    public ClampedStat(float maxValue, float minValue = 0) : base(maxValue)
    {
        this._maxValue = maxValue;
        this._minValue = minValue;
        this._value = maxValue;
    }

    protected Action<float> _onMinValueChanged;
    public void RegisterMinValueChangedCallback(Action<float> callback)
    {
        _onMinValueChanged += callback;
    }

    public void UnregisterMinValueChangedCallback(Action<float> callback)
    {
        _onMinValueChanged -= callback;
    }

    protected Action<float> _onMaxValueChanged;
    public void RegisterMaxValueChangedCallback(Action<float> callback)
    {
        _onMaxValueChanged += callback;
    }

    public void UnregiserMaxValueChangedCallback(Action<float> callback)
    {
        _onMaxValueChanged -= callback;
    }

    [SerializeField] protected float _maxValue;
    [SerializeField] protected float _minValue;

    #region Public Getters/Setters
    public override float Value
    {
        get
        {
            return this._value;
        }
        set
        {
            if(value != Value)
            {
                this._value = Mathf.Clamp(value, MinValue, MaxValue);
                this._onValueChanged?.Invoke(Value);
            }
        }
    }

    public virtual float MinValue
    {
        get
        {
            return this._minValue;
        }
        set
        {
            if(value != MinValue)
            {
                this._minValue = value;
                this._onMinValueChanged?.Invoke(value);
                if(Value < value)
                {
                    Value = value;
                }
            }
        }
    }

    public virtual float MaxValue
    {
        get
        {
            return this._maxValue;
        }
        set
        {
            if (value != MaxValue)
            {
                this._maxValue = value;
                this._onMaxValueChanged?.Invoke(value);
                if (Value > value)
                {
                    Value = value;
                }
            }
        }
    }
    #endregion
    
    #region Operator Overloads
    public static implicit operator float(ClampedStat stat)
    {
        return stat.Value;
    }

    public static implicit operator int(ClampedStat stat)
    {
        return stat.ValueInt;
    }
    #endregion
}
