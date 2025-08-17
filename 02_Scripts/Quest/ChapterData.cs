using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "chapter", menuName = "New chapter")]
public class ChapterData : ScriptableObject
{
    //public Bounds zone;
    public int stateCount;
    public List<string> state;
    public int maxStack;
}
