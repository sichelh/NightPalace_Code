using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI promptUI;
    [SerializeField] private Image crosshairUI;
    [SerializeField] private TextMeshProUGUI keyUI;

    [Header("Crosshair Sprites")]
    [SerializeField] private Sprite itemCrosshair;
    [SerializeField] private Sprite doorCrosshair;
    [SerializeField] private Sprite lockedDoorCrosshair;

    //--------------상호작용 UI 업데이트 메서드--------------//
    public void UpdateInteractionUI(bool detected, IInteractable interactable)
    {
        if (detected && interactable != null)
            ShowInteractionUI(interactable);
        else
            HideInteractionUI();
    }

    //--------------상호작용 UI 켜기--------------//
    private void ShowInteractionUI(IInteractable interactable)
    {
        promptUI.text = interactable.PromptUI();
        keyUI.text = interactable.KeyUI();
        UpdateCrosshair(interactable.GetCrosshairType());
        crosshairUI.color = Color.white;
    }

    //--------------상호작용 UI 끄기--------------//
    private void HideInteractionUI()
    {
        promptUI.text = string.Empty;
        keyUI.text = string.Empty;
        crosshairUI.sprite = null;
        crosshairUI.color = new Color(0, 0, 0, 0);
    }

    //--------------타입에 따라 크로스헤어를 변경하는 메서드--------------//
    private void UpdateCrosshair(CrosshairType type)
    {
        switch (type)
        {
            case CrosshairType.Item:
                crosshairUI.sprite = itemCrosshair;
                break;
            case CrosshairType.Door:
                crosshairUI.sprite = doorCrosshair;
                break;
            case CrosshairType.LockedDoor:
                crosshairUI.sprite = lockedDoorCrosshair;
                break;
            default:
                crosshairUI.sprite = null;
                break;
        }
    }
}
