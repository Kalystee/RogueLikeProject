using UnityEngine;

using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Item", menuName ="Item/StatModifier")]
public class Item : ScriptableObject
{
    public string itemName;
    public TypedUpgrade typedUpgrade;
   
    

}
