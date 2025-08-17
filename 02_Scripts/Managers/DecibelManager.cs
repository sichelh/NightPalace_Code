using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DecibelManager : Singleton<DecibelManager>
{
    public float currentValue; //현재 데시벨
    public float perceptibleValue = 5; //좀비가 쫓아오는 데시벨 기준
    
    public float attenuationPerUnit = 0.5f; //감쇠 계수
    public float distance; //좀비와의 거리
    
    private float descreasePerUnit = 2f;
    
    public Transform currentSoundObject;
    
    //소리 발생 메서드
    public void ApplySound(float decibel, Transform soundObject)
    {
        currentValue = decibel;
        StopCoroutine("ApplySound");
        StartCoroutine(DecreaseSound());
        currentSoundObject = soundObject;
    }

    //소리 감소시키기
    private IEnumerator DecreaseSound()
    {
        while (currentValue > 0)
        {
            currentValue -= Time.deltaTime * descreasePerUnit;
            yield return null;
        }

        currentSoundObject = null;
    }
    
    //리스너 위치로부터 실제 들리는 데시벨 계산
    public float GetPercivedDecibel(Vector3 listenerPosition)
    {
        if (currentSoundObject == null)
        {
            return float.NegativeInfinity;
        }
        
        distance = Vector3.Distance(currentSoundObject.position, listenerPosition);
        return currentValue - attenuationPerUnit * distance;
    }
}