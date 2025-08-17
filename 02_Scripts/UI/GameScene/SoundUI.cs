using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject[] soundIcon;
    [SerializeField] private float maxValue = 30f;

    private float currentValue;
    private Color color;
    
    private void Update()
    {
        currentValue = DecibelManager.Instance.currentValue;
        
        SetValue();
        ChangeSoundIcon();
    }
    
    private void SetValue()
    {
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        
        fillImage.fillAmount = currentValue / maxValue;
    }

    private void ChangeSoundIcon()
    {
        color = fillImage.color;
        
        if (currentValue == 0)
        {
            soundIcon[1].SetActive(false);
            soundIcon[2].SetActive(false);
            soundIcon[0].SetActive(true);
            
            color = Color.white;
        }
        else if (currentValue < 10)
        {
            soundIcon[0].SetActive(false);
            soundIcon[2].SetActive(false);
            soundIcon[1].SetActive(true);
            
            color = Color.white;
        }
        else if (currentValue < 30)
        {
            soundIcon[0].SetActive(false);
            soundIcon[1].SetActive(false);
            soundIcon[2].SetActive(true);
            
            color = Color.red;
        }
        
        color.a = 0.5f;
        fillImage.color = color;
    }
}
