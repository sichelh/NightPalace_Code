using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum InstrumentType
{
    Item,
    Music
}
public class InstrumentObject : MonoBehaviour, IInteractable
{
    [SerializeField] private InstrumentType type;
    [SerializeField] private ItemData itemData; // 아이템 주는 경우
    [Header("음향 관련")]
    [SerializeField] private AudioClip instrumentClip;
    private AudioSource audioSource;

    private bool isPlay = false;

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            Debug.LogWarning("InstrumentObject: AudioSource not found!");
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
        return "[E] 조사 하기";
    }

    //--------------상호작용 메서드--------------//
    public void Interact()
    {

        if (type == InstrumentType.Item)
        {
            // 아이템 추가 기능
            InventoryController.Instance.AddItem(itemData);
            itemData = null;
        }
        else
        {
            DecibelManager.Instance.ApplySound(20, transform);
            PlayInstrumentSound();
        }
        //Destroy(gameObject,3f); 삭제 안함.
       
    }

    private void PlayInstrumentSound()
    {
        if (audioSource && instrumentClip)
        {
            audioSource.PlayOneShot(instrumentClip);
        }
    }

    public ItemData GetData()
    {
        if (!isPlay) return null;

        return type == InstrumentType.Item ? itemData : null;
    }

    public string InteractGetDataText()
    {
        string text = string.Empty;

        if(type == InstrumentType.Item)
        {
            if (itemData == null)
                text = $"아무것도 없다.";
            else
                text = $"{itemData.itemName} 을 획득하셨습니다.";
        }
        else
        {
            text = $"소리가 들린다.";
        }

        return text;
    }
}
