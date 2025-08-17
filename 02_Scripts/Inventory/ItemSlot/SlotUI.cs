using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
	//[SerializeField] private TextMeshProUGUI itemText;
	//[SerializeField] private TextMesh
	[field : SerializeField] public string ItemName { get; set; }
	[field : SerializeField] public TextMeshProUGUI ItemText { get; private set; }
	[field : SerializeField] public TextMeshProUGUI ItemDescription { get; private set; }
	[field : SerializeField] public TextMeshProUGUI CurStat { get; private set; }
	[field: SerializeField] public TextMeshProUGUI ItemValue { get; private set; }
	[field: SerializeField] public Image Icon { get; private set; }
	[field: SerializeField] public Button ItemClick { get; private set; }

	private void Awake()
	{
		ItemClick.onClick.AddListener(() => InventoryController.Instance.SelectItem(ItemName));
	}
}
