using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerCondition : MonoBehaviour
{
    //HP
    [field: SerializeField][Range(0f, 100f)] public float hp;
    [field: SerializeField] public float MaxHp { get; private set; } = 100f;

    //감염도
    [field: SerializeField][Range(0f, 100f)] public float infection;
    [field: SerializeField] public float MaxInfection { get; private set; } = 100f;
    [field: SerializeField] private float infectionValue;
    [field: SerializeField] private float DamageByInfection = 10f;
    [field: SerializeField] private float DamageByInfectionDuration = 5f;

    [field: SerializeField][Range(0f, 100f)] public float stamina;
    [field: SerializeField] public float MaxStamina { get; private set; } = 100f;

    public bool isAttacked = false;
    public bool isDrunk = false;

    float time = 0f;

    private Player player;
    [field: SerializeField] public StatsUI StatsUI { get; private set; }
    [field: SerializeField] private HitPanel hitPanel;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        hp = MaxHp;
        infection = 0f;
        stamina = MaxStamina;
        StatsUI.UpdateStatsUI();
    }

    private void Update()
    {
        if (isDrunk)
        {
            isAttacked = false;
            //DecreaseInfection();
        }

        if (isAttacked)
        {
            isDrunk = false;
            IncreaseInfection();
        }

        if (infection > 80f && hp > 0.1f)
        {
            GetDamageByInfection(DamageByInfection);
        }

        StatsUI.UpdateStatsUI();
    }

    //스태미나 증가
    public void IncreaseStaminaOnSprint()
    {
        stamina += player.PlayerStateData.Ground.SprintStaminaIncrease * Time.deltaTime;
    }

    //스태미나 감소
    public void DecreaseStaminaOnSprint()
    {
        stamina -= player.PlayerStateData.Ground.SprintStaminaDecrease * Time.deltaTime;
    }

    //점프시, 스태미나 감소
    public void DecreaseStaminaOnJump()
    {
        stamina -= player.PlayerStateData.Air.JumpStaminaDecrease;
    }

    //감염도 받아오기
    public float GetInfection()
    {
        return infection;
    }

    //감염도 증가
    public void IncreaseInfection()
    {
        infection += infectionValue * Time.deltaTime;
    }

    //감염도 감소
    public void DecreaseInfection(float value)
    {
        infection = Mathf.Max(infection - value, 0);
    }

    //물리적 공격에 의한 데미지
    public void GetPhysicsDamage(float damage)
    {
        hp -= damage;
        player.StartParticle(player.bloodOnceParticleHash);
        //TODO : 경직 되면 더 좋을듯?
        //TODO : 카메라 흔들림 있는건가..?
        StartCoroutine(StopPlayer());

        if (hp > 0.1f)
        {
            hitPanel.ShowHitPanel();
            player.PlayerSFX.HitSFXPlay();
        }
    }

    public IEnumerator StopPlayer() //경직 상태
    {
        player.CamShake.Shake(player.PlayerStateData.GroundCamShake.Noise, 3f, 1.0f);
        player._PlayerInput.actions["Move"].Disable();
        player._PlayerInput.actions["Jump"].Disable();
        player._PlayerInput.actions["Look"].Disable();
        yield return new WaitForSeconds(0.5f);

        player.CamShake.Shake(player.PlayerStateData.GroundCamShake.Noise, 0f, 0f);
        player._PlayerInput.actions["Move"].Enable();
        player._PlayerInput.actions["Jump"].Enable();
        player._PlayerInput.actions["Look"].Enable();
    }

    // 감염에 의한 데미지 5초마다 줌
    public void GetDamageByInfection(float damage)
    {
        time += Time.deltaTime;
        if (time > DamageByInfectionDuration)
        {
            hp -= damage;
            player.PlayerSFX.HitSFXPlay();

            time = 0f;
        }

        hitPanel.ShowHitPanel();
    }

    //체력 회복
    public void Heal(float heal)
    {
        hp = Mathf.Min(hp + heal, 100);
        hitPanel.HideHitPanel();
    }
}
