using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditUI : MonoBehaviour
{
    [field: SerializeField] private GameObject MenuUI;
    [field: SerializeField] private Button backButton;

    void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            MenuUI.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        });
    }
}
