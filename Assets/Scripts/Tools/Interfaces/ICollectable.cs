using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    /* This define the objects that can be collected by the player
     * WITHOUT any need of interaction
     * eg.: Money, Powerups, ... */

    /// <summary>
    /// Called when the entity is being collected
    /// </summary>
    /// <param name="entity">The entity that collected the item</param>
    void OnBeingCollected(EntityController entity);
}
