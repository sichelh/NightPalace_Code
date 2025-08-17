using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: Header("Inputs")]
    public Vector2 MoveInput { get; private set; }
    public Vector2 MouseInput { get; private set; }
    [field: SerializeField] public bool UsePressed { get; private set; }
    [field: SerializeField] public bool SprintPressed { get; private set; }
    [field: SerializeField] public bool CrouchPressed { get; private set; }
    [field: SerializeField] public bool JumpPressed { get; private set; }
    [field: SerializeField] public bool menuPressed;
    [field: SerializeField] public bool InventoryPressed { get; private set; }

    [field: Header("Look Settings")]
    [field: SerializeField][Range(0f, 0.5f)] public float MouseSensitivity;
    [field: SerializeField] public Transform CamContainer { get; private set; }

    [field: Header("Crouch Settings")]
    [field: SerializeField] public Transform StandCamPos { get; private set; }
    [field: SerializeField] public Transform CrouchCamPos { get; private set; }
    [field: SerializeField] public float StandHeight { get; set; }
    [field: SerializeField] public float CrouchHeight { get; private set; }
    [field: SerializeField] public LayerMask OverheadObstacleLayer { get; private set; }

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start()
    {
        CamContainer.position = StandCamPos.position;
    }

    //----------입력-----------//
    private void OnMove(InputValue value) { MoveInput = value.Get<Vector2>(); }
    private void OnLook(InputValue value) { MouseInput = value.Get<Vector2>(); }
    //private void OnUse(InputValue value) { UsePressed = value.isPressed; }
    private void OnSprint(InputValue value) { SprintPressed = value.isPressed; }
    private void OnCrouch(InputValue value) { CrouchPressed = !CrouchPressed; }
    private void OnJump(InputValue value) { JumpPressed = value.isPressed; }
    private void OnInteract(InputValue value)
    {
        if (value.isPressed) 
        { 
            if(player.IsGrap) player.Grap();
            player.Interaction.Interact(); 
        }
    }
    private void OnMenu(InputValue value)
    {
        menuPressed = !menuPressed;
    }

    private void OnInventory(InputValue value)
    {
        InventoryPressed = !InventoryPressed;
    }

    private void OnUse(InputValue value)
    {
        Debug.Log("입력됨");
        InventoryController.Instance.EquipItemUse();
    }
}
