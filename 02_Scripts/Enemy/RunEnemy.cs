using UnityEngine;

public class RunEnemy : BaseEnemy
{
    // 감지 반경 시각화
    private void OnDrawGizmosSelected()
    {
        if (enemyStats == null) return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, enemyStats.detectRange);
    }
}
