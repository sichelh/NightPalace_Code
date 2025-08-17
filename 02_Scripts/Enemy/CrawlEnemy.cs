using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlEnemy : BaseEnemy
{
    // 감지 반경 시각화
    private void OnDrawGizmosSelected()
    {
        if (enemyStats == null) return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, enemyStats.detectRange);
    }
}
