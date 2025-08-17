using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public static LoadingUI Instance;

    [Header("UI 요소")]
    public GameObject loadingPanel;     // 전체 로딩 패널
    public Slider progressBar;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI LodingText;
    private string[] lodingText;
    private bool isLoading = false;
    AsyncOperation asyncOp;

    private void Awake()
    {
        // 싱글톤 세팅
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            loadingPanel.SetActive(false); // 초기 비활성화
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        lodingText = new string[]
        {
            "폐궁에 입장하는 중...",
            "해당 게임 개발팀의 평균 수면시간은 5시간입니다",
            "당신이 퇴근한 후에도… 매니저님은 당신의 TIL을 기다리고 계십니다",
            "어? 뒤에 누구에요?",
            "해당 게임의 개발 기간은 일주일입니다",
            "Welcome to this game!",
            "PR 제목 \"여러가지 수정사항\"이라고 한 사람 누구에요?",
            "돼지님은...문짝을 뜯어...",
        };
    }

    public void LoadScene(string sceneName)
    {
        if (isLoading) return; // 이미 로딩 중이면 무시
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        isLoading = true;
        loadingPanel.SetActive(true);
        progressBar.value = 0f;
        progressText.text = "0%";

        asyncOp = SceneManager.LoadSceneAsync(sceneName);
        asyncOp.allowSceneActivation = false;

        float displayedProgress = 0f;
        float targetProgress = 0f;
        LodingText.text = lodingText[Random.Range(0, lodingText.Length)];

        while (!asyncOp.isDone)
        {
            // 실제 로딩 진행도 계산 (0 ~ 0.9)
            targetProgress = Mathf.Clamp01(asyncOp.progress / 0.9f);

            // 실제 progress에 따라 UI 진행률을 Lerp로 부드럽게 증가
            displayedProgress = Mathf.Lerp(displayedProgress, targetProgress, Time.deltaTime * 1f);
            progressBar.value = displayedProgress;
            progressText.text = $"{displayedProgress * 100f:F0}%";

            // 실제 로딩 완료되었을 때 (0.9 이상)
            if (displayedProgress >= 0.995f)
            {
                progressBar.value = 1f;
                progressText.text = "100%";

                yield return new WaitForSeconds(0.3f); // 연출용 대기
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingPanel.SetActive(false);
        isLoading = false;
    }

}
