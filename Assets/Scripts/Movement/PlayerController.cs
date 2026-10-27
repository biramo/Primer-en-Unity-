using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement), typeof(Look))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference sprintAction;

    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference rechargeAction;


    private Movement movement;
    private Look look;
    private Weapon weapon;

    private float horizontal;
    private float vertical;
    private bool jumpRequested;
    private bool isSprinting;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        look = GetComponent<Look>();
        weapon = GetComponentInChildren<Weapon>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
        shootAction.action.Enable();
        sprintAction.action.Enable();

    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();
        shootAction.action.Disable();
        sprintAction.action.Disable();

        Cursor.lockState=CursorLockMode.None;
        Cursor.visible=true;

    }

    private void Update()
    {
        Vector2 move = moveAction.action.ReadValue<Vector2>();
        horizontal = move.x;
        vertical = move.y;

        if (jumpAction.action.WasPressedThisFrame()) jumpRequested = true;
        if (shootAction.action.WasPressedThisFrame()) weapon.Shoot();
        if(rechargeAction.action.WasPressedThisFrame()) weapon.Recharge();
        isSprinting = sprintAction.action.IsPressed();
        look.Rotate(lookAction.action.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        movement.MoveEntity(horizontal, vertical, jumpRequested,isSprinting);
        jumpRequested = false;
    }
}