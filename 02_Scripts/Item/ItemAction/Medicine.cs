using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : ItemObject, IUsable
{
    private float value = 20;

    public Medicine(ItemData itemData) : base(itemData)
    {
    }

    public bool Use()
    {
        value = 20;

        Player.Instance.PlayerCondition.DecreaseInfection(value);
        return true;
    }
}