using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameoverUI : MonoBehaviour
{
	private void Awake()
	{
		this.HeaderText();
		this.InitStats();
		this.FillStats();
	}

	private void InitStats()
	{
		for (int i = 0; i < Player.allStats.Length; i++)
		{
			this.statPrefabs.Add(Object.Instantiate<GameObject>(this.statPrefab, this.statsParent).GetComponent<StatPrefab>());
		}
	}

	private void FillStats()
	{
		int num = this.page;
		Dictionary<string, int> dictionary = GameManager.instance.stats[num];
		int num2 = 0;
		foreach (KeyValuePair<string, int> stat in dictionary)
		{
			if (num2 == 0)
			{
				this.nameText.text = (GameManager.players[stat.Value].username ?? "");
			}
			else
			{
				this.statPrefabs[num2 - 1].SetStat(stat);
			}
			num2++;
		}
	}

	public void FlipPage(int dir)
	{
		if (dir < 0 && this.page <= 0)
		{
			return;
		}
		if (dir > 0 && this.page >= GameManager.instance.nStatsPlayers - 1)
		{
			return;
		}
		this.page += dir;
		this.FillStats();
	}

	private void HeaderText()
	{
		int winnerId = GameManager.instance.winnerId;
		if (winnerId == -3)
		{
			this.header.text = "Victory!";
			return;
		}
		if (winnerId == -2)
		{
			this.header.text = "Defeat..";
			return;
		}
		if (winnerId == -1)
		{
			this.header.text = "Draw...";
			return;
		}
		string text = GameManager.players[winnerId].username;
		text = GameoverUI.Truncate(text, 10);
		this.header.text = text + " won!";
	}

	public static string Truncate(string value, int maxLength)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		if (value.Length > maxLength)
		{
			return value.Substring(0, maxLength);
		}
		return value;
	}

	public TextMeshProUGUI header;

	public TextMeshProUGUI nameText;

	public GameObject statPrefab;

	public List<StatPrefab> statPrefabs;

	public Transform statsParent;

	private int page;
}
