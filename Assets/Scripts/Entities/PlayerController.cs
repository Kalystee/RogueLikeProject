using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : EntityController
{
    protected override void Awake()
    {
        base.Awake();

        AwakeCamera();
    }

    protected override void Update()
    {
        base.Update();
        
        UpdateCameraControl();
        UpdateInteraction();
        UpdateCollectable();
        UpdateWeaponControl();
        UpdateUI();
    }

    #region Movement
    protected override void UpdateMovementControl()
    {
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MovementInput(movementInput, Input.GetKey(KeyCode.LeftShift));

        if (Input.GetKeyDown(unlockForwardLookKey))
            keepLookingForward = !keepLookingForward;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.X))
            Knockback(new Vector3(Random.Range(-1, 2) * 5f, 10f, Random.Range(-5f, 5f)));
    }
    #endregion

    #region CameraControl
    [Header("Objects")]
    [SerializeField] GameObject cameraAxis;
    [SerializeField] GameObject cameraObject;
    [SerializeField] LayerMask cameraCollisionMask;

    [Header("Camera Rotation Settings")]
    [SerializeField] float verticalSensibility = 1f;
    [SerializeField] float horizontalSensibility = 1f;
    [SerializeField] float swivelSpeed = 5f;
    [SerializeField] float minimumRotationX = 0f;
    [SerializeField] float maximumRotationX = 85f;
    [SerializeField] KeyCode unlockForwardLookKey = KeyCode.P;
    bool keepLookingForward = true;

    [Header("Zoom Settings")]
    [SerializeField] float zoomSensibility = 1.5f;
    [SerializeField] float minimumZoom = 0.5f;
    [SerializeField] float maximumZoom = 7.5f;

    [SerializeField] bool invertedYRotation = true;

    [Header("Camera Settings")]
    [SerializeField] float cameraAdjustementSpeed = 5f;
    [SerializeField] float verticalOffset = 1.5f;
    [SerializeField] float horizontalOffset = 0.75f;
    [SerializeField] CameraSide cameraSide = CameraSide.RIGHT;
    [SerializeField] KeyCode cameraSideSwitchButton = KeyCode.C;

    private float _rotationX = 0;

    float currentZoom;

    public enum CameraSide
    {
        RIGHT,
        LEFT,
        CENTER
    }

    private void AwakeCamera()
    {
        base.Awake();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (cameraAxis == null || cameraObject == null)
        {
            Debug.LogError("An element for 'CameraController' is missing!");
            this.enabled = false;
            return;
        }

        currentZoom = cameraObject.transform.localPosition.z;
        _rotationX = cameraAxis.transform.eulerAngles.x;
    }

    protected override Vector3 aimDirection
    {
        get
        {
            return new Vector3(transform.forward.x, cameraAxis.transform.forward.y, transform.forward.z);
        }
    }

    private void UpdateCameraControl()
    {
        if (Input.GetKeyDown(cameraSideSwitchButton))
        {
            cameraSide = (cameraSide == CameraSide.RIGHT) ? CameraSide.LEFT : CameraSide.RIGHT;
        }
        
        if (keepLookingForward)
        {
            Quaternion cameraRotation = cameraAxis.transform.localRotation;
            Quaternion targetRotation = Quaternion.Euler(cameraAxis.transform.eulerAngles.x, 0, 0);
            cameraAxis.transform.localRotation = Quaternion.Lerp(cameraRotation, targetRotation, Time.deltaTime * swivelSpeed);
            transform.Rotate(0, Input.GetAxis("Mouse X") * horizontalSensibility, 0);
        }
        else
        {
            cameraAxis.transform.Rotate(0, Input.GetAxis("Mouse X") * horizontalSensibility, 0);
        }

        _rotationX -= Input.GetAxis("Mouse Y") * verticalSensibility;
        _rotationX = Mathf.Clamp(_rotationX, minimumRotationX, maximumRotationX);
        float rotationY = cameraAxis.transform.localEulerAngles.y;
        cameraAxis.transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);

        //Gestion du zoom
        Vector3 cameraPosition = cameraObject.transform.localPosition;
        currentZoom = Mathf.Clamp(currentZoom + Input.mouseScrollDelta.y, -maximumZoom, -minimumZoom);
        cameraPosition.y = verticalOffset;
        if (cameraSide != CameraSide.CENTER)
        {
            cameraPosition.x = (cameraSide == CameraSide.RIGHT ? 1 : -1) * horizontalOffset;
        }
        else
        {
            cameraPosition.x = 0;
        }

        if (Physics.Raycast(transform.position, (cameraObject.transform.position - transform.position).normalized, out RaycastHit hit, -currentZoom, cameraCollisionMask))
        {
            cameraPosition.z = Mathf.Clamp(-hit.distance + 0.5f, -maximumZoom, -minimumZoom);
        }
        else
        {
            cameraPosition.z = currentZoom;
        }

        cameraObject.transform.localPosition = Vector3.Lerp(cameraObject.transform.localPosition, cameraPosition, Time.deltaTime * cameraAdjustementSpeed);
    }
    #endregion

    #region Interaction
    private IInteractable lastInteractable = null;

    [Header("Interaction")]
    [SerializeField] float interactionRange = 6.5f;
    [SerializeField] KeyCode interactionKey = KeyCode.E;

    private void UpdateInteraction()
    {
        Ray ray = new Ray(cameraObject.transform.position, cameraObject.transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, interactionRange);

        if (hit.transform != null)
        {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();
            if (lastInteractable != interactable)
            {
                interactable?.OnEnterRange(this);
                lastInteractable?.OnExitRange(this);

                lastInteractable = interactable;
            }
            if (interactable != null)
            {
                interactable.OnRange(this);

                if (Input.GetKeyDown(interactionKey))
                {
                    interactable.OnActivation(this);
                }
            }
        }
        else
        {
            lastInteractable?.OnExitRange(this);
            lastInteractable = null;
        }
    }
    #endregion

    #region Collectable
    [Header("Collectable Settings")]
    [SerializeField] private UpgradableStat collectablePickupRange = new UpgradableStat(1.2f);
    [SerializeField] private UpgradableStat collectableMagnetRange = new UpgradableStat(5f);
    [SerializeField] private UpgradableStat collectableMagnetPower = new UpgradableStat(1.5f);
    [SerializeField] private LayerMask collectableLayer;

    public void UpdateCollectable()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, collectableMagnetRange, Vector3.one, 0, collectableLayer);

        foreach(RaycastHit hit in hits)
        {
            if(hit.transform != null)
            {
                ICollectable collectable = hit.transform.GetComponent<ICollectable>();
                if (collectable != null)
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance < collectablePickupRange)
                    {
                        collectable.OnBeingCollected(this);
                    }
                    else
                    {
                        hit.transform.position = Vector3.Lerp(hit.transform.position, transform.position, Time.deltaTime * collectableMagnetRange / distance * collectableMagnetPower / 2);
                    }
                }
            }
        }
    }
    #endregion

    #region Weapon
    private void UpdateWeaponControl()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedWeapon = WeaponTypeSelected.MELEE;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectedWeapon = WeaponTypeSelected.RANGED;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectedWeapon = WeaponTypeSelected.NONE;
        }

        if(Input.GetMouseButtonDown(0))
        {
            UseWeapon();
        }
    }
    #endregion

    #region UI

    [Header("UI Settings")]
    public TextMeshProUGUI moneyTextUI;
    public Image healthBar;

    private void UpdateUI()
    {
        moneyTextUI.text = "Money : " + this.entityProperties.Money;
    
        this.healthBar.fillAmount = (float)this.entityProperties.Stats.Health / (float)this.entityProperties.Stats.Health.MaxValue; ;
    }
    #endregion
}
