using System;
using System.Collections.Generic;
using UnityEngine;

public class StartChest : MonoBehaviour
{
	private void Start()
	{
		this.rand = new ConsistentRandom(GameManager.GetSeed());
		Chest componentInChildren = base.GetComponentInChildren<Chest>();
		List<InventoryItem> list = new List<InventoryItem>();
		foreach (InventoryItem item in this.loot.GetLoot(this.rand))
		{
			list.Add(item);
		}
		componentInChildren.InitChest(list, this.rand);
	}

	public LootDrop loot;

	private ConsistentRandom rand;
}
