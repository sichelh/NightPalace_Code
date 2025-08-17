using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookUI : MonoBehaviour
{
    [SerializeField] private Button amulet;
    [SerializeField] private Button backButton;
    [SerializeField] private ItemData amuletData;

    [SerializeField] public BookObject bookObject;

    Player player;
    private bool isKey = false;
    public void SetKey(bool value)
    {
        isKey = value;
    }
    private void OnEnable()
    {
        AudioManager.Instance.BookPageTurnPlay();
        amulet.gameObject.SetActive(isKey);
        backButton.gameObject.SetActive(true);

        player = FindObjectOfType<Player>();

        if (player != null)
        {
            player.StateMachine.ChangeState(player.StateFactory.Get<PlayerBookState>());
        }
    }

    void Start()
    {
        amulet.onClick.AddListener(() =>
        {
            amulet.gameObject.SetActive(false);
            ChapterManager.Instance.AddIndexCount(6, 7, 0);
            AudioManager.Instance.BookPageTurnPlay();
            InventoryController.Instance.AddItem(amuletData);
            bookObject.itemData = null;
            // 안 먹어짐
        });

        backButton.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
            player.StateMachine.ChangeState(player.StateFactory.Get<PlayerIdleState>());
        });
    }
}
