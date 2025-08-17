using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PoolManager : Singleton<PoolManager>
{
    // 풀링할 프리팹
    public GameObject[] prefabs;

    // int로 index를 사용해서 objectpool들을 Queue에 넣어서 관리한다.
    private Dictionary<int, Queue<GameObject>> pools = new Dictionary<int, Queue<GameObject>>();

    private void Awake()
    {
        // 각 프리팹 인덱스에 대한 큐 초기화
        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new Queue<GameObject>();
        }
    }

    public GameObject GetObject(int prefabIndex, Vector3 position, Quaternion rotation)
    {
        // 지정된 인덱스에 프리팹이 존재하지 않는 경우 출력
        if (!pools.ContainsKey(prefabIndex))
        {
            Debug.Log($"프리팹 인덱스 {prefabIndex}에 대한 풀이 존재하지 않습니다.");
            return null;
        }

        GameObject obj;
        //풀에 오브젝트가 있다면 꺼내기
        if (pools[prefabIndex].Count > 0)
        {
            obj = pools[prefabIndex].Dequeue();
        }
        else // 없으면 새로 생성 후 반환처리를 위한 초기화
        {
            obj = Instantiate(prefabs[prefabIndex]);
            obj.GetComponent<IPoolable>()?.Initialize(o => ReturnObject(prefabIndex, o));
        }
        
        // 예외처리 : false와 true 사이에 값 설정
        obj.SetActive(false);

        // 풀에서 꺼내서 오브젝트 설정
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        var poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
            poolable.PrefabIndex = prefabIndex;
            poolable.Initialize(o => ReturnObject(poolable.PrefabIndex, o));
            poolable.OnSpawn();
        }
        return obj;
    }

    public void ReturnObject(int prefabIndex, GameObject obj)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            // 풀에 없으면 그냥 삭제
            Destroy(obj);
            return;
        }

        // 풀에 있으면 비활성화 후 큐에 다시 넣어주기
        obj.SetActive(false);
        pools[prefabIndex].Enqueue(obj);
    }
}