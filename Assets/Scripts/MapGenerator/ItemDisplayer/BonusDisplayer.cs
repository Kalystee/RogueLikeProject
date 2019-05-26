using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDisplayer : MonoBehaviour, ICollectable
{
    public Item item;

    public void OnBeingCollected(EntityController entity)
    {
        Debug.Log(item.itemName);
        entity.entityProperties.Stats.ApplyUpgrade(this.item.typedUpgrade);
        
        Destroy(this.gameObject);
    }
}
