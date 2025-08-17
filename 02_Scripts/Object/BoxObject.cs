using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxType
{
    Empty,
    Item
}

public class BoxObject : MonoBehaviour, IInteractable
{
    [Header("음향 관련")]
    [SerializeField] private BoxType boxType;
    [SerializeField] private AudioClip boxClip;
    private AudioSource audioSource;

    [Header("아이템 관련")]
    [SerializeField] private ItemData itemData; // 박스에서 나올 아이템

    private bool isOpened = false;

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            Debug.LogWarning("BoxObject: AudioSource not found!");
        }
    }

    //--------------프롬프트 텍스트 반환 메서드--------------//
    public string PromptUI()
    {
        return string.Empty;
    }

    //--------------크로스헤어 이미지 반환 메서드--------------//
    public CrosshairType GetCrosshairType()
    {
        return CrosshairType.Item; //
    }

    //--------------상호작용 키 텍스트 반환 메서드--------------//
    public string KeyUI()
    {
        return isOpened ? "" : "[E] 열어 보기";
    }

    //--------------상호작용 메서드--------------//
    public void Interact()
    {
        if (isOpened) return;

        isOpened = true;

        PlayBoxSound();
        DecibelManager.Instance.ApplySound(13, transform);

        // 인벤토리 아이템 로직 추가
        if (boxType.Equals(BoxType.Item))
        {
            InventoryController.Instance.AddItem(itemData);
            itemData = null;
        }

        //Destroy(gameObject,1);
        // 파티클 추가 가능
    }

    private void PlayBoxSound()
    {
        if (audioSource && boxClip)
        {
            audioSource.PlayOneShot(boxClip);
        }
    }

    public ItemData GetData()
    {
        return isOpened ? itemData : null;
    }

    public string InteractGetDataText()
    {
        string text = string.Empty;

        if (boxType == BoxType.Item)
        {
            if (itemData != null)
            {
                text = $"{itemData.itemName} 을 획득하셨습니다.";
            }
            else
            {
                text = $"아무것도 들어있지 않다.";
            }
        }
        else
        {
            text = $"아무것도 들어있지 않다.";
        }

       return text;
    }
}
