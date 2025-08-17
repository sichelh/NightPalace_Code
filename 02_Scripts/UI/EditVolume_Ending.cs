using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EditVolume_Ending : MonoBehaviour
{
    private Volume volume;
    private ColorAdjustments colorAdj;

    [Tooltip("노출값 시작점")]
    [SerializeField] private float fromExposure = 0f;
    [Tooltip("노출값 끝점")]
    [SerializeField] private float toExposure = 10f;
    [Tooltip("페이드 소요 시간")]
    [SerializeField] private float duration = 7f;

    private void Awake()
    {
        volume = GetComponent<Volume>();
        
        if (!volume.profile.TryGet<ColorAdjustments>(out colorAdj))
        {
            colorAdj = volume.profile.Add<ColorAdjustments>(true);
            Debug.LogError("Volume Profile에 Color Adjustments가 없습니다!");
        }
            
        colorAdj.postExposure.overrideState = true;
    }
    
    public void FadeExposure(float value)
    {
        StopAllCoroutines();
        StartCoroutine(CoFadeExposure(fromExposure, toExposure, duration));
    }

    private IEnumerator CoFadeExposure(float start, float end, float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            float t = elapsed / time;
            colorAdj.postExposure.value = Mathf.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        colorAdj.postExposure.value = end;
    }
    
}
