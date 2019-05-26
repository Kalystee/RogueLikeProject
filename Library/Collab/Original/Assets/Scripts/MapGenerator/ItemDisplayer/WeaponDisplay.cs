﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDisplay : MonoBehaviour, IInteractable
{
    public Weapon weapon;
    public GameObject weaponPrefab;

    public void OnActivation(EntityController entity)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (this.weapon is WeaponMelee)
            {
                WeaponMelee weaponMeleeToEquip = this.weapon as WeaponMelee;

                if (entity.entityProperties.weaponMelee != null)
                {
                    Debug.Log("Drop a "+ entity.entityProperties.weaponMelee.weaponName+" and get a "+this.weapon.weaponName);
                  
                    weaponPrefab.GetComponent<WeaponDisplay>().weapon = entity.entityProperties.weaponMelee;
                    Instantiate(weaponPrefab, this.transform);
                }
                entity.entityProperties.weaponMelee = weaponMeleeToEquip;
               
            }else if(this.weapon is WeaponRange)
            {
                WeaponRange weaponRangeToEquip = this.weapon as WeaponRange;
                if (entity.entityProperties.weaponRange != null)
                {
                    Debug.Log("Drop a " + entity.entityProperties.weaponRange.weaponName + " and get a " + this.weapon.weaponName);
                    weaponPrefab.GetComponent<WeaponDisplay>().weapon = entity.entityProperties.weaponRange;
                    
                    Instantiate(weaponPrefab, this.transform);
                }
                
                entity.entityProperties.weaponRange = weaponRangeToEquip;

            }
            Destroy(this.gameObject);

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

   
