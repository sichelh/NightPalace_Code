using UnityEngine;

public class PlayerDeadState : PlayerGroundState
{
    public override void Enter()
    {
        currentSpeed = 0f;

        Cursor.lockState = CursorLockMode.None;

        owner.CamShake.Shake(owner.PlayerStateData.GroundCamShake.Noise,
            owner.PlayerStateData.GroundCamShake.AmplitudeOnIdle,
            owner.PlayerStateData.GroundCamShake.FrequencyOnIdle);

        UIManager.Instance.ShowDeadUI();
        AudioManager.Instance.DeadBGMPlay();
        owner.PlayerSFX.DeadSFXPlay();
    }

    public override void Update() { }

    public override void LateUpdate() { }

    public override void Exit() { }
}
