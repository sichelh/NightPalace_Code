using UnityEngine;
using UnityEngine.Audio;

public class PlayerSFX : MonoBehaviour
{
    [field: Header("Clips")]
    [field: SerializeField] public AudioClip[] JumpClips { get; private set; }
    [field: SerializeField] public AudioClip[] DeadClips { get; private set; }
    [field: SerializeField] public AudioClip[] HitClips { get; private set; }

    [field: Header("Components")]
    [field: SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void JumpSFXPlay()
    {
        _audioSource.PlayOneShot(JumpClips[Random.Range(0, JumpClips.Length)]);
        DecibelManager.Instance.ApplySound(10, transform);
    }

    public void DeadSFXPlay()
    {
        _audioSource.PlayOneShot(DeadClips[Random.Range(0, DeadClips.Length)]);
    }

    public void HitSFXPlay()
    {
        _audioSource.PlayOneShot(HitClips[Random.Range(0, HitClips.Length)]);
    }
}
