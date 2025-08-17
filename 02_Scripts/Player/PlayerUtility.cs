using UnityEngine;

public static class PlayerUtility
{
    // 플레이어의 이동
    public static void Move(Player player, float speed, float verticalVelocity)
    {
        Vector3 moveDir = (player.transform.forward * player.Input.MoveInput.y) + (player.transform.right * player.Input.MoveInput.x).normalized;
        moveDir *= speed;
        moveDir.y = verticalVelocity;
                
        player.Controller.Move(moveDir * Time.deltaTime);
    }

    private static float CamXRot;
    // 플레이어의 시점 이동
    public static void Look(Player player)
    {
        if (player.Input.CamContainer == null)
        {
            Debug.LogError("CamContainer is null");
            return;
        }

        CamXRot += player.Input.MouseInput.y * player.Input.MouseSensitivity;
        CamXRot = Mathf.Clamp(CamXRot, -90f, 90f);
        player.Input.CamContainer.localEulerAngles = new Vector3(-CamXRot, 0f, 0f);
        player.transform.eulerAngles += new Vector3(0f, player.Input.MouseInput.x * player.Input.MouseSensitivity, 0f);
    }
}

