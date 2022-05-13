using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TraderUI : MonoBehaviour
{
	private void Awake()
	{
		TraderUI.Instance = this;
	}

	public void SetTrades(List<WoodmanTrades.Trade> buy, List<WoodmanTrades.Trade> sell, WoodmanBehaviour.WoodmanType type)
	{
		if (this.root.activeInHierarchy)
		{
			return;
		}
		this.buy = buy;
		this.sell = sell;
		this.title.text = type.ToString() + " Trader";
		this.OpenBuy();
		this.Show();
	}

	public void FillTrades()
	{
		for (int i = this.listParent.childCount - 1; i >= 0; i--)
		{
			Object.Destroy(this.listParent.GetChild(i).gameObject);
		}
		if (this.buying)
		{
			using (List<WoodmanTrades.Trade>.Enumerator enumerator = this.buy.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WoodmanTrades.Trade t = enumerator.Current;
					Object.Instantiate<GameObject>(this.tradePrefab, this.listParent).GetComponent<TradeUi>().SetTrade(t, true);
				}
				return;
			}
		}
		foreach (WoodmanTrades.Trade t2 in this.sell)
		{
			Object.Instantiate<GameObject>(this.tradePrefab, this.listParent).GetComponent<TradeUi>().SetTrade(t2, false);
		}
	}

	public void Show()
	{
		OtherInput.Instance.ToggleInventory(OtherInput.CraftingState.Inventory);
		this.root.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void Hide()
	{
		this.root.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		OtherInput.Instance.ToggleInventory(OtherInput.CraftingState.Inventory);
	}

	public void OpenBuy()
	{
		this.buying = true;
		this.FillTrades();
	}

	public void OpenSell()
	{
		this.buying = false;
		this.FillTrades();
	}

	public TextMeshProUGUI title;

	private List<WoodmanTrades.Trade> buy;

	private List<WoodmanTrades.Trade> sell;

	public Transform listParent;

	public GameObject tradePrefab;

	public GameObject root;

	private bool buying = true;

	public static TraderUI Instance;
}
