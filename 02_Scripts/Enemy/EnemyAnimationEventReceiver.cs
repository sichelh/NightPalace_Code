using UnityEngine;

public class EnemyAnimationEventReceiver : MonoBehaviour
{
    [SerializeField] private BaseEnemy enemy;

    private void Awake()
    {
        if (enemy == null)
            enemy = GetComponentInParent<BaseEnemy>();
    }

    // 공격 후 Idle 이벤트 호출
    public void OnAttackAnimationEnd()
    {
        enemy.OnAttackAnimationEnd();
    }

    // 공격 시 플레이어에게 데미지 주는 이벤트 호출
    public void OnAttackAnimationDamage()
    {
        enemy.OnAttackDamage();
    }
}
