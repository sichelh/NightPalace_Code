using UnityEngine;

public class EnemyWanderState : BaseState<BaseEnemy>
{
    public override void Enter()
    {
        owner.GetAnimator()?.SetBool(owner.Move, true);
        owner.WanderToNewPosition();
        owner.PlaySFXLoop(owner.wanderClip, 5f);
    }

    public override void Update()
    {
        // 플레이어 감지되면 바로 추적
        // TODO : 데시벨 감지 조건 추가 + DODO : 추가했습니당
        if (owner.IsPlayerInRage() || owner.IsSoundSensedRange())
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<EnemyChaseState>());
            return;
        }
        
        // 플레이어 감지 안되고 목적지에 도달했다면 Idle로 돌아가서 대기
        if (!owner.GetAgent().pathPending && owner.GetAgent().remainingDistance <= owner.GetAgent().stoppingDistance)
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<EnemyIdleState>());
        }

        // 감염된 플레이어가 감지되면 Dance
        if (owner.IsInfectedPlayerInDanceRange() && !owner.IsPlayerInRage())
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<EnemyDanceState>());
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
        owner.GetAnimator()?.SetBool(owner.Move, false);
        owner.StopSFXLoop();
    }
}