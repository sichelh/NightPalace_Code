using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChapterManager : Singleton<ChapterManager>
{
    //TODO : UI에 챕터에 따라 Prompt 창 변경.
    //TODO : 클리어되는 존 지정.
    //노드 구조
    // ChapterManager => Zone TriggerEnter();
    //
    //[SerializeField] private 
    //[SerializeField] private ChapterBlock ChapterZonePrefab;
    //[SerializeField] private List<ChapterBlock> Chapters = new();
    [SerializeField] private TextMeshProUGUI promptText;

    [SerializeField] private int curChapter;
    [SerializeField] private List<ChapterData> chapterDatas = new();
    [SerializeField] private List<int> curStack = new();

    public void Awake()
    {
        promptText.text = chapterDatas[0].state[0];
        foreach(ChapterData chapterData in chapterDatas)
        {
            curStack.Add(0);
        }
    }

    //TODO : 받아온 인덱스를 기반으로 텍스트 출력
    public void AddIndexCount(int index, int nextIndex, int stateCount)
    {
        //TODO : 인덱스를 기반하여 들어가 있는 텍스트 불러오기
        if (curChapter > index) return;
        //Text = string[index]
        if (!QuestClear(index)) //해당 미션이 클리어해보지 않았다면,
        {
            curStack[index]++;
            Debug.Log(curStack[index]);
            if (curStack[index] == chapterDatas[index].maxStack)
            {
                promptText.text = chapterDatas[nextIndex].state[0];
                curChapter = nextIndex;
            }
        }
        //Chapters[index].ChangeState(stateCount);
        //TODO : 조건을 완료하면 changeState
        //Chapters[index].ChangeState(3);
    }

    public bool QuestClear(int index)
    {
        Debug.Log(index);
        Debug.Log(curStack[index] +" "+ chapterDatas[index].maxStack);
        if (curStack[index] == chapterDatas[index].maxStack)
        {
            return true;
        }
        return false;
    }
}

