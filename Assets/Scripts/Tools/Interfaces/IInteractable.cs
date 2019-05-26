using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    /* This define the objects that can be collected by the player
     * WITH need of interaction
     * eg.: Money, Powerups, ... */

    /// <summary>
    /// Called the first frame the entity is aiming at the object
    /// </summary>
    /// <param name="entity">The entity aiming at the object</param>
    void OnEnterRange(EntityController entity);
    
    /// <summary>
    /// Called every frame the entity is aiming at the object
    /// </summary>
    /// <param name="entity">The entity aiming at the object</param>
    void OnRange(EntityController entity);

    /// <summary>
    /// Called the first frame the entity is no longer aiming at the object
    /// </summary>
    /// <param name="entity">The entity aiming at the object</param>
    void OnExitRange(EntityController entity);

    /// <summary>
    /// Called when the entity interact with the object
    /// </summary>
    /// <param name="entity">The entity interacting with the object</param>
    void OnActivation(EntityController entity);
}
