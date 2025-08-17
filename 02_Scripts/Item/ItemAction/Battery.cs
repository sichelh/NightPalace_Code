using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bettery : ItemObject, IUsable
{
    private float value; // 처음에 ItemObject로 생성해서 값이 없는거야

    public Bettery(ItemData itemData) : base(itemData)
    {
    }

    public bool Use()
    {
        //손전등 게이지 
        value = 20;

        Debug.Log($"아이템 수치 {value}");
        if (Player.Instance.EquipItem == null) return false;

        if(Player.Instance.EquipItem.TryGetComponent<FlashLight>(out FlashLight light))
        {
            Debug.Log($"배터리 충전전{light.battery}");
            light.battery = Mathf.Min(light.battery + value, light.maxBattery);
            Debug.Log($"배터리 충전됨{light.battery}");
            InventoryController.Instance.UpdateItem(light);
            return true;
        }
        return false;
    }
}