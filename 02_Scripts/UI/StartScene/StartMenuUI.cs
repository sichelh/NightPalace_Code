using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartMenuUI : MonoBehaviour
{
    [field: Header("Buttons")]
    [field: SerializeField] private Button startButton;
    [field: SerializeField] private Button settingButton;
    [field: SerializeField] private Button KeyGuideButton;
    [field: SerializeField] private Button creditButton;
    [field: SerializeField] private Button quitButton;

    [field: Header("UIs")]
    [field: SerializeField] private GameObject MenuUI;
    [field: SerializeField] private StartSettingUI startSettingUI;
    [field: SerializeField] private KeyGuideUI KeyGuideUI;
    [field: SerializeField] private CreditUI creditUI;
    [field: SerializeField] private StartQuitUI quitUI;

    void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            MenuUI.gameObject.SetActive(false);
            LoadingUI.Instance.LoadScene("IntroCutScene");
        });

        settingButton.onClick.AddListener(() =>
        {
            MenuUI.gameObject.SetActive(false);
            startSettingUI.gameObject.SetActive(true);
        });

        KeyGuideButton.onClick.AddListener(() =>
        {
            MenuUI.gameObject.SetActive(false);
            KeyGuideUI.gameObject.SetActive(true);
        });

        creditButton.onClick.AddListener(() =>
        {
            MenuUI.gameObject.SetActive(false);
            creditUI.gameObject.SetActive(true);
        });

        quitButton.onClick.AddListener(() =>
        {
            MenuUI.gameObject.SetActive(false);
            quitUI.gameObject.SetActive(true);
        });
    }
}
