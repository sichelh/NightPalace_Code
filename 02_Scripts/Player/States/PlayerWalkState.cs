using System.Collections;
using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
    public override void Enter()
    {
        base.Enter();
        currentSpeed = owner.PlayerStateData.Ground.WalkSpeed;

        owner.CamShake.Shake(owner.PlayerStateData.GroundCamShake.Noise
            , owner.PlayerStateData.GroundCamShake.AmplitudeOnWalk,
            owner.PlayerStateData.GroundCamShake.FrequencyOnWalk);
        owner.StartParticle(owner.walkParticleHash);

        Debug.Log("Enter Walk");
    }

    public override void Update()
    {
        base.Update();
        owner.FootStepSFX.FootStepSFXPlay();
    }

    public override void LateUpdate() { base.LateUpdate(); }

    public override void Exit() 
    { 
        Debug.Log("Exit Walk");
        owner.StopParticle();
    }
}
