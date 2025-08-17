using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{

    [field: SerializeField] private Button resumeButton;
    [field: SerializeField] private Button settingButton;
    [field: SerializeField] private Button quitButton;

    private void OnEnable()
    {
        AudioManager.Instance.UIOpenAndCloseClipPlay();
    }

    void Start()
    {
        resumeButton.onClick.AddListener(() =>
        {
            Player player = FindObjectOfType<Player>();

            if (player != null)
            {
                player.Input.menuPressed = false;
                // resumeButton을 눌렀을 때, menuPressed를 false로 바꿔줘야 Idle 상태가 시작됨
            }
        });

        settingButton.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
        {
            UIManager.Instance.ToggleSettingUI();
        }));

        quitButton.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
        {
            UIManager.Instance.ToggleQuitUI();
        }));
    }
}
