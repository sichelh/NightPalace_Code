using UnityEngine;

public class PlayerInGameMenuState : PlayerUIState
{

    public override void Enter()
    {
        base.Enter();
        UIManager.Instance.ToggleMenuUI();  // 상태 시작 - 메인메뉴 보기

        owner.CamShake.Shake(owner.PlayerStateData.GroundCamShake.Noise,
            owner.PlayerStateData.GroundCamShake.AmplitudeOnIdle,
            owner.PlayerStateData.GroundCamShake.FrequencyOnIdle);

        Debug.Log("Enter InGameMenu");
    }

    public override void Update()
    {
        if (!owner.Input.menuPressed && UIManager.Instance.MenuUI.gameObject.activeSelf)
        {
            UIManager.Instance.ToggleMenuUI();
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerIdleState>());
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit InGameMenu");
    }
}
