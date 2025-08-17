using UnityEngine;

public class PlayerSprintState : PlayerGroundState
{
    public override void Enter()
    {
        base.Enter();
        currentSpeed = owner.PlayerStateData.Ground.SprintSpeed;

        owner.CamShake.Shake(owner.PlayerStateData.GroundCamShake.Noise,
            owner.PlayerStateData.GroundCamShake.AmplitudeOnSprint,
            owner.PlayerStateData.GroundCamShake.FrequencyOnSprint);
        owner.StartParticle(owner.runParticleHash);
        owner.StartParticle(owner.breathParticleHash);

        Debug.Log("Enter Sprint");
    }

    public override void Update()
    {
        Move();

        owner.PlayerCondition.DecreaseStaminaOnSprint();
        owner.FootStepSFX.FootStepSFXPlay();

        //Idle
        if (!owner.Input.SprintPressed || owner.PlayerCondition.stamina < 0.1f)
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerWalkState>());
        }

        // 달리는 중 점프
        if (owner.Input.JumpPressed && owner.PlayerCondition.stamina > owner.PlayerStateData.Air.JumpStaminaDecrease)
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
    }

    public override void LateUpdate() { base.LateUpdate(); }

    public override void Exit() 
    { 
        Debug.Log("Exit Sprint"); 
        owner.StopParticle();
        owner.StartParticle(owner.breathOnceParticleHash);
    }
}
