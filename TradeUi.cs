using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeUi : MonoBehaviour
{
	public void SetTrade(WoodmanTrades.Trade t, bool buy)
	{
		this.name.text = string.Format("{0} (x{1})", t.item.name, t.amount);
		this.price.text = string.Concat(t.price);
		this.itemIcon.texture = t.item.sprite.texture;
		if (buy)
		{
			if (InventoryUI.Instance.GetMoney() < t.price)
			{
				this.overlay.SetActive(true);
			}
			else
			{
				this.overlay.SetActive(false);
			}
		}
		else
		{
			InventoryItem inventoryItem = Object.Instantiate<InventoryItem>(t.item);
			inventoryItem.amount = t.amount;
			if (InventoryUI.Instance.HasItem(inventoryItem))
			{
				this.overlay.SetActive(false);
			}
			else
			{
				this.overlay.SetActive(true);
			}
		}
		this.trade = t;
		this.buy = buy;
		if (buy)
		{
			this.buyText.text = "Buy";
			return;
		}
		this.buyText.text = "Sell";
	}

	public void BuySell()
	{
		if (this.buy)
		{
			if (InventoryUI.Instance.GetMoney() < this.trade.price)
			{
				return;
			}
			InventoryItem inventoryItem = Object.Instantiate<InventoryItem>(this.trade.item);
			inventoryItem.amount = this.trade.amount;
			if (!InventoryUI.Instance.CanPickup(inventoryItem))
			{
				return;
			}
			InventoryUI.Instance.UseMoney(this.trade.price);
			InventoryUI.Instance.AddItemToInventory(inventoryItem);
			if (InventoryUI.Instance.GetMoney() < this.trade.price)
			{
				this.overlay.SetActive(true);
				return;
			}
			this.overlay.SetActive(false);
			return;
		}
		else
		{
			InventoryItem inventoryItem2 = Object.Instantiate<InventoryItem>(this.trade.item);
			inventoryItem2.amount = this.trade.amount;
			if (!InventoryUI.Instance.HasItem(inventoryItem2))
			{
				return;
			}
			InventoryUI.Instance.RemoveItem(inventoryItem2);
			InventoryItem inventoryItem3 = Object.Instantiate<InventoryItem>(ItemManager.Instance.GetItemByName("Coin"));
			inventoryItem3.amount = this.trade.price;
			InventoryUI.Instance.AddItemToInventory(inventoryItem3);
			if (InventoryUI.Instance.HasItem(inventoryItem2))
			{
				this.overlay.SetActive(false);
				return;
			}
			this.overlay.SetActive(true);
			return;
		}
	}

	public new TextMeshProUGUI name;

	public TextMeshProUGUI price;

	public TextMeshProUGUI buyText;

	public RawImage itemIcon;

	public GameObject overlay;

	public GameObject button;

	private WoodmanTrades.Trade trade;

	private bool buy;
}
