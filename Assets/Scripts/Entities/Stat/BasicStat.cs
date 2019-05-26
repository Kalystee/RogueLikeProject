using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BasicStat
{
    public BasicStat(float value)
    {
        this._value = value;
    }

    protected Action<float> _onValueChanged;
    public void RegisterValueChangedCallback(Action<float> callback)
    {
        _onValueChanged += callback;
    }

    public void UnregisterValueChangedCallback(Action<float> callback)
    {
        _onValueChanged -= callback;
    }

    [SerializeField] protected float _value;

    #region Public Getters/Setters
    public virtual float Value
    {
        get
        {
            return this._value;
        }
        set
        {
            if (value != this._value)
            {
                this._onValueChanged?.Invoke(value);
                this._value = value;
            }
        }
    }

    public virtual int ValueInt
    {
        get
        {   
            return Mathf.RoundToInt(Value);
        }
    }
    #endregion

    #region Operator Overloads
    public static implicit operator float(BasicStat stat)
    {
        return stat.Value;
    }

    public static implicit operator int(BasicStat stat)
    {
        return stat.ValueInt;
    }
    #endregion
}
