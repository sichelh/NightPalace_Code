using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FlashLight : ItemObject, IUsable, IGetItemData
{
    public GameObject light;
    public float maxBattery = 100;
    public float battery = 100; //현 배터리량.
    public float delay = 1.0f; //배터리 닳는 속도
    public float minusValue = 0.02f; //배터리 닳는 양
    public float weight = 1f; // 가중치

    Coroutine onLight;

    public FlashLight(ItemData itemData) : base(itemData)
    {
    }

    public void OnEnable()
    {
        light.SetActive(false);
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    public bool Use()
    {
        if (battery <= 0) return false;

        base.ChangeActive(light);
        if (light.activeSelf)
        {
            onLight = StartCoroutine(OnLight());
        }
        else
        {
            Debug.Log("정지됨");
            StopCoroutine(onLight);
        }
        return true;
    }
    public string CurStat()
    {
        return "배터리 : " + battery.ToString("N2");
    }

    IEnumerator OnLight()
    {
        while (battery >= 0)
        {
            yield return new WaitForSeconds(delay);
            battery--;
            InventoryController.Instance.UpdateItem(this);
        }
        base.ChangeActive(light);
    }

}