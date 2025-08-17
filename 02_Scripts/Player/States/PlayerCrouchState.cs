using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerCrouchState : PlayerGroundState
{
    public override void Enter()
    {
        base.Enter();
        currentSpeed = owner.PlayerStateData.Ground.CrouchSpeed;

        owner.CamShake.Shake(owner.PlayerStateData.GroundCamShake.Noise,
            owner.PlayerStateData.GroundCamShake.AmplitudeOnCrouch,
            owner.PlayerStateData.GroundCamShake.FrequencyOnCrouch);

        owner.StartCoroutine(Crouch());
        Debug.Log("Enter Crouch");
    }

    public override void Update()
    {
        Move();
        owner.PlayerCondition.IncreaseStaminaOnSprint();

        //장애물이 머리 위에 있을 시, 못 일어나게 함
        if (!owner.Input.CrouchPressed && DetectOverheadObstacle())
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerCrouchState>());
        }
        else if (!owner.Input.CrouchPressed && !DetectOverheadObstacle())
        {
            owner.StateMachine.ChangeState(owner.StateFactory.Get<PlayerIdleState>());
        }
    }

    public override void LateUpdate() { base.LateUpdate(); }

    public override void Exit()
    {
        owner.StartCoroutine(StandUp());
        Debug.Log("Exit Crouch");
    }

    //앉는 기능
    private IEnumerator Crouch()
    {
        float time = 0f;
        while(time < owner.PlayerStateData.Ground.CrouchDuration)
        {
            float t = time / owner.PlayerStateData.Ground.CrouchDuration;
            time += Time.deltaTime;

            owner.Input.CamContainer.position = Vector3.Lerp(owner.Input.CamContainer.position, owner.Input.CrouchCamPos.position, t);
            owner.Controller.height = Mathf.Lerp(owner.Controller.height, owner.Input.CrouchHeight, t);
            owner.Controller.center = Vector3.Lerp(owner.Controller.center, new Vector3(0f, -owner.Input.CrouchHeight / 2f, 0f), t);

            yield return null;
        }

        owner.Input.CamContainer.position = owner.Input.CrouchCamPos.position;
        owner.Controller.height = owner.Input.CrouchHeight;
        owner.Controller.center = new Vector3(0f, -owner.Input.CrouchHeight / 2f, 0f);
    }

    //일어서는 기능
    private IEnumerator StandUp()
    {
        float time = 0f;
        while (time < owner.PlayerStateData.Ground.CrouchDuration)
        {
            float t = time / owner.PlayerStateData.Ground.CrouchDuration;
            time += Time.deltaTime;

            owner.Input.CamContainer.position = Vector3.Lerp(owner.Input.CamContainer.position, owner.Input.StandCamPos.position, t);
            owner.Controller.height = Mathf.Lerp(owner.Controller.height, owner.Input.StandHeight, t);
            owner.Controller.center = Vector3.Lerp(owner.Controller.center, new Vector3(0f, 0f, 0f), t);

            yield return null;
        }

        owner.Input.CamContainer.position = owner.Input.StandCamPos.position;
        owner.Controller.height = owner.Input.StandHeight;
        owner.Controller.center = new Vector3(0f, 0f, 0f);
    }

    //머리 위 장애물 감지
    public bool DetectOverheadObstacle()
    {
        Ray[] ray = new Ray[4]
        {
            new Ray(owner.Input.CrouchCamPos.position + (owner.Input.CrouchCamPos.forward * 0.2f) + (owner.Input.CrouchCamPos.right * 0.2f), Vector3.up),
            new Ray(owner.Input.CrouchCamPos.position + (owner.Input.CrouchCamPos.forward * 0.2f) + (-owner.Input.CrouchCamPos.right * 0.2f), Vector3.up),
            new Ray(owner.Input.CrouchCamPos.position + (-owner.Input.CrouchCamPos.forward * 0.2f) + (owner.Input.CrouchCamPos.right * 0.2f), Vector3.up),
            new Ray(owner.Input.CrouchCamPos.position + (-owner.Input.CrouchCamPos.forward * 0.2f) + (-owner.Input.CrouchCamPos.right * 0.2f), Vector3.up),
        };

        RaycastHit hit;
        for (int i = 0; i < ray.Length; i++)
        {
            Debug.DrawRay(ray[i].origin, ray[i].direction);
            if (Physics.Raycast(ray[i], out hit, 0.5f, owner.Input.OverheadObstacleLayer))
            {
                return true;
            }
        }
        return false;
    }
}
