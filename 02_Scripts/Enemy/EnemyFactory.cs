using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : Singleton<EnemyFactory>
{
    [System.Serializable] 
    public struct SpawnArea // 적 스폰 영역 데이터 저장하는 구조체
    {
        public int zoneId; // 존 구분
        public Vector3 center; // 스폰 영역 중심 좌표
        public Vector3 size; // 스폰 영역 크기
        public int spawnCount; // 스폰할 갯수
        public int[] spawnWeights; // 각 몬스터 프리팹의 스폰 비율
    }

    [SerializeField] private List<SpawnArea> spawnAreas; // 적 스폰 영역
    [SerializeField] private float spawnInterval; // 적 스폰 간격

    public float SpawnRadius { get; private set; }

    private int currentZoneId = 2; // 현재 플레이어가 머무는 존 ID
    private int[] index = { 0, 1 };

    // 존 ID에 따른 활성화할 존 목록 
    private readonly Dictionary<int, int[]> activeZone = new()
    {
        { 1, new[] { 1, 2, 4 } },
        { 2, new[] { 1, 2, 3, 4 } },
        { 3, new[] { 2, 3, 4} },
        { 4, new[] { 1, 2, 3, 4 } }
    };

    
    private List<int> lastactiveID = new List<int>(); // 이전 프레임의 활성 존ID
    private List<int> nowactiveID = new List<int>(); // 현재 프레임의 활성 존ID

    // 존별 적 리스트 캐싱
    private Dictionary<int, List<GameObject>> zoneEnemies = new();
    private void Start()
    {
        // 시작 시 현재 플레이어가 위치한 존 확인
        //currentZoneId = GetZoneByPlayerPos(Player.Instance.transform.position);
        currentZoneId = 2;
        nowactiveID = new List<int>(activeZone[currentZoneId]);
        lastactiveID = new List<int>(nowactiveID); // 초기 상태

        // 최초 스폰
        foreach (var id in nowactiveID)
        {
            SpawnEnemiesInZone(id);
            SetZoneEnemiesActive(id, true);
        }

        StartCoroutine(AutoSpawnLoop());
    }
    // 일정 시간마다 플레이어 존 검사 및 스폰
    private IEnumerator AutoSpawnLoop()
    {
        while (true)
        {
            int zoneId = GetZoneByPlayerPos(Player.Instance.gameObject.transform.position);
            
            if (zoneId != currentZoneId && zoneId != -1)
            {
                currentZoneId = zoneId;

                // 현재 활성화될 존 ID 리스트 계산
                nowactiveID = new List<int>(activeZone[currentZoneId]);

                // 새로 들어온 존들만 스폰 및 활성화
                foreach (var newId in nowactiveID)
                {
                    if (!lastactiveID.Contains(newId))
                    {
                        SpawnEnemiesInZone(newId);          // 새로 생성
                        SetZoneEnemiesActive(newId, true);  // 활성화
                    }
                }

                // 빠진 존은 비활성화
                foreach (var oldId in lastactiveID)
                {
                    if (!nowactiveID.Contains(oldId))
                    {
                        SetZoneEnemiesActive(oldId, false); // 비활성화
                    }
                }

                // 다음 비교를 위해 현재 리스트를 저장
                lastactiveID = new List<int>(nowactiveID);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 플레이어 위치 기준으로 속한 존 판별
    private int GetZoneByPlayerPos(Vector3 playerPos)
    {
        foreach (var area in spawnAreas)
        {
            if (Mathf.Abs(playerPos.x - area.center.x) <= area.size.x * 0.5f && Mathf.Abs(playerPos.z - area.center.z) <= area.size.z * 0.5f)
                return area.zoneId;
        }
        return -1;
    }
    // 현재 존 기준으로 적들을 스폰
    private void SpawnEnemiesInZone(int zoneId)
    {
        if (!zoneEnemies.ContainsKey(zoneId))
            zoneEnemies[zoneId] = new List<GameObject>();

        // 이미 생성된 적이 있다면 새로 생성 안 함
        if (zoneEnemies[zoneId].Count > 0) return;

        foreach (var area in spawnAreas)
        {
            if (area.zoneId != zoneId) continue;

            for (int i = 0; i < area.spawnCount; i++)
            {
                Vector3 spawnPos = GetRandomPositionInArea(area);
                Quaternion rot = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);
                int prefabIndex = GetRandomPrefabIndex(area);

                GameObject enemy = PoolManager.Instance.GetObject(prefabIndex, spawnPos, rot);
                if (enemy == null) continue;

                enemy.transform.position = spawnPos;
                enemy.transform.rotation = rot;
                enemy.SetActive(false); // 기본 비활성화

                zoneEnemies[zoneId].Add(enemy);

                var poolable = enemy.GetComponent<IPoolable>();
                if (poolable != null)
                {
                    poolable.Initialize(obj =>
                    {
                        zoneEnemies[zoneId].Remove(obj);
                        PoolManager.Instance.ReturnObject(poolable.PrefabIndex, obj);
                    });
                }
            }
        }
    }

    // 스폰 영역 내 랜덤 위치 생성
    private Vector3 GetRandomPositionInArea(SpawnArea area)
    {
        Vector3 half = area.size * 0.5f;
        return new Vector3(Random.Range(-half.x, half.x),0f,Random.Range(-half.z, half.z)) + area.center;
    }

    // 가중치 기반 프리팹 인덱스 선택
    private int GetRandomPrefabIndex(SpawnArea area)
    {
        if (area.spawnWeights.Length != index.Length) return 0;

        int totalWeight = 0;
        foreach (var weight in area.spawnWeights)
            totalWeight += weight;

        int rand = Random.Range(0, totalWeight);
        int cumulative = 0;

        for (int i = 0; i < area.spawnWeights.Length; i++)
        {
            cumulative += area.spawnWeights[i];
            if (rand < cumulative)
            {
                return index[i];
            }
        }

        return index[0]; 
    }
    // 해당 존에 있는 적을 활성/비활성화
    private void SetZoneEnemiesActive(int zoneId, bool isActive)
    {
        if (!zoneEnemies.ContainsKey(zoneId)) return;

        foreach (var enemy in zoneEnemies[zoneId])
        {
            if (enemy == null) continue;

            enemy.SetActive(isActive);

            if (isActive)
            {
                var poolable = enemy.GetComponent<IPoolable>();
                poolable?.OnSpawn();
            }
        }
    }

    // 에디터에서 스폰 영역 표시
    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;

        foreach (var area in spawnAreas)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawCube(area.center, area.size);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(area.center + Vector3.up * 1.5f, $"Zone {area.zoneId}\nSpawn: {area.spawnCount}");
#endif
        }
    }
}
