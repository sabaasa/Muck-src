using System;
using UnityEngine;

public class TradesManager : MonoBehaviour
{
	private void Awake()
	{
		TradesManager.Instance = this;
	}

	public WoodmanTrades archerTrades;

	public WoodmanTrades chefTrades;

	public WoodmanTrades smithTrades;

	public WoodmanTrades woodTrades;

	public WoodmanTrades wildcardTrades;

	public static TradesManager Instance;
}
