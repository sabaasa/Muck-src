using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WoodmanTrades : ScriptableObject
{
	public List<WoodmanTrades.Trade> GetTrades(int min, int max, ConsistentRandom rand, float priceMultiplier = 1f)
	{
		List<WoodmanTrades.Trade> list = new List<WoodmanTrades.Trade>();
		List<WoodmanTrades.Trade> list2 = new List<WoodmanTrades.Trade>();
		foreach (WoodmanTrades.Trade item in this.trades)
		{
			list2.Add(item);
		}
		int num = rand.Next(min, max);
		int num2 = 0;
		while (num2 < num && num2 < this.trades.Length)
		{
			WoodmanTrades.Trade trade = list2[rand.Next(0, list2.Count)];
			list.Add(new WoodmanTrades.Trade
			{
				amount = trade.amount,
				item = trade.item,
				price = trade.price,
				price = (int)(priceMultiplier * (float)trade.price)
			});
			list2.Remove(trade);
			num2++;
		}
		list.Sort();
		return list;
	}

	public WoodmanTrades.Trade[] trades;

	[Serializable]
	public class Trade : IComparable
	{
		public int CompareTo(object obj)
		{
			WoodmanTrades.Trade trade = (WoodmanTrades.Trade)obj;
			if (this.item.id > trade.item.id)
			{
				return 1;
			}
			if (this.item.id < trade.item.id)
			{
				return -1;
			}
			return 0;
		}

		public InventoryItem item;

		public int price;

		public int amount;
	}
}
