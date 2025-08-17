using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    //UI 프리팹
    [SerializeField] private Transform slotParentPos;
    [SerializeField] private SlotUI slotUIPrefab;

    [SerializeField] private Dictionary<string, SlotUI> slotUIs = new();

    //인벤토리 설명창
    [SerializeField] private Image itemIcon;
    [SerializeField] private Sprite itemDefaultIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI curStatText;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unEquipButton;
    [SerializeField] private Button useButton;
    [SerializeField] private Button throwButton;

    //슬롯에 들어갈 내용 + UpdateUI에 Index 포함.
    //public TextMeshProUGUI ItemText { get; }
    //public TextMeshProUGUI ItemValue { get; }
    //public Image Icon { get; }

    private void OnEnable()
    {
        TypeCheck(ItemType.None);
        AudioManager.Instance.UIOpenAndCloseClipPlay();
        itemIcon.sprite = itemDefaultIcon;
        itemNameText.text = string.Empty;
        itemDescriptionText.text = string.Empty;
        curStatText.text = string.Empty;
    }

    public void Awake()

    {
        equipButton.onClick.AddListener(() => InventoryController.Instance.SetEquipped(true));
        unEquipButton.onClick.AddListener(() => InventoryController.Instance.SetEquipped(false));
        useButton.onClick.AddListener(() => InventoryController.Instance.UsableUse());
        //throwButton.onClick.AddListener(() => InventoryController.Instance.Throw());
    }

    public void UpdateInventorySlots(Dictionary<string, ItemObject> items)
    {
        List<string> keysToRemove = new List<string>();

        foreach (var slot in slotUIs)
        {
            var key = slot.Key;
            if (!items.ContainsKey(key))
            {
                Destroy(slot.Value.gameObject); // UI 오브젝트 삭제
                keysToRemove.Add(key);
            }
        }

        foreach (var key in keysToRemove)
        {
            slotUIs.Remove(key);
        }

        foreach (var item in items) //아이템 생성
        {
            var key = item.Key;
            var value = item.Value;

            var itemStack = value.stack;
            var itemData = value.itemData;

            if (!slotUIs.ContainsKey(key))
            {
                var slot = Instantiate(slotUIPrefab, slotParentPos);
                slot.ItemName = key;
                slotUIs.Add(key, slot);
            }

            //slotUIs[key].ItemText.text = itemData.itemName;
            //slotUIs[key].ItemDescription.text = itemData.itemDescrpition;
            //if (value.itemData.dropPrefab.TryGetComponent<IGetItemData>(out IGetItemData getItem))
            //{
            //	slotUIs[key].CurStat.text = getItem.CurStat();
            //}
            slotUIs[key].ItemValue.text = itemStack.ToString();
            slotUIs[key].Icon.sprite = itemData.icon;
        }
    }

    public void SelectItem(ItemObject item)
    {
        if (item == null)
        {
            itemNameText.text = string.Empty;
            itemDescriptionText.text = string.Empty;
            curStatText.text = string.Empty;
            TypeCheck(ItemType.None);
            return;
        }

        var itemData = item.itemData;
        itemIcon.sprite = itemData.icon;
        itemNameText.text = itemData.itemName;
        itemDescriptionText.text = itemData.itemDescrpition;
        itemDescriptionText.ForceMeshUpdate();
        curStatText.text = string.Empty;
        if (item.TryGetComponent<IGetItemData>(out IGetItemData getItem))
        {
            curStatText.text = getItem.CurStat();
        }
        TypeCheck(itemData.itemType, item.isEquip);
    }

    public void TypeCheck(ItemType itemType, bool isActive = false)
    {
        equipButton.interactable = ((itemType == ItemType.Equipable) && !isActive); // 장착하지 않은 상태라면
        unEquipButton.interactable = ((itemType == ItemType.Equipable) && isActive); //장착한 상태라면
        useButton.interactable = (itemType == ItemType.Usable);
        // //각 버튼의 컴포넌트에서 Interactable 값을 이용해 Disabled Color를 표현
        // if((itemType == ItemType.Equipable) && !isActive)	// 장착하지 않은 상태라면
        // {
        // 	equipButton.interactable = true;
        //     unEquipButton.interactable = false;
        // }
        // else if((itemType == ItemType.Equipable) && isActive)   //장착한 상태라면
        // {
        //     equipButton.interactable = false;
        //     unEquipButton.interactable = true;
        // }

        // if(itemType == ItemType.Consumable)	// 소비성 아이템이라면
        // {
        //     useButton.interactable = true;

        // }
        // else    // 소비성 아이템이 아니라면
        // {
        //     useButton.interactable = false;
        // }
    }

    private void OnDisable()
    {
        AudioManager.Instance.UIOpenAndCloseClipPlay();
    }
}