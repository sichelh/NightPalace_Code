using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneBGMPlayer : MonoBehaviour
{
    
    private AudioSource audioSource;
    [field: SerializeField] private AudioClip StartSceneBGMClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.clip = StartSceneBGMClip;
        audioSource.Play();
    }
}
