using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Chest : MonoBehaviour,IInteractable
{
    public GameObject weaponPrefab;
    public Weapon weapon;
    bool isOpen = false;

    public void OnActivation(EntityController entity)
    {
        if (!isOpen)
        {
            isOpen = true;
            weaponPrefab.GetComponent<WeaponDisplay>().weapon = weapon;
            GameObject clone = Instantiate(weaponPrefab, this.transform);
        }
    }

    public void OnEnterRange(EntityController entity)
    {
    }

    public void OnExitRange(EntityController entity)
    {
      
    }

    public void OnRange(EntityController entity)
    {
        
    }
}
