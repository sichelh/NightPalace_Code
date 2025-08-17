using UnityEngine;

public class EnemyIdleState : BaseState<BaseEnemy>
{
    private float timer = 0f;
    private float wanderTime = 5f;

    public override void Enter()
    {
        owner.GetAnimator()?.SetBool(owner.Idle, true);
        owner.PlaySFXLoop(owner.idleClip, 3f);
    }

    public override void Update()
    {
        owner.StopMove();
        
        timer+=Time.deltaTime;
        
        // 플레이어 감지되면 Chase
        if (owner.IsPlayerInRage() || owner.IsSoundSensedRange())
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<EnemyChaseState>());
            return;
        }

        // 감염된 플레이어가 감지되면 Dance
        if (owner.IsInfectedPlayerInDanceRange() && !owner.IsPlayerInRage())
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<EnemyDanceState>());
        }

        // 플레이어 감지 안되면 Wander
        if (!owner.IsInfectedPlayerInDanceRange() && !owner.IsPlayerInRage() && timer >= wanderTime)
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<EnemyWanderState>());
            timer = 0;
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
        owner.GetAnimator()?.SetBool(owner.Idle, false);
        owner.StopSFXLoop();
    }

}
