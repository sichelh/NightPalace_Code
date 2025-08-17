using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadUI : MonoBehaviour
{
    [field: SerializeField] private Button goToMainButton;

    private Image DeadPanel;
    [field: SerializeField] private float DeadPanelDuration = 5f;
    [field: SerializeField] private Color startColor = new Color(0f, 0f, 0f, 0f);
    [field: SerializeField] private Color endColor = Color.black;

    private void Awake()
    {
        DeadPanel = GetComponentInChildren<Image>();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeDeadUI());
    }

    void Start()
    {
        goToMainButton.onClick.AddListener(() =>
        {
            UIManager.Instance.HideDeadUI();
            LoadingUI.Instance.LoadScene("StartScene");
        });
    }

    private IEnumerator FadeDeadUI()
    {
        float time = 0f;

        while (time < DeadPanelDuration)
        {
            time += Time.deltaTime;
            DeadPanel.color = Color.Lerp(startColor, endColor, time / DeadPanelDuration);
            yield return null;
        }

        DeadPanel.color = endColor;
    }
}
