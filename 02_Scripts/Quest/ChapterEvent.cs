using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChapterEvent : MonoBehaviour
{
    [Header("일회성")]
    [SerializeField] private bool onceChapterKey;
    [SerializeField] public int onceChapterIndex;
    [SerializeField] public int onceNextIndex;

    [Header("스택성")]
    [SerializeField] private bool chapterKey;
    [SerializeField] public int chapterIndex;
    [SerializeField] public int nextIndex;
    [SerializeField] public int chaptercurState;
    public string lockText { get; }
    public string unLockText { get; }

    public void OnceChapter()
    {
        if (onceChapterKey)
        ChapterManager.Instance.AddIndexCount(onceChapterIndex, onceNextIndex, chaptercurState);
    }

    public bool ChapterCheck()
    {
        return chapterKey;
    }

    public bool Event()
    {
        if(chapterKey) // 퀘스트가 있다면 이벤트 검사
        {
            return true;
        }
        return false;
    }
    
    public void AddEvent()
    {
        Debug.Log($"아니 ChapterIndex가 이상해요{chapterIndex}");
        ChapterManager.Instance.AddIndexCount(chapterIndex, nextIndex, chaptercurState);
    }

    public bool QuestClear()
    {
        return ChapterManager.Instance.QuestClear(chapterIndex);
    }
}
