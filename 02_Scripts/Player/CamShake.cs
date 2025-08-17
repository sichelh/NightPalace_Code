using Cinemachine;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    [field: Header("Components")]
    [field: SerializeField] public CinemachineVirtualCamera FPS_cam { get; private set; }   //1인칭 시네머신 컴포넌트
    private CinemachineBasicMultiChannelPerlin perlin;   //1인칭 시네머신의 노이즈

    private void Awake()
    {
        perlin = FPS_cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    //--------------진폭과 주기를 받아 변경함--------------//
    public void Shake(NoiseSettings noise, float amplitude, float frequency)
    {
        perlin.m_NoiseProfile = noise;
        perlin.m_AmplitudeGain = amplitude;
        perlin.m_FrequencyGain = frequency;
    }
}
