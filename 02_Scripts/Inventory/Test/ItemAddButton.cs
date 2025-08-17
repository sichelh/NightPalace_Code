using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private Button addButton;
    [SerializeField] private Button useButton;
    [SerializeField] private Button startWalkParticleButton;
    [SerializeField] private Button stopWalkParticleButton;
    [SerializeField] private Button startRunParticleButton;
    [SerializeField] private Button stopRunParticleButton;
    [SerializeField] private Button saveTestButton;
    [SerializeField] private ItemObject itemObject;

    private void Start()
    {
        addButton.onClick.AddListener(() => inventoryController.AddItem(itemObject.itemData));
        useButton.onClick.AddListener(() => inventoryController.UsableUse());
        startWalkParticleButton.onClick.AddListener(() => ParticleManager.Instance.StartParticle(ParticleType.Walk, Player.Instance.Target));
        stopWalkParticleButton.onClick.AddListener(() => ParticleManager.Instance.StopParticle(ParticleType.Walk));
        startRunParticleButton.onClick.AddListener(() => ParticleManager.Instance.StartParticle(ParticleType.Run, Player.Instance.Target));
        stopRunParticleButton.onClick.AddListener(() => ParticleManager.Instance.StopParticle(ParticleType.Run));
        saveTestButton.onClick.AddListener(() => SaveManager.Instance.SaveJson(Player.Instance ,"Test"));
    }
}
