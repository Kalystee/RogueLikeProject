using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    IDLE,
    AGRESSIVE,
    ATTACK,
}

public class EnemyController : EntityController
{
    [Header("Agent Settings")]
    public NavMeshPath path;
    public Transform target;
    EnemyState state;

    [Header("Vision Settings")]
    
    public FieldOfView fov;
    float stopRange;
    bool hasSeeTarget = false;

    [Header("Loot Settings")]
    public List<Weapon> itemLoot;
    public GameObject lootPrefab;
    public GameObject moneyLootPrefab;
    public int moneyLoot;

    
    protected override void Awake()
    {
        base.Awake();
        path = new NavMeshPath();
        this.stopRange = this.entityProperties.weaponMelee.range;
        this.state = EnemyState.IDLE;
       
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
       
        if (state == EnemyState.IDLE)
        {
            if(fov.visibleTargets.Count > 0)
            {
                state = EnemyState.AGRESSIVE;
            }
            else
            {
                this.path.ClearCorners();
            }
        }
        else if(state == EnemyState.AGRESSIVE)
        {
            float distanceToTarget = Vector3.Distance(target.transform.position, this.transform.position);
            if (distanceToTarget <= stopRange)
            {
                this.state = EnemyState.ATTACK;
            }
            else if(fov.visibleTargets.Count == 0)
            {
                this.state = EnemyState.IDLE;
            }
            else 
            {
                NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path);
            }
        }
        else if(state == EnemyState.ATTACK)
        {
            EntityController entityTarget = target.GetComponent<EntityController>();
            this.UseWeapon();
            this.state = EnemyState.AGRESSIVE;
            if (entityTarget.entityProperties.Stats.Health <= 0)
            {
                this.state = EnemyState.IDLE;
            }
        }

    }
    
    protected override void UpdateMovementControl()
    {
        if (this.path.status != NavMeshPathStatus.PathInvalid && this.path.corners.Length > 1)
        {
            Vector3 direction = this.path.corners[1] - this.path.corners[0];
            direction.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, direction.normalized, 0.75f);

            MovementInput(Vector2.up);

            for (int i = 0; i < this.path.corners.Length - 1; i++)
                Debug.DrawLine(this.path.corners[i], this.path.corners[i + 1], Color.red);
        }
        else
        {
            MovementInput(Vector2.zero);
        }
    }

    protected override void OnDeath()
    {
        
        Instantiate(moneyLootPrefab, this.transform.position, Quaternion.identity);
        foreach(Weapon loot in itemLoot)
        {
            this.lootPrefab.GetComponent<WeaponDisplay>().weapon = loot;
            Instantiate(this.lootPrefab, this.transform.position,Quaternion.identity);
        }

        Destroy(this.gameObject);
    }
}
