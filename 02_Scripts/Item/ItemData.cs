using System.Collections;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Usable,
    Grapable,
    Quest,
    None
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string itemDescrpition;
    public ItemType itemType;
    public ItemObject equipPrefab;
    public ItemObject dropPrefab;
    public Sprite icon;
}

public interface IUsable
{
    bool Use();
}

public interface IGetItemData
{
    string CurStat();
}