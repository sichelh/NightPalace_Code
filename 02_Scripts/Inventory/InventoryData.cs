using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryData : MonoBehaviour
{
	[SerializeField] private Dictionary<string, ItemObject> items = new();
    [SerializeField] private int slotCount = 20;
	[SerializeField] private string selectKey;
	[SerializeField] private string equipKey;
	[SerializeField] private Player player;

	//아이템을 장착하고 있는가? / 장착한 아이템 / 장착할 위치 => 역할 플레이어로 옮김.
	//[SerializeField] private bool isEquip = false;
	//[SerializeField] private GameObject equipItem;
	//[SerializeField] private Transform target;

    public Dictionary<string, ItemObject> Items
	{
		get { return items; }
		set { items = value; }
	}
	public string SelectKey { get => selectKey; set => selectKey = value; }
	public bool IsEquip { get; private set; }

	void Awake()
	{
		player = Player.Instance;
	}

	public void AddItem(string itemName, ItemData item)
	{
		if (items.ContainsKey(itemName))
		{
			items[itemName].stack += 1;
		}
		else
		{
            GameObject go = new GameObject("Item_" + itemName);
            ItemObject itemObj = go.AddComponent<ItemObject>();
            itemObj.Init(item);
            itemObj.isEquip = false;

            items.Add(itemName, itemObj);
			items[itemName].stack = 1;

            ////items.Add(itemName,item.equipPrefab.GetComponent<ItemObject>());
            //items.Add(itemName, new ItemObject(item));
            ////TODO : 프리팹 복사 방식 고려
            //items[itemName].isEquip = false;
        }
	}

    public void UpdateItem(ItemObject item)
    {
		items[equipKey] = item;
    }

    //패턴
    //아이템 추가, 삭제
    //장착, 장착해제,
    //사용, 버리기

    public void Equip()
	{
		if (player.IsEquip)
		{
			UnEquip(); //장착 중이라면 장착 해제
		}
		//하고 넣어준다.

		equipKey = selectKey; //장착한 키를 선택한 키로 변경.
		items[equipKey].isEquip = true;

		var itemData = items[equipKey].itemData;
		//ItemObject equipPrefab = itemData.equipPrefab;

		player.OnGrap(itemData);

		player.IsEquip = true;
	}

	public void UnEquip()
	{
		
		items[equipKey].isEquip = false; //장착되었던 키로 아이템 해제
		equipKey = null;

		Destroy(player.EquipItem);
		player.EquipItemData = null;

        player.IsEquip = false;
	}

	public void EquipItemUse()
	{
		// 아이템 Consume
		//items[selectKey].itemData
		if (Player.Instance.EquipItem.TryGetComponent<IUsable>(out IUsable item))
		{
			Debug.Log("장착한 아이템을 사용하셨습니다.");
			item.Use();
		}
	}

	public void UsableUse()
	{
		Debug.Log("아이템 사용시도");
        if (items[selectKey].itemData.equipPrefab.TryGetComponent<IUsable>(out IUsable item))
        {
            Debug.Log("사용됐습니다!");
            if (item.Use())
            {
                items[selectKey].stack -= 1;
				if (items[selectKey].stack <= 0)
				{
					items.Remove(selectKey);
                    selectKey = null;
                }
            }
        }
    }

	public void Throw() // 버리기
	{
		if (equipKey == selectKey) UnEquip(); //버리려는 아이템이 장착중이라면

		var item = items[selectKey];
        Instantiate(item, player.Target.position, Quaternion.identity);

		items.Remove(selectKey);

        selectKey = null;
    }
}