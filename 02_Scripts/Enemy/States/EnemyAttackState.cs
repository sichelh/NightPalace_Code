using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackState : BaseState<BaseEnemy>
{
    private float timer = 0;
    private float coolTime = 0;
    
    private bool isAttacking = false;
    private bool isWaitingForNextAttack = false; // 공격 애니가 끝나면 쿨타임 돌도록
    
    public override void Enter()
    {
        owner.StopMove();
        coolTime = owner.enemyStats.attackCoolTime;
        timer = 0f;
        isAttacking = false;
        isWaitingForNextAttack = true;
        timer = coolTime;
    }

    public override void Update()
    {
        if (!(owner.IsAttackRange() || owner.IsDoorAttackRange())) // 공격 범위를 벗어났을 때
        {
            if (owner.IsPlayerInRage() || owner.IsSoundSensedRange()) // 적이 플레이어를 쫓아감
            {
                owner.StateMachine.ChangeState(owner.StateFactory.Get<EnemyChaseState>());
            }
            else // 플레이어 감지 안됐으므로 idle
            {
                owner.StateMachine.ChangeState(owner.StateFactory.Get<EnemyIdleState>());
            }
            return;
        }
       
        timer += Time.deltaTime;

        // 공격 중이 아니고, 쿨타임이 다 찼을때
        if (!isAttacking && timer >= coolTime)
        {
            owner.FaceTargetInstantly();
            AttackOnce();
            owner.PlaySFX(owner.attackClip);
        }
        
    }
    
    // 한번만 공격
    private void AttackOnce()
    {
        isAttacking = true;
        isWaitingForNextAttack = false;
        timer = 0f;
        
        // Idle 끄고 Attack 실행
        owner.GetAnimator()?.SetBool(owner.AttackIdle, false);
        owner.GetAnimator()?.SetBool(owner.Attack, true);
        owner.FaceTargetInstantly();
    }

    // 애니메이션 이벤트에서 공격 끝날 때 호출
    public void OnAttackAnimationEnd()
    {
        owner.GetAnimator()?.SetBool(owner.Attack, false);
        owner.GetAnimator()?.SetBool(owner.AttackIdle, true);
        isAttacking = false;
        isWaitingForNextAttack = true;
        timer = 0f;
    }


    public override void FixedUpdate()
    {
    }

    public override void LateUpdate()
    {
    }

    public override void Exit()
    {
        owner.GetAnimator()?.SetBool(owner.Attack, false);
        owner.GetAnimator()?.SetBool(owner.AttackIdle, false);
        owner.StopSFX();
    }
}