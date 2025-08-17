using UnityEngine;

public abstract class PlayerUIState : BaseState<Player>
{
    public override void Enter()
    {
        //UIState 중에 마우스 키기
        Cursor.lockState = CursorLockMode.None;
    }

    public override void Update()
    {
        owner.PlayerCondition.IncreaseStaminaOnSprint();

        if(owner.Input.menuPressed && UIManager.Instance.MenuUI.gameObject.activeSelf)
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerIdleState>());
        }

        if (owner.Input.InventoryPressed && UIManager.Instance.MenuUI.gameObject.activeSelf)
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerIdleState>());
        }
    }

    public override void FixedUpdate() { }
    public override void LateUpdate() { }
    public override void Exit() { }
}
