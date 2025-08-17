using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
	[SerializeField] private InventoryData inventoryData;
    [SerializeField] private InventoryUI inventoryUI;

	public void AddItem(ItemData item)
	{
		//if (inventoryData == null || inventoryUI == null)
		//{
		//	Debug.LogError("InventoryData 또는 InventoryUI가 null입니다.");
		//	return;
		//}
		if (item == null) return;
        inventoryData.AddItem(item.itemName, item);
        inventoryUI.UpdateInventorySlots(inventoryData.Items);

        //if (inventoryData.Items == null)
        //{
        //	Debug.LogError("tems 가 null입니다.");
        //	return;
        //}
    }


    public void UpdateItem(ItemObject item)
	{
		inventoryData.UpdateItem(item);
	}

	public void SelectItem(string key)
	{
		var item = inventoryData.Items[key];
		var isActive = inventoryData.IsEquip; 

		inventoryData.SelectKey = key; //선택된 아이템 키.
		inventoryUI.SelectItem(item); //UI 아이템 설명 보이기.
	}
	public void SetEquipped(bool isActive = false)
	{
		if (Player.Instance.IsGrap) //그랩 중이라면 그랩 해제
		{
			Player.Instance.Grap();
		}

		if (isActive)
		{
			inventoryData.Equip(); //장착을 하고
		}
		else
		{
			inventoryData.UnEquip();
		}

		UpdateUI();
	}

	public void EquipItemUse()
	{
		inventoryData.EquipItemUse();
		UpdateUI();
	}

	public void UsableUse()
	{
        inventoryData.UsableUse();
        UpdateUI();
    }

	public void Throw()
	{
		inventoryData.Throw();

		UpdateUI();
	}

	public void UpdateUI()
	{
		string key = inventoryData.SelectKey;
		ItemObject item = null;
		if (key != null)
		{
			item = key != null ? inventoryData.Items[key] : null;
		}
        inventoryUI.SelectItem(item); //선택화면 갱신
        inventoryUI.UpdateInventorySlots(inventoryData.Items); //인벤토리 슬롯 갱신
    }
}