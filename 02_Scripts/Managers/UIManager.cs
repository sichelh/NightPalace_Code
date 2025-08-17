using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [field: SerializeField] public InteractionUI InteractionUI { get; private set; }
    [field: SerializeField] public MenuUI MenuUI { get; private set; }
    [field: SerializeField] public SettingUI SettingUI { get; private set; }
    [field: SerializeField] public QuitUI QuitUI { get; private set; }
    [field: SerializeField] public InventoryUI InventoryUI { get; private set; }
    [field: SerializeField] public DeadUI DeadUI { get; private set; }
    [field: SerializeField] public StatsUI StatUI { get; private set; }
    [field: SerializeField] public HitPanel HitPanel { get; private set; }
    [field: SerializeField] public BookUI BookUI { get; private set; }
    [field: SerializeField] public SoundUI SoundUI { get; private set; }

    private void Start()
    {
        StatUI.gameObject.SetActive(true);
        InteractionUI.gameObject.SetActive(true);
        DeadUI.gameObject.SetActive(false);
    }

    public void ToggleMenuUI()
    {
        if (MenuUI.gameObject.activeSelf)
        {
            MenuUI.gameObject.SetActive(false);
            SoundUI.gameObject.SetActive(true);
        }
        else
        {
            MenuUI.gameObject.SetActive(true);
            SoundUI.gameObject.SetActive(false);
        }
    }

    public void ToggleSettingUI()
    {
        if (SettingUI.gameObject.activeSelf)
        {
            SettingUI.gameObject.SetActive(false);
            MenuUI.gameObject.SetActive(true);
        }
        else
        {
            SettingUI.gameObject.SetActive(true);
            MenuUI.gameObject.SetActive(false);
        }
    }
    public void ToggleQuitUI()
    {
        if (QuitUI.gameObject.activeSelf)
        {
            QuitUI.gameObject.SetActive(false);
            MenuUI.gameObject.SetActive(true);
        }
        else
        {
            QuitUI.gameObject.SetActive(true);
            MenuUI.gameObject.SetActive(false);
        }
    }

    public void ToggleInventoryUI()
    {
        InventoryUI.gameObject.SetActive(!InventoryUI.gameObject.activeSelf);
    }

    public void ShowDeadUI()
    {
        DeadUI.gameObject.SetActive(true);

        InteractionUI.gameObject.SetActive(false);
        MenuUI.gameObject.SetActive(false);
        SettingUI.gameObject.SetActive(false);
        QuitUI.gameObject.SetActive(false);
        InventoryUI.gameObject.SetActive(false);
        StatUI.gameObject.SetActive(false);
        BookUI.gameObject.SetActive(false);
    }

    public void HideDeadUI()
    {
        DeadUI.gameObject.SetActive(false);
        HitPanel.gameObject.SetActive(false);
    }

    public void ShowBookUI()
    {
        BookUI.gameObject.SetActive(true);
    }
}
