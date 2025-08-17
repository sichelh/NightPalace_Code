using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public enum ZombieObjectType
{
    Object,
    Enemy
}
public class ZombieObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ZombieObjectType _type;
    [SerializeField] private ItemData _itemData; // 아이템 주는 경우
    [SerializeField] private ParticleSystem particle;
    
    [Header("음향 관련")]
    [SerializeField] private AudioClip _zombieClip;
    private AudioSource audioSource;
    
    [Header("애니메이션")]
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject crawlEnemy;
    [SerializeField] private GameObject zombieObject;
    
    private bool isPlay = false;

    [Header("이벤트 판정")]
    [SerializeField]private ChapterEvent chapterEvent;

    public ItemData ItemData { get => _itemData; set => _itemData = value; }

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            Debug.LogWarning("InstrumentObject: AudioSource not found!");
        }

        animator = GetComponentInChildren<Animator>();
        zombieObject = this.gameObject;
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
        particle.Play();
        if (isPlay) return;

        if (chapterEvent != null)
        {
            chapterEvent.OnceChapter();
            chapterEvent.AddEvent();
        }
        isPlay = true;
        Debug.Log(isPlay);

        if (_type == ZombieObjectType.Object)
        {
            _itemData = null;
            // 아이템 추가 기능
            // 해당 아이템은 플레이어 시점에서 확인함.
            //InventoryController.Instance.AddItem(_itemData);
        }
        else
        {
            animator.SetBool("Idle", true); //좀비 일어나는 로직
            StartCoroutine("StopZombieIdle");
            PlayZombieSound(); //좀비 사운드
        }
    }

    //-------------좀비 생성 코루틴------------//
     private IEnumerator StopZombieIdle()
     {
         Vector3 duration = new Vector3(0f, 0.5f, 0f);
         
         yield return new WaitForSeconds(1.7f);
         Destroy(zombieObject);
         Instantiate(crawlEnemy, transform.position - duration, Quaternion.identity);
     }
    
    private void PlayZombieSound()
    {
        if (audioSource && _zombieClip)
        {
            audioSource.PlayOneShot(_zombieClip);
        }
    }

    public ItemData GetData()
    {
        //if (!isPlay) return null;

        return _type == ZombieObjectType.Object ? _itemData : null;
    }

    public string InteractGetDataText()
    {
        string text = string.Empty;
        if (ZombieObjectType.Object == _type)
        {
            if (!isPlay) //선택하기 전에는 참, 선택하고 나면 확인한 시체
            {
                text = $"{_itemData.itemName} 을 획득하셨습니다.";
            }
            else
            {
                text = $"아무것도 발견할 수 없었습니다.";
            }
        }
        return text;
    }
}
