using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPanel : MonoBehaviour
{
    [field: SerializeField] private GameObject Panels;

    void Update()
    {
    }

    public void ShowHitPanel()
    {
        Panels.SetActive(true);
    }

    public void HideHitPanel()
    {
        Panels.SetActive(false);
    }
}
