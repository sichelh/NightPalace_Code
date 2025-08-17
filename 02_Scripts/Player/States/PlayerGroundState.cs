using UnityEngine;

public abstract class PlayerGroundState : BaseState<Player>
{
    protected float currentSpeed;   //GroundState의 현재 속도

    public override void Enter()
    {
        //GroundState 중 마우스 잠금
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Update()
    {
        Move();
        owner.PlayerCondition.IncreaseStaminaOnSprint();

        //Idle
        if (owner.Input.MoveInput == Vector2.zero)
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerIdleState>());
        }

        //걷기
        if (owner.Input.MoveInput != Vector2.zero && !owner.Input.SprintPressed && !owner.Input.CrouchPressed)
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerWalkState>());
        }

        //달리기
        if (owner.Input.MoveInput != Vector2.zero && owner.Input.SprintPressed && owner.PlayerCondition.stamina > 10f)
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerSprintState>());
        }

        //앉기
        if (owner.Input.CrouchPressed)
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerCrouchState>());
        }

        //점프
        if (owner.Controller.isGrounded && owner.Input.JumpPressed && owner.PlayerCondition.stamina > owner.PlayerStateData.Air.JumpStaminaDecrease)
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerJumpState>());
        }

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

        // 사망UI
        if (owner.PlayerCondition.hp < 0.1f)
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerDeadState>());
        }
    }

    public override void FixedUpdate() { }
    public override void LateUpdate() { PlayerUtility.Look(owner); }

    //GroundState에서의 이동
    protected void Move()
    {
        float groundedVerticalVelocity = -2f;   //중력 고정
        PlayerUtility.Move(owner, currentSpeed, groundedVerticalVelocity);
    }
}
