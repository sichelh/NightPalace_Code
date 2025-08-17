using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorObject : MonoBehaviour, IInteractable
{
    [Header("위치 관련")]
    [SerializeField] private float _openSpeed = 2f;
    [SerializeField] private bool _isLeftDoor = true;
    private bool isOpen = false;

    [Header("음향 관련")]
    [SerializeField] private AudioClip _openClip;
    [SerializeField] private AudioClip _closeClip;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _destroyClip;
    private AudioSource audioSource;
    public bool isLocked = false;

    [Header("체력 관련")]
    [SerializeField] private int _health = 5; // 문 체력

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine currentCoroutine;
    private float openAngle;
    private NavMeshLink doorLink;

    [Header("이벤트 관련")]
    [SerializeField] private ChapterEvent chapterEvent;

    private void Awake()
    {
        openAngle = _isLeftDoor ? 90f : -90f;
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
           // Debug.Log("audioSource is null");
        }
        if (!TryGetComponent<NavMeshLink>(out doorLink))
        {
            //Debug.LogWarning("문 오브젝트에 NavMeshLink가 없습니다.");
        }
    }

    private void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    //--------------프롬프트 텍스트 반환 메서드--------------//
    public string PromptUI()
    {
        string promptText = string.Empty;
        return promptText;
    }

    //--------------크로스헤어 이미지 반환 메서드--------------//
    public CrosshairType GetCrosshairType()
    {
        return isLocked ? CrosshairType.LockedDoor : CrosshairType.Door;
    }

    //--------------상호작용 키 텍스트 반환 메서드--------------//
    public string KeyUI()
    {
        if (isLocked)
        {
            return LockedDoor();
        }
        return isOpen ? "[E] Close" : "[E] Open";
    }

    //--------------상호작용 메서드--------------//
    public void Interact()
    {
        if(chapterEvent != null)
        {
            chapterEvent.OnceChapter();
            if (chapterEvent.QuestClear())
            {
                SceneManager.LoadScene("EndingScene");
                isLocked = false;
            }
        }

        if (isLocked)
        {
            return;
        }

        // 상태 토글
        isOpen = !isOpen;

        // 기존 코루틴 중지
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Navmesh Link 관리
        if (doorLink != null)
        {
            doorLink.enabled = isOpen;
        }

        // 새 코루틴 시작
        currentCoroutine = StartCoroutine(ToggleDoor(isOpen));

        PlayDoorSound();

        Debug.Log(isOpen ? "Open" : "Close");
    }

    private void PlayDoorSound()
    {
        if (audioSource == null) return;

        AudioClip clipToPlay = isOpen ? _openClip : _closeClip;

        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }
    }

    //--------------잠겨있는 문과 상호작용했을 때 실행되는 메서드--------------//
    private string LockedDoor()
    {
        string lookDoorText = "It's Locked!";
        Debug.Log(lookDoorText);
        return lookDoorText;
    }

	public ItemData GetData()
	{
		return null;
	}
    //--------------문 열고 닫는 회전 코루틴--------------//
    private IEnumerator ToggleDoor(bool opening)
    {
        Quaternion targetRotation = opening ? openRotation : closedRotation;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _openSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    //--------------몬스터가 문에 충돌했을 때 체력 감소 및 문 파괴 처리--------------//
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Monster"))
    //    {
    //        TakeDamage(1);
    //    }
    //}

    //--------------문이 피해를 입을 때 호출되는 메서드--------------//
    public void TakeDamage(int damage)
    {
        _health -= damage;
        audioSource.PlayOneShot(_attackClip);
        if (_health <= 0)
        {
            audioSource.PlayOneShot(_destroyClip);
            Destroy(gameObject,1f); // 문파괴
        }
    }

    public string InteractGetDataText()
    {
        string text;
        if (!isOpen)
        {
            text = "문이 열렸습니다.";
        }
        else
        {
            text = "문이 닫혔습니다.";
        }

        if (isLocked) text = "문이 잠겨있습니다.";

        if (chapterEvent != null) //퀘스트가 있나요?
        {
            if (chapterEvent.QuestClear()) //퀘스트를 클리어했나요?
            {
                return chapterEvent.unLockText;
            }
            else
            {
                return chapterEvent.lockText;
            }
        }


        return text;
    }
}
