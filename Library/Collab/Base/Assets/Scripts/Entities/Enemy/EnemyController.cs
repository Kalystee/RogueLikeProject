using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityController
{
    public NavMeshAgent agent;
    public float visionRange;
    public List<Weapon> itemLoot;
    public GameObject lootPrefab;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, visionRange,Vector3.one,11);
        
        foreach(RaycastHit hit in hits)
        {
            if (hit.transform.name == "Player")
            {
                this.agent.SetDestination(hit.transform.position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, visionRange);
    }

    protected override void OnDeath()
    {
        Debug.Log("Death");
        foreach(Weapon loot in itemLoot)
        {
            this.lootPrefab.GetComponent<WeaponDisplay>().weapon = loot;
            Instantiate(this.lootPrefab, this.transform.position,Quaternion.identity);
        }

        Destroy(this.gameObject);
    }
}
