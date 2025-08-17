using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public override void Enter()
    {
        base.Enter();
        verticalVelocity = owner.PlayerStateData.Air.JumpForce;
        owner.PlayerCondition.DecreaseStaminaOnJump();

        owner.CamShake.Shake(owner.PlayerStateData.GroundCamShake.Noise,
    owner.PlayerStateData.GroundCamShake.AmplitudeOnJump,
    owner.PlayerStateData.GroundCamShake.FrequencyOnJump);

        owner.PlayerSFX.JumpSFXPlay();
        Debug.Log("Enter Jump");
    }

    public override void Update()
    {
        base.Update();
    }

    public override void LateUpdate() { base.LateUpdate(); }

    public override void Exit() { Debug.Log("Exit Jump"); }
}
