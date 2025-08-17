using UnityEngine;

public abstract class PlayerAirState : BaseState<Player>
{
    protected float currentSpeed;
    protected float verticalVelocity;
    private readonly float _gravity = -9.8f;

    public override void Enter()
    {
        //AirState 중 마우스 잠금
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Update()
    {
        Move();

        // 땅에 닿았는지 확인해서 상태 전환
        if (owner.Controller.isGrounded)
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerIdleState>());

            // 인게임메뉴
            if (owner.Input.menuPressed)
            {
                owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerInGameMenuState>());
            }

            // 인벤토리UI
            if (owner.Input.InventoryPressed)
            {
                owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerInventoryState>());
            }
        }
    }

    public override void FixedUpdate() { }

    public override void LateUpdate() { PlayerUtility.Look(owner); }

    //AirState에서의 이동
    protected void Move()
    {
        verticalVelocity += _gravity * Time.deltaTime;
        currentSpeed = owner.Input.SprintPressed ? owner.PlayerStateData.Ground.SprintSpeed : owner.PlayerStateData.Ground.WalkSpeed;
        PlayerUtility.Move(owner, currentSpeed, verticalVelocity);
    }
}
