using UnityEngine;

public class ActiveSkipButton : MonoBehaviour
{
    [SerializeField] private int introNumber;
    [SerializeField] private GameObject skipButton;
    
    private void Start()
    {
        introNumber = PlayerPrefs.GetInt("introNumber");
        if (introNumber > 0)
        {
            skipButton.SetActive(true);
        }

        introNumber++;
        PlayerPrefs.SetInt("introNumber", introNumber);
        PlayerPrefs.Save();
    }
}
