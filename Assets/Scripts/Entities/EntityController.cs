using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponTypeSelected
{
    NONE,
    MELEE,
    RANGED
}

[RequireComponent(typeof(CharacterController))]
public class EntityController : MonoBehaviour
{
    [Header("Entity Settings")]
    [SerializeField] public EntityProperties entityProperties = new EntityProperties();

    [Header("Advanced Entity Settings")]
    [SerializeField] private float accelerationFactor = 0.35f;
    [SerializeField] private float inAirAccelerationFactor = 0.02f;
    [SerializeField] private LayerMask groundLayers;

    [Header("Weapons Settings")]
    [SerializeField] private GameObject meleeTransform;
    [SerializeField] private GameObject rangedTransform;

    public float JumpForce { get { return Mathf.Sqrt(entityProperties.Stats.JumpHeight * -2f * Physics.gravity.y); } }
    private CharacterController c_characterController;
    protected Vector3 _velocity;
    private int avalaibleJump;

    [SerializeField] private WeaponTypeSelected _selectedWeapon = WeaponTypeSelected.NONE;
    [SerializeField] protected Weapon currentWeapon;
    public WeaponTypeSelected SelectedWeapon
    {
        get
        {
            return _selectedWeapon;
        }
        set
        {
            if(_selectedWeapon != value)
            {
                _selectedWeapon = value;
                switch(SelectedWeapon)
                {
                    case WeaponTypeSelected.NONE:
                        currentWeapon = null;
                        break;

                    case WeaponTypeSelected.MELEE:
                        currentWeapon = entityProperties.weaponMelee;
                        break;

                    case WeaponTypeSelected.RANGED:
                        currentWeapon = entityProperties.weaponRange;
                        break;
                }
                RefreshWeaponDisplayed();
            }
        }
    }

    protected virtual void Awake()
    {
        avalaibleJump = entityProperties.Stats.JumpQuantity;
        c_characterController = GetComponent<CharacterController>();
        this.entityProperties.Stats.Health.RegisterValueChangedCallback(
            delegate(float health) {
                if (health == 0f)
                    OnDeath();
            });

        switch (SelectedWeapon)
        {
            case WeaponTypeSelected.NONE:
                currentWeapon = null;
                break;

            case WeaponTypeSelected.MELEE:
                currentWeapon = entityProperties.weaponMelee;
                break;

            case WeaponTypeSelected.RANGED:
                currentWeapon = entityProperties.weaponRange;
                break;
        }
        RefreshWeaponDisplayed();
    }
    
    protected void MovementInput(Vector2 movementInput, bool sprinting = false)
    {
        Vector3 moveInput = (transform.forward * movementInput.y + transform.right * movementInput.x).normalized * (sprinting ? entityProperties.Stats.SprintMultiplier : 1) * entityProperties.Stats.MoveSpeed;
        Vector3 targetVelocity = new Vector3(moveInput.x, _velocity.y, moveInput.z);

        _velocity = Vector3.Lerp(_velocity, targetVelocity, CheckGround() ? accelerationFactor : inAirAccelerationFactor);
    }

    protected virtual void OnDeath()
    {
        Destroy(this.gameObject);
    }

    protected void Knockback(Vector3 knockbackForce)
    {
        _velocity += knockbackForce / entityProperties.Stats.KnockbackResistance;
    }

    protected void Jump()
    {
        if(avalaibleJump > (CheckGround() ? 0 : 1))
        {
            _velocity.y = JumpForce;
            avalaibleJump--;
        }
    }

    protected virtual void Update()
    {
        if (!CheckGround())
        {
            _velocity.y += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            if (_velocity.y < 0f)
            {
                _velocity.y = 0f;
                if (avalaibleJump != entityProperties.Stats.JumpQuantity)
                {
                    avalaibleJump = entityProperties.Stats.JumpQuantity;
                }
            }
        }

        c_characterController.Move(_velocity * Time.deltaTime);

        if (timeBeforeUsingWeapon > 0)
            timeBeforeUsingWeapon -= Time.deltaTime;

        UpdateMovementControl();

        if(alwaysHit)
        {
            UseWeapon();
        }
    }

    protected virtual void UpdateMovementControl()
    {
        MovementInput(Vector2.zero);
    }

    protected bool CheckGround()
    {
        Ray ray = new Ray(transform.position + Vector3.down * (c_characterController.height / 2 - c_characterController.radius), Vector3.down);
        return Physics.SphereCast(ray, c_characterController.radius, 0.1f, groundLayers);
    }

    #region Weapon Control Management
    private void RefreshWeaponDisplayed()
    {
        meleeTransform.SetActive(SelectedWeapon == WeaponTypeSelected.MELEE);
        rangedTransform.SetActive(SelectedWeapon == WeaponTypeSelected.RANGED);
    }

    private float timeBeforeUsingWeapon = 0;
    protected virtual Vector3 aimDirection
    {
        get
        {
            return transform.forward;
        }
    }

    protected void UseWeapon()
    {
        if (timeBeforeUsingWeapon <= 0)
        {
            if (SelectedWeapon == WeaponTypeSelected.NONE)
                return;

            if (SelectedWeapon == WeaponTypeSelected.MELEE)
                SwingMeleeWeapon();

            if (SelectedWeapon == WeaponTypeSelected.RANGED)
                FireRangedWeapon();

            timeBeforeUsingWeapon = currentWeapon.delayBetweenUses;
        }
    }

    private void FireRangedWeapon()
    {

    }

    private void SwingMeleeWeapon()
    {
        Vector3 center = transform.position + aimDirection * 0.5f;
        Vector3 halfExtends = new Vector3(0.5f, 0.5f, 0.05f);

        Vector3 speedKnockback = _velocity;
        if(Mathf.Sign(speedKnockback.x) != Mathf.Sign(transform.forward.x))
        {
            speedKnockback.x = 0;
        }
        if (Mathf.Sign(speedKnockback.z) != Mathf.Sign(transform.forward.z))
        {
            speedKnockback.z = 0;
        }

        RaycastHit[] castResult = Physics.BoxCastAll(center, halfExtends, aimDirection, Quaternion.LookRotation(aimDirection, Vector3.up), currentWeapon.range);

        foreach(RaycastHit hit in castResult)
        {
            if(hit.transform != null)
            {
                EntityController entityController = hit.transform.GetComponent<EntityController>();
                if (entityController != null && entityController != this)
                {
                    entityController.entityProperties.Stats.Health.Value -= entityProperties.weaponMelee.damage + entityProperties.Stats.Damage;
                    Debug.Log(entityController.entityProperties.Stats.Health.Value);
                    entityController.Knockback(aimDirection * currentWeapon.knockbackPower + Vector3.up * 2.5f + speedKnockback);
                }
            }
        }
    }
    #endregion

    [Header("DEBUG ONLY")]
    [SerializeField] private bool alwaysHit = false;
}
