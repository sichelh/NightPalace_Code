using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    private Player player;

    [field: SerializeField] private Image hpCircle;
    [field: SerializeField] private Image staminaCircle;
    [field: SerializeField] private Image infectionCircle;

    [field: SerializeField] private Image hpIcon;
    [field: SerializeField] private Image staminaIcon;
    [field: SerializeField] private Image infectionIcon;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }

    public void UpdateStatsUI()
    {
        hpCircle.fillAmount = player.PlayerCondition.hp / player.PlayerCondition.MaxHp;
        staminaCircle.fillAmount = player.PlayerCondition.stamina / player.PlayerCondition.MaxStamina;
        infectionCircle.fillAmount = player.PlayerCondition.infection / player.PlayerCondition.MaxInfection;

        if(player.PlayerCondition.hp < 50f)
        {
            hpIcon.color = new Color(150f / 255f, 0f, 0f, 50f / 255f);
        }
        else
        {
            hpIcon.color = new Color(1f, 1f, 1f, 50f / 255f);
        }

        if (player.PlayerCondition.stamina < 50f)
        {
            staminaIcon.color = new Color(255f, 200f / 255f, 0f, 50f / 255f);
        }
        else
        {
            staminaIcon.color = new Color(1f, 1f, 1f, 50f / 255f);
        }

        if (player.PlayerCondition.infection > 50f)
        {
            infectionIcon.color = new Color(0f, 150f / 255f, 0f, 50f / 255f);
        }
        else
        {
            infectionIcon.color = new Color(1f, 1f, 1f, 50f / 255f);
        }
    }
}
