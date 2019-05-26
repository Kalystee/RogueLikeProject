using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityController
{
    [Header("Behaviour Settings")]
    public NavMeshAgent agent;
    public Transform target;
    public float visionRange;
    public float rangeStop;

    [Header("Loot Settings")]
    public List<Weapon> itemLoot;
    public GameObject lootPrefab;

    CharacterController charactController;

    protected override void Awake()
    {
       base.Awake();
       charactController = GetComponent<CharacterController>();
           
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        float distanceToTarget = Vector3.Distance(target.position, transform.position);
        if(distanceToTarget <= visionRange)
        {
            agent.SetDestination(target.position);            
        }
    }

    
    protected override void OnDeath()
    {
        Debug.Log("Death");
        Transform spawnLoot = this.transform;

        foreach (Weapon loot in itemLoot)
        {
            spawnLoot.position = new Vector3(spawnLoot.position.x, spawnLoot.position.y, spawnLoot.position.z+0.5f);
            Debug.Log(loot);
            this.lootPrefab.GetComponent<WeaponDisplay>().weapon = loot;
            Debug.Log(lootPrefab);
            Instantiate(this.lootPrefab, this.transform);
        }

        Destroy(this.gameObject);
    }
}