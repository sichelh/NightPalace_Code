using UnityEngine;

public class PlayerBookState : PlayerUIState
{
    public override void Enter()
    {
        base.Enter();
        owner.CamShake.Shake(owner.PlayerStateData.GroundCamShake.Noise,
            owner.PlayerStateData.GroundCamShake.AmplitudeOnIdle,
            owner.PlayerStateData.GroundCamShake.FrequencyOnIdle);
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }
}
