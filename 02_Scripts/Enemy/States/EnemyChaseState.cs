using UnityEngine;

public class EnemyChaseState : BaseState<BaseEnemy>
{
    public override void Enter()
    {
        owner.GetAnimator()?.SetBool(owner.Move, true);
        owner.PlaySFXLoop(owner.chaseClip, 5f);
    }

    public override void Update()
    {
        owner.MoveToPlayer();

        if (owner.IsAttackRange() || owner.IsDoorAttackRange()) // 공격 범위까지 쫓아왔을때
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<EnemyAttackState>());
        }

        if (!owner.IsPlayerInRage()) // 감지 범위 밖으로 벗어났을때
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<EnemyIdleState>());
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

    // 공격하는 적은 플레이어 위치로 이동
    private void MoveToPlayer()
    {
        Transform playerTransform = Player.Instance.gameObject.transform;
        if (playerTransform.position != null)
        {
            Vector3 dir = (playerTransform.position - owner.transform.position).normalized;
            owner.transform.position += dir * 3f * Time.deltaTime;
            if (dir.sqrMagnitude > 0) // 같은 위치 인지 체크
            {
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }
}