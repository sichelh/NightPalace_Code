using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitUI : MonoBehaviour
{
    [field: SerializeField] private Button YesButton;
    [field: SerializeField] private Button NoButton;
    void Start()
    {
        YesButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        NoButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ToggleQuitUI();
        });
    }
}
