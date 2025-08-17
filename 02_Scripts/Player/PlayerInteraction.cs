using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact Settings")]
    [SerializeField] private Camera _cam;
    [SerializeField] private float _detectDistance = 3f;
    [SerializeField] private LayerMask _interactableLayer;

    [SerializeField] private TextMeshProUGUI promptText;

    private GameObject currentTarget;

    void Update()
    {
        DetectInteractable();
    }

    //--------------오브젝트 감지 메서드--------------//
    private void DetectInteractable()
    {
        Ray ray = _cam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit[] hit = new RaycastHit[1];
        int hitCount = Physics.RaycastNonAlloc(ray, hit, _detectDistance, _interactableLayer);

        if (hitCount > 0)
        {
            var detectedObject = hit[0].collider.gameObject;
            if (detectedObject.TryGetComponent<IInteractable>(out var interactable))
            {
                currentTarget = detectedObject;
                UIManager.Instance.InteractionUI.UpdateInteractionUI(true, interactable);
                return;
            }
        }

        UIManager.Instance.InteractionUI.UpdateInteractionUI(false, null);
        currentTarget = null;
    }

    //--------------오브젝트의 특정 컴포넌트를 반환하는 메서드--------------//
    public T GetComponentFromTarget<T>() where T : Component
    {
        return currentTarget != null ? currentTarget.GetComponent<T>() : null;
    }

    //--------------상호작용 메서드--------------//
    public void Interact()
    {
        if (currentTarget != null &&
            currentTarget.TryGetComponent<IInteractable>(out var interactable))
        {

            promptText.text = interactable.InteractGetDataText();
            StartCoroutine(ViewText());
            ItemData itemData = interactable.GetData();
            if (itemData == null)
            {
                // 아이템 없으면 상호작용만 실행
                interactable.Interact();
                return;
            }
            Player player = Player.Instance;

            switch (itemData.itemType) //아이템 상호작용
            {
                case ItemType.Usable:
                case ItemType.Equipable:
                case ItemType.Quest:
                    InventoryController.Instance.AddItem(itemData);
                    //TODO : 이벤트 발생; -> 아래 if문에 옮김.
                    break;
                case ItemType.Grapable:
                    player.Grap(itemData);
                    break;
            }

            if (ItemType.Quest == itemData.itemType)
            {
                //TODO : 이벤트 발생
            }
            interactable.Interact(); //삭제
        }
    }

    public IEnumerator ViewText()
    {
        float time = 1f;

        while (time > 0f)
        {
            promptText.alpha = time;
            time -= Time.unscaledDeltaTime;
            yield return null;
        }
        promptText.alpha = 0f;
        yield return null;
    }
}
