using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Singleton<Player>
{
    public PlayerController Input { get; private set; }
    public CharacterController Controller { get; set; }
    public PlayerInteraction Interaction { get; private set; }
    public PlayerSFX PlayerSFX { get; private set; }
    public FootStepSFX FootStepSFX { get; private set; }
    public CamShake CamShake { get; private set; }
    public PlayerCondition PlayerCondition { get; private set; }
    public StateMachine<Player> StateMachine { get; private set; }

    [field : SerializeField] public List<ParticleSystem> Particle { get; private set; }

    private StateFactory<Player> stateFactory; // 상태들 캐싱
    [field: SerializeField] public StateData PlayerStateData { get; private set; }

	[SerializeField] private bool isEquip = false;
    [SerializeField] private bool isGrap = false;
	[SerializeField] private Transform target;
    public StateFactory<Player> StateFactory => stateFactory;
    public bool IsEquip { get => isEquip;  set => isEquip = value; }
	public bool IsGrap { get => isGrap; set => isGrap = value; }
	[field : SerializeField] public GameObject EquipItem { get; set; }
    [field : SerializeField] public ItemData EquipItemData { get; set; }
    public Transform Target { get => target; private set => target = value; }
    public PlayerInput _PlayerInput { get; private set; }

    public string walkParticleHash = "StartWalkParticle";
    public string runParticleHash = "StartRunParticle";
    public string breathParticleHash = "StartBreathParticle";
    public string breathOnceParticleHash = "StartBreathParticleOnce";
    public string bloodOnceParticleHash = "StartBloodParticleOnce";

    public void StartParticle(string action = null)
    {
        if (action != null)
        {
            StartCoroutine(action);
        }
    }
    public void StopParticle()
    {
        StopAllCoroutines();
    }

    IEnumerator StartBloodParticleOnce()
    {
        Particle[3].Play();
        yield return null;
    }

    IEnumerator StartWalkParticle()
    {
        while (true)
        {
            Particle[0].Play();
            yield return new WaitForSeconds(1.0f);
        }
    }
    IEnumerator StartRunParticle()
    {
        while (true)
        {
            Particle[1].Play();
            yield return new WaitForSeconds(1.0f);
        }
    }
    IEnumerator StartBreathParticle()
    {
        while (true)
        {
            Particle[2].Play();
            yield return new WaitForSeconds(10.0f);
        }
    }

    IEnumerator StartBreathParticleOnce()
    {
        Particle[2].Play();
        yield return null;
    }

    protected override void Awake()
    {
        base.Awake();
        _PlayerInput = GetComponent<PlayerInput>();
        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
        Interaction = GetComponent<PlayerInteraction>();
        PlayerSFX = GetComponent<PlayerSFX>();
        FootStepSFX = GetComponent<FootStepSFX>();
        CamShake = GetComponent<CamShake>();
        PlayerCondition = GetComponent<PlayerCondition>();
        stateFactory = new StateFactory<Player>(this);
    }

    private void Start()
    {
        InitStateMachine();
        Controller.height = Input.StandHeight;
    }

    void Update()
    {
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        StateMachine.LateUpdate();
    }

    //--------------StateMachine 객체 생성 및 처음 상태 초기화--------------//
    private void InitStateMachine()
    {
        StateMachine = new StateMachine<Player>(this);
        StateMachine.ChangeState(StateFactory.Get<PlayerIdleState>());
    }

    public void Grap(ItemData itemData = null)
    {

		if (!IsGrap) //물건을 잡고 있지 않다면,
		{
			if (IsEquip) InventoryController.Instance.SetEquipped(false); //장비중이면 장비아이템 없애기 
            OnGrap(itemData);
			
			IsGrap = true;
		}
		else //TODO : 물건을 잡고 있다면
		{
            //아이템에서 
			Instantiate(EquipItemData.dropPrefab, target.position, Quaternion.identity);
			Destroy(EquipItem);
			IsGrap = false;
		}
		
	}
    public void OnGrap(ItemData itemData)
    {
        EquipItem = Instantiate(itemData.equipPrefab.gameObject, target);
        EquipItemData = itemData; //아이템 정보
    }
}
