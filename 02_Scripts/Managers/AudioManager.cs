using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("BGM")]
    [field:SerializeField] public AudioSource BGMPlayer;
    [field: SerializeField] public AudioClip BGMClip { get; private set; }
    [field: SerializeField] public AudioClip DeadBGMClip { get; private set; }

    [Header("SFX")]
    [field:SerializeField] private AudioSource SFXplayer;
    [field: SerializeField] public AudioClip ItemPickUpClip { get; private set; }
    [field: SerializeField] public AudioClip BookPageTurnClip { get; private set; }

    [Header("UI")]
    [field:SerializeField] private AudioSource UIPlayer;
    [field: SerializeField] public AudioClip UIOpenClip { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        BGMPlay();
    }

    public void PickUpClipPlay()
    {
        SFXplayer.PlayOneShot(ItemPickUpClip);
    }

    public void UIOpenAndCloseClipPlay()
    {
        UIPlayer.PlayOneShot(UIOpenClip);
    }

    public void BookPageTurnPlay()
    {
        SFXplayer.PlayOneShot(BookPageTurnClip);
    }

    public void BGMPlay()
    {
        BGMPlayer.clip = BGMClip;
        BGMPlayer.loop = true;
        BGMPlayer.Play();
    }

    public void DeadBGMPlay()
    {
        BGMPlayer.clip = DeadBGMClip;
        BGMPlayer.loop = false;
        BGMPlayer.Play();
    }
}
