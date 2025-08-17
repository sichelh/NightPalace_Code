    using System;
    using UnityEngine;

    // 오브젝트 풀링을 위한 인터페이스
    public interface IPoolable
    {
        // 오브젝트 풀에 의해 생성될 때 초기화, 반환 처리를 위한 Action을 전달 받음
        void Initialize(Action<GameObject> returnAction);

        // 오브젝트가 풀에서 꺼내져 활성화 될 때
        void OnSpawn();

        // 오브젝트가 비활성화 될 때
        void OnDespawn();

        event Action<GameObject> returnToPool;
        int PrefabIndex { get; set; }
}