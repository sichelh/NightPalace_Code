using UnityEngine;

public class EnemyDanceState : BaseState<BaseEnemy>
{
    public override void Enter()
    {
        owner.GetAnimator()?.SetBool(owner.Dance, true);
        owner.PlaySFXLoop(owner.danceClip, 5f);
    }

    public override void Update()
    {
        owner.StopMove();
        // 플레이어 감지되면 Chase
        if (owner.IsPlayerInRage())
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<EnemyChaseState>());
            return;
        }

        // 플레이어 감지 안되고 범위 밖으로 나가면 Wander
        if (!owner.IsInfectedPlayerInDanceRange())
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<EnemyWanderState>());
        }
    }

    public override void FixedUpdate()
    {
        
    }

    public override void LateUpdate()
    {
        
    }

    public override void Exit()
    {
        owner.GetAnimator()?.SetBool(owner.Dance, false);
        owner.StopSFXLoop();
    }
}
