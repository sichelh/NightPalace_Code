using UnityEngine;
using UnityEngine.Assertions.Must;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ChapterEvent chapterEvent;
    public ItemData itemData;
    public int stack = 1;
    public bool isEquip = false;

    public void Init(ItemData itemData)
    {
        this.itemData = itemData;
    }

    public ItemObject(ItemData itemData)
    {
        this.itemData = itemData;
    }

    //--------------프롬프트 텍스트 반환 메서드--------------//
    public string PromptUI()
    {
        string promptText = $"{itemData.itemName}";
        return promptText;
    }

    //--------------크로스헤어 타입 반환 메서드--------------//
    public CrosshairType GetCrosshairType()
    {
        return CrosshairType.Item;
    }

    //--------------상호작용 키 텍스트 반환 메서드--------------//
    public string KeyUI()
    {
        return "[E] Pick up";
    }

    //--------------상호작용 메서드--------------//
    public void Interact()
    {
        //이벤트 활성화
        if (chapterEvent != null)
        {
            chapterEvent.AddEvent();
        }
        //픽업 효과음 재생
        AudioManager.Instance.PickUpClipPlay();
        Destroy(gameObject);
    }

	public ItemData GetData()
	{
        return itemData;
	}

    public virtual void ChangeActive(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public string InteractGetDataText()
    {
        return $"{itemData.itemName} 을 획득하셨습니다.";
    }
}
