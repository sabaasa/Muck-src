using System;
using System.Linq;
using UnityEngine;

public class TestAchievements : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		this.FillWildcard();
	}

	private void FillWildcard()
	{
		WoodmanTrades.Trade[] array = new WoodmanTrades.Trade[0];
		foreach (WoodmanTrades woodmanTrades in this.ye)
		{
			array = array.Concat(woodmanTrades.trades).ToArray<WoodmanTrades.Trade>();
		}
		this.wildcard.trades = new WoodmanTrades.Trade[array.Length];
		for (int j = 0; j < array.Length; j++)
		{
			WoodmanTrades.Trade trade = array[j];
			WoodmanTrades.Trade trade2 = new WoodmanTrades.Trade();
			trade2.amount = trade.amount;
			trade2.item = trade.item;
			trade2.price = trade.price;
			this.wildcard.trades[j] = trade2;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			GameManager.instance.GameOver(-3, 4f);
		}
	}

	public WoodmanTrades[] ye;

	public WoodmanTrades wildcard;
}
