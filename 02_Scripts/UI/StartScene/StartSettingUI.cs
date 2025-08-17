using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartSettingUI : MonoBehaviour
{

    [field: SerializeField] private GameObject MenuUI;

    [field: SerializeField] private Slider masterVolume;
    [field: SerializeField] private Slider BGMVolume;
    [field: SerializeField] private Slider SFXVolume;
    [field: SerializeField] private Slider UIVolume;
    [field: SerializeField] private Button backButton;

    [field: SerializeField] private AudioMixer audioMixer;

    void OnEnable()
    {
        InitAudioMixerVolume();
    }

    private void Start()
    {
        masterVolume.onValueChanged.AddListener(SetMasterVolume);
        BGMVolume.onValueChanged.AddListener(SetBGMVolume);
        SFXVolume.onValueChanged.AddListener(SetSFXVolume);
        UIVolume.onValueChanged.AddListener(SetUIVolume);

        backButton.onClick.AddListener(() =>
        {
            MenuUI.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        });
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", ToDecibel(volume));
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", ToDecibel(volume));
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", ToDecibel(volume));
    }

    public void SetUIVolume(float volume)
    {
        audioMixer.SetFloat("UI", ToDecibel(volume));
    }

    private float ToDecibel(float linear)
    {
        return Mathf.Log10(Mathf.Clamp(linear, 0.0001f, 1f)) * 20f;
    }

    private float ToLinear(float decibel)
    {
        return Mathf.Pow(10f, decibel / 20f);
    }

    private void InitAudioMixerVolume()
    {
        float volume;

        if (audioMixer.GetFloat("Master", out volume))
        {
            masterVolume.value = ToLinear(volume);
        }

        if (audioMixer.GetFloat("BGM", out volume))
        {
            BGMVolume.value = ToLinear(volume);
        }

        if (audioMixer.GetFloat("SFX", out volume))
        {
            SFXVolume.value = ToLinear(volume);
        }

        if (audioMixer.GetFloat("UI", out volume))
        {
            UIVolume.value = ToLinear(volume);
        }
    }
}
