using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : ItemObject, IUsable
{
    private float value;

    public HealPack(ItemData itemData) : base(itemData)
    {
    }

    public bool Use()
    {
        value = 20;
        Player.Instance.PlayerCondition.Heal(value);
        return true;
    }
}