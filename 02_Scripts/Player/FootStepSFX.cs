using System.Collections;
using UnityEngine;

public class FootStepSFX : MonoBehaviour
{
    [field: Header("Settings")]
    public Transform foot;

    [field: Header("Walk")]
    [field: SerializeField] public float WalkPeriod { get; private set; } = 0.8f;
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }

    [field: Header("Sprint")]
    [field: SerializeField] public float SprintPeriod { get; private set; } = 0.4f;

    [field: Header("Clips")]
    [field: SerializeField] public AudioClip[] SandClips { get; private set; }
    //추가

    [field: SerializeField] private AudioClip[] footStepClips;
    [field: SerializeField] private AudioClip curClip;

    [field: Header("Components")]
    [field: SerializeField] private AudioSource audioSource;
    [field: SerializeField] private Player player;

    private float time = 0f;

    void Awake()
    {
        if (!foot.gameObject.TryGetComponent<AudioSource>(out audioSource))
        {
            Debug.LogError("AudioSource is null");
            return;
        }

        if (!TryGetComponent<Player>(out player))
        {
            Debug.LogError("Player is null");
            return;
        }
    }

    public void FootStepSFXPlay()
    {
        //걷기 및 달리기 발자국 SFX
        DetectSurface();

        time += Time.deltaTime;

        float period = player.Input.SprintPressed ? SprintPeriod : WalkPeriod;

        if (time > period)
        {
            audioSource.PlayOneShot(curClip);
            DecibelManager.Instance.ApplySound(7, transform);
            time = 0;
        }
    }

    //--------------표면 감지 메서드--------------//
    private void DetectSurface()
    {
        Ray ray = new Ray(foot.position, Vector3.down);
        RaycastHit[] hit = new RaycastHit[1];

        int hitCount = Physics.RaycastNonAlloc(ray, hit, 0.2f, GroundLayer);

        if (hitCount > 0)
        {
            //머터리얼의 이름을 클립 변경 메서드로 보냄
            Renderer renderer = hit[0].collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                string materialName = material.name;

                footStepClips = FootStepClipSwitch(materialName);
            }

            if (footStepClips != null)
            {
                curClip = footStepClips[Random.Range(0, footStepClips.Length)];
            }
        }
    }

    //--------------텍스처를 받아 클립을 변경해주는 메서드--------------//
    private AudioClip[] FootStepClipSwitch(string _surfaceName)
    {
        string[] _textureNames = _surfaceName.Split('_'); //텍스처 이름을 '_'로 분리하여 배열로 만듭니다.

        //ex) m_dirt_01 -> dirt
        switch (_textureNames[1])
        {
            //추가 및 텍스트 수정 필요
            case "sand":
                return SandClips;
            default:
                return null;
        }
    }
}
