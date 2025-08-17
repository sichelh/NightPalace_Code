using UnityEngine;

public class PlayerInventoryState : PlayerUIState
{
    public override void Enter()
    {
        base.Enter();
        UIManager.Instance.ToggleInventoryUI();  // 상태 시작 - 인벤토리 보기

        owner.CamShake.Shake(owner.PlayerStateData.GroundCamShake.Noise,
            owner.PlayerStateData.GroundCamShake.AmplitudeOnIdle,
            owner.PlayerStateData.GroundCamShake.FrequencyOnIdle);

        Debug.Log("Enter Inventory");
    }

    public override void Update()
    {
        if (!owner.Input.InventoryPressed && UIManager.Instance.InventoryUI.gameObject.activeSelf)
        {
            UIManager.Instance.ToggleInventoryUI();
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerIdleState>());
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit Inventory");
    }
}
