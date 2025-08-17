using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BookType
{
    Key,
    Normal
}
public class BookObject : MonoBehaviour, IInteractable
{
    [SerializeField] private BookType bookType;
    [Header("음향 관련")]
    [SerializeField] private AudioClip bookClip;
    private AudioSource audioSource;

    [SerializeField] public ItemData itemData; // 부적
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
        return isOpened ? "" : "[E] 조사 하기";
    }

    //--------------상호작용 메서드--------------//
    public void Interact()
    {
        PlayBookSound();

        // 책 UI나오게하는 로직 추가
        if(itemData != null)
        {
            UIManager.Instance.BookUI.SetKey(true);
            UIManager.Instance.BookUI.bookObject = this;
        }
        else
        {
            UIManager.Instance.BookUI.SetKey(false);
        }
        UIManager.Instance.BookUI.gameObject.SetActive(true);
        // 책 없애기
        //Destroy(gameObject);
    }

    private void PlayBookSound()
    {
        if (audioSource && bookClip)
        {
            audioSource.PlayOneShot(bookClip);
        }
    }

    public ItemData GetData()
    {
        return null;
    }

    public string InteractGetDataText()
    {
        string text = string.Empty;

        text = $"책을 여셨습니다.";

        return text;
    }
}
