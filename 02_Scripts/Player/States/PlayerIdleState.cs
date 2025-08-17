using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public override void Enter()
    {
        base.Enter();
        currentSpeed = 0f;  //Idle상태 속도는 0

        owner.CamShake.Shake(owner.PlayerStateData.GroundCamShake.Noise,
            owner.PlayerStateData.GroundCamShake.AmplitudeOnIdle,
            owner.PlayerStateData.GroundCamShake.FrequencyOnIdle);

        Debug.Log("Enter Idle");
    }

    public override void Update()
    {
        base.Update();
    }

    public override void LateUpdate() { base.LateUpdate(); }

    public override void Exit() { Debug.Log("Exit Idle"); }
}
