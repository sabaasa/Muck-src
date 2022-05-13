﻿using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class Player
{
	public Player(int id, string username, Color color)
	{
		this.id = id;
		this.username = username;
		this.currentHp = 100;
		this.dead = false;
		this.powerups = new int[ItemManager.Instance.allPowerups.Count];
		this.armor = new int[4];
		for (int i = 0; i < this.armor.Length; i++)
		{
			this.armor[i] = -1;
		}
		this.InitStats();
	}

	public Player(int id, string username, Color color, SteamId steamId)
	{
		this.id = id;
		this.username = username;
		this.steamId = steamId;
		this.currentHp = 100;
		this.dead = false;
		this.powerups = new int[ItemManager.Instance.allPowerups.Count];
		this.armor = new int[4];
		for (int i = 0; i < this.armor.Length; i++)
		{
			this.armor[i] = -1;
		}
		this.InitStats();
	}

	private void InitStats()
	{
		this.stats = new Dictionary<string, int>();
		foreach (string key in Player.allStats)
		{
			this.stats.Add(key, 0);
		}
	}

	public void PingPlayer()
	{
		this.lastPingTime = Time.time;
	}

	public void UpdateArmor(int armorSlot, int itemId)
	{
		Debug.Log(string.Concat(new object[]
		{
			"slot: ",
			armorSlot,
			", itemid: ",
			itemId
		}));
		this.armor[armorSlot] = itemId;
		this.totalArmor = 0;
		foreach (int num in this.armor)
		{
			if (num != -1)
			{
				this.totalArmor += ItemManager.Instance.allItems[num].armor;
			}
		}
	}

	public void Died()
	{
		this.currentHp = 0;
		this.dead = true;
	}

	public int Damage(int damageDone)
	{
		this.currentHp -= damageDone;
		if (this.currentHp < 0)
		{
			this.currentHp = 0;
		}
		return this.currentHp;
	}

	public int id;

	public string username;

	public bool ready;

	public bool joined;

	public bool loading;

	public Color color;

	public Vector3 pos;

	public float yOrientation;

	public float xOrientation;

	public bool running;

	public bool dead;

	public Dictionary<string, int> stats;

	public float lastPingTime;

	public int[] powerups;

	public int[] armor;

	public int totalArmor;

	public SteamId steamId;

	public static string[] allStats = new string[]
	{
		"Kills",
		"Deaths",
		"Revives",
		"DamageDone",
		"DamageTaken",
		"Day",
		"Powerups",
		"Chests",
		"Gold collected"
	};

	public int currentHp;
}
