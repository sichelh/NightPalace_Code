using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterBlock : MonoBehaviour
{
    [SerializeField] private BoxCollider collider;
    [SerializeField] private int chapterIndex = 0;
    [SerializeField] private int nextIndex = 0;
    [SerializeField] private int stateCount = 0;

    public void SetIndex(int index)
    {
        this.chapterIndex = index;
    }

    public void Initialize(Vector3 center, Vector3 size)
    {
        transform.position = center;
        this.collider.size = size;
    }

    public void ChangeState(int stateCount)
    {
        this.stateCount = stateCount;
    }

    public void OnTriggerEnter(Collider other)
    {
        //TODO : ChapterManager에 해당 인덱스를 전달
        ChapterManager.Instance.AddIndexCount(this.chapterIndex, nextIndex, stateCount);
        gameObject.SetActive(false); //한번 사용된 지점에 대해서는 false 처리. 
                                        //특정 메서드와 연관지어 다시 활성화 시킬 수도 있음.
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawCube(gameObject.transform.position, collider.size);
    }
}
