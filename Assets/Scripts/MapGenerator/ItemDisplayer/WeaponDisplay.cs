﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDisplay : MonoBehaviour, IInteractable
{
    public Weapon weapon;
    public GameObject weaponPrefab;

    public void OnActivation(EntityController entity)
    {
        //TODO: appliquer les buffs
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (this.weapon is WeaponMelee)
            {
                WeaponMelee weaponMeleeToEquip = this.weapon as WeaponMelee;

                if (entity.entityProperties.weaponMelee != null)
                {
                    Debug.Log("Drop a "+ entity.entityProperties.weaponMelee.weaponName+" and get a "+this.weapon.weaponName);
                  
                    weaponPrefab.GetComponent<WeaponDisplay>().weapon = entity.entityProperties.weaponMelee;
                    Instantiate(weaponPrefab, this.transform.position, Quaternion.identity);
                }
                entity.entityProperties.weaponMelee = weaponMeleeToEquip;
               
            }else if(this.weapon is WeaponRange)
            {
                WeaponRange weaponRangeToEquip = this.weapon as WeaponRange;
                if (entity.entityProperties.weaponRange != null)
                {
                    Debug.Log("Drop a " + entity.entityProperties.weaponRange.weaponName + " and get a " + this.weapon.weaponName);
                    weaponPrefab.GetComponent<WeaponDisplay>().weapon = entity.entityProperties.weaponRange;
                    
                    Instantiate(weaponPrefab, this.transform.position, Quaternion.identity);
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

    //TODO : Temporary Method to test procedural generation of object
    private void FixedUpdate()
    {
        if( this.transform.position.y < 0)
        {
            Destroy(this.gameObject);
        }
    }
}

   