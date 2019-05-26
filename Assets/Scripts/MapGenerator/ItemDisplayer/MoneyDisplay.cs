using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour,ICollectable
{
    public Item item;

    public void OnBeingCollected(EntityController entity)
    {
        entity.entityProperties.Money.Value += Mathf.RoundToInt(this.item.typedUpgrade.upgradeValue);
        Destroy(this.gameObject);
    }
}
