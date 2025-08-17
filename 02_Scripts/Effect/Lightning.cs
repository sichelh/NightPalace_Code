using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    //TODO : 코루틴으로 밝기 꺼졌다 켜졌다.
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float lightningTime;
    [SerializeField] private float minTimeDelay;
    [SerializeField] private float maxTimeDelay;
    [SerializeField] private Light light;
    [SerializeField] private AudioSource audio;

    private void Awake()
    {
        StartCoroutine(LightEffect());    
    }

    IEnumerator LightEffect()
    {
        while(true)
        {
            float time = Random.Range(minTimeDelay, maxTimeDelay);
            float curTime = 0f;
            while(time > curTime)
            {
                curTime += Time.deltaTime;
                yield return null;
            }
            //TODO 번개 반짝임. 움직임 애니메이션
            //Curve
            curTime = 0f;
            audio.Play();
            while (lightningTime > curTime)
            {
                curTime += Time.deltaTime;
                light.intensity = 0.2f + curve.Evaluate(curTime / lightningTime);
                float value = curve.Evaluate(curTime/lightningTime);
                light.color = new Color(value*1.3f, value*1.3f, value*2);
                yield return null;
                //value만큼 반짝임 효과
            }
        }
    }
}
