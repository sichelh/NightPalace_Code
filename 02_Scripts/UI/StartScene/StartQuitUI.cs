using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartQuitUI : MonoBehaviour
{
    [field: SerializeField] private GameObject MenuUI;

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
            MenuUI.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        });
    }
}
