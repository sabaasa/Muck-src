using System;
using System.Collections.Generic;
using UnityEngine;

public class TraderInteract : MonoBehaviour, SharedObject, Interactable
{
	public void SetType(WoodmanBehaviour.WoodmanType type, ConsistentRandom rand)
	{
		this.type = type;
		switch (type)
		{
		case WoodmanBehaviour.WoodmanType.Archer:
			this.GenerateTrades(TradesManager.Instance.archerTrades, rand);
			return;
		case WoodmanBehaviour.WoodmanType.Smith:
			this.GenerateTrades(TradesManager.Instance.smithTrades, rand);
			return;
		case WoodmanBehaviour.WoodmanType.Woodcutter:
			this.GenerateTrades(TradesManager.Instance.woodTrades, rand);
			return;
		case WoodmanBehaviour.WoodmanType.Chef:
			this.GenerateTrades(TradesManager.Instance.chefTrades, rand);
			return;
		case WoodmanBehaviour.WoodmanType.Wildcard:
			this.GenerateTrades(TradesManager.Instance.wildcardTrades, rand);
			return;
		default:
			this.GenerateTrades(TradesManager.Instance.archerTrades, rand);
			return;
		}
	}

	private void GenerateTrades(WoodmanTrades trades, ConsistentRandom rand)
	{
		trades = Object.Instantiate<WoodmanTrades>(trades);
		this.buy = trades.GetTrades(5, 10, rand, 1f);
		this.sell = trades.GetTrades(5, 10, rand, 0.5f);
	}

	private void GenerateWildcardTrades()
	{
	}

	public void SetId(int id)
	{
		this.id = id;
	}

	public int GetId()
	{
		return this.id;
	}

	public void Interact()
	{
		TraderUI.Instance.SetTrades(this.buy, this.sell, this.type);
	}

	public void LocalExecute()
	{
		throw new NotImplementedException();
	}

	public void AllExecute()
	{
		throw new NotImplementedException();
	}

	public void ServerExecute(int fromClient = -1)
	{
		throw new NotImplementedException();
	}

	public void RemoveObject()
	{
		throw new NotImplementedException();
	}

	public string GetName()
	{
		return string.Format("Press {0} to trade with {1}", InputManager.interact, this.type);
	}

	public bool IsStarted()
	{
		throw new NotImplementedException();
	}

	private int id;

	private WoodmanBehaviour.WoodmanType type;

	private List<WoodmanTrades.Trade> buy;

	private List<WoodmanTrades.Trade> sell;
}
