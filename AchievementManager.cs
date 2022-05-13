﻿using System;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
	private void Awake()
	{
		AchievementManager.Instance = this;
		SteamUserStats.OnAchievementProgress += this.AchievementChanged;
	}

	private void Start()
	{
		this.GameStarted();
	}

	private void AchievementChanged(Achievement ach, int currentProgress, int progress)
	{
		if (ach.State)
		{
			ach.Trigger(true);
			Debug.Log(ach.Name + " WAS UNLOCKED!");
		}
	}

	public void CheckGameOverAchievements(int endState)
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		bool onlyRock = GameManager.instance.onlyRock;
		bool damageTaken = GameManager.instance.damageTaken;
		bool powerupsPickedup = GameManager.instance.powerupsPickedup;
		int currentDay = GameManager.instance.currentDay;
		bool gameMode = GameManager.gameSettings.gameMode != GameSettings.GameMode.Survival;
		GameSettings.Difficulty difficulty = GameManager.gameSettings.difficulty;
		if (!gameMode && endState == -3)
		{
			switch (difficulty)
			{
			case GameSettings.Difficulty.Easy:
				SteamUserStats.AddStat("WinsEasy", 1);
				Debug.Log("Game finished on Easy: " + SteamUserStats.GetStatInt("WinsEasy"));
				break;
			case GameSettings.Difficulty.Normal:
				SteamUserStats.AddStat("WinsNormal", 1);
				Debug.Log("Game finished on normal: " + SteamUserStats.GetStatInt("WinsNormal"));
				break;
			case GameSettings.Difficulty.Gamer:
				SteamUserStats.AddStat("WinsGamer", 1);
				Debug.Log("Game finished on Gamer: " + SteamUserStats.GetStatInt("WinsGamer"));
				if (currentDay < 10)
				{
					SteamUserStats.AddStat("GamerMove", 1);
					Debug.Log("Game finished on Gamer in less than 10 days: " + SteamUserStats.GetStatInt("GamerMove"));
				}
				break;
			}
			if (currentDay < 8)
			{
				SteamUserStats.AddStat("Speedrunner", 1);
				Debug.Log("Game finished in less than 8 days: " + SteamUserStats.GetStatInt("Speedrunner"));
			}
			if (!powerupsPickedup)
			{
				SteamUserStats.AddStat("NoPowerups", 1);
				Debug.Log("Game finished without powerups: " + SteamUserStats.GetStatInt("NoPowerups"));
			}
			if (!damageTaken && difficulty >= GameSettings.Difficulty.Normal)
			{
				int nPlayers = NetworkController.Instance.nPlayers;
				switch (nPlayers)
				{
				case 1:
					SteamUserStats.AddStat("Untouchable", 1);
					Debug.Log("game finished without taking damage 1: " + SteamUserStats.GetStatInt("Untouchable"));
					break;
				case 2:
					SteamUserStats.AddStat("Dream Team", 1);
					Debug.Log("game finished without taking  2: " + SteamUserStats.GetStatInt("Dream Team"));
					break;
				case 3:
					break;
				case 4:
					SteamUserStats.AddStat("The bois", 1);
					Debug.Log("game finished without taking damage 4: " + SteamUserStats.GetStatInt("The bois"));
					break;
				default:
					if (nPlayers == 8)
					{
						SteamUserStats.AddStat("Sweat and tears", 1);
						Debug.Log("game finished without taking damage 8: " + SteamUserStats.GetStatInt("Sweat and tears"));
					}
					break;
				}
			}
			if (onlyRock)
			{
				SteamUserStats.AddStat("Caveman", 1);
				Debug.Log("game finished using only a rock: " + SteamUserStats.GetStatInt("Caveman"));
			}
			if (difficulty >= GameSettings.Difficulty.Normal && onlyRock && !damageTaken && !powerupsPickedup)
			{
				SteamUserStats.AddStat("Muck", 1);
				Debug.Log("Literally did the impossible: " + SteamUserStats.GetStatInt("Muck"));
			}
			SteamUserStats.AddStat("GamesWon", 1);
			Debug.Log("GamesWon: " + SteamUserStats.GetStatInt("GamesWon"));
		}
		SteamUserStats.StoreStats();
	}

	public void LeaveMuck()
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		SteamUserStats.AddStat("Set sail", 1);
		Debug.Log("Leaving muck. Left: " + SteamUserStats.GetStatInt("Set sail"));
		SteamUserStats.StoreStats();
	}

	public void AddKill(PlayerStatus.WeaponHitType type, Mob mob)
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		if (mob.countedKill)
		{
			return;
		}
		mob.countedKill = true;
		bool flag = mob.IsBuff();
		string name = mob.mobType.name;
		Debug.Log("Is buff: " + flag.ToString());
		SteamUserStats.AddStat("Kills", 1);
		SteamUserStats.AddStat("TotalKills", 1);
		if (type == PlayerStatus.WeaponHitType.Ranged)
		{
			SteamUserStats.AddStat("BowKills", 1);
		}
		if (flag)
		{
			Debug.Log("Is buff so adding kills");
			SteamUserStats.AddStat("BuffKills", 1);
		}
		if (name == "Cow")
		{
			SteamUserStats.AddStat("Cow Kills", 1);
		}
		if (name == "Big Chunk")
		{
			SteamUserStats.AddStat("BigChunkKills", 1);
			this.CheckAllBossesKilled();
		}
		if (name == "Gronk")
		{
			SteamUserStats.AddStat("GronkKills", 1);
			this.CheckAllBossesKilled();
		}
		if (name == "Guardian")
		{
			SteamUserStats.AddStat("GuardianKills", 1);
			this.CheckAllBossesKilled();
		}
		if (name == "Chief")
		{
			SteamUserStats.AddStat("ChiefKills", 1);
			this.CheckAllBossesKilled();
		}
		if (name == "Goblin")
		{
			SteamUserStats.AddStat("GoblinKills", 1);
		}
		if (name == "Woodman")
		{
			SteamUserStats.AddStat("WoodmanKills", 1);
		}
		int statInt = SteamUserStats.GetStatInt("Kills");
		int statInt2 = SteamUserStats.GetStatInt("TotalKills");
		int statInt3 = SteamUserStats.GetStatInt("BowKills");
		int statInt4 = SteamUserStats.GetStatInt("BuffKills");
		int statInt5 = SteamUserStats.GetStatInt("Cow Kills");
		int statInt6 = SteamUserStats.GetStatInt("BigChunkKills");
		int statInt7 = SteamUserStats.GetStatInt("GronkKills");
		int statInt8 = SteamUserStats.GetStatInt("GuardianKills");
		int statInt9 = SteamUserStats.GetStatInt("GoblinKills");
		int statInt10 = SteamUserStats.GetStatInt("WoodmanKills");
		Debug.Log(string.Concat(new object[]
		{
			"Killcount: ",
			statInt,
			", allkills: ",
			statInt2,
			", bowkills: ",
			statInt3,
			", buffkills: ",
			statInt4,
			", Cow Kills: ",
			statInt5,
			", chunks: ",
			statInt6,
			", gronks: ",
			statInt7,
			", guardians: ",
			statInt8,
			", goblins: ",
			statInt9,
			"Woodman kills: ",
			statInt10
		}));
		SteamUserStats.StoreStats();
	}

	private void CheckAllBossesKilled()
	{
		int statInt = SteamUserStats.GetStatInt("BigChunkKills");
		int statInt2 = SteamUserStats.GetStatInt("GronkKills");
		int statInt3 = SteamUserStats.GetStatInt("GuardianKills");
		int statInt4 = SteamUserStats.GetStatInt("ChiefKills");
		if (statInt3 > 0 && statInt2 > 0 && statInt > 0 && statInt4 > 0)
		{
			SteamUserStats.AddStat("Fearless", 1);
			Debug.Log("All bosses killed: " + SteamUserStats.GetStatInt("Fearless"));
		}
	}

	public void StartBattleTotem()
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		SteamUserStats.AddStat("Battle totems started", 1);
		Debug.Log("battle totems started: " + SteamUserStats.GetStatInt("Battle totems started"));
		SteamUserStats.StoreStats();
	}

	public void ReviveTeammate()
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		SteamUserStats.AddStat("Revives", 1);
		Debug.Log("Revives: " + SteamUserStats.GetStatInt("Revives"));
		SteamUserStats.StoreStats();
	}

	public void AddDeath(PlayerStatus.DamageType deathCause)
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		Debug.Log("Cause of death: " + deathCause);
		if (deathCause == PlayerStatus.DamageType.Drown)
		{
			SteamUserStats.AddStat("Drown", 1);
			Debug.Log("Drowned: " + SteamUserStats.GetStatInt("Drown"));
		}
		SteamUserStats.AddStat("Deaths", 1);
		SteamUserStats.AddStat("TotalDeaths", 1);
		Debug.Log("Deaths: " + SteamUserStats.GetStatInt("Deaths"));
		Debug.Log("TotalDeaths: " + SteamUserStats.GetStatInt("TotalDeaths"));
		SteamUserStats.StoreStats();
	}

	public bool CanUseAchievements()
	{
		return SteamManager.Instance && SteamClient.IsValid;
	}

	public void OpenChest()
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		SteamUserStats.AddStat("Chests opened", 1);
		SteamUserStats.AddStat("TotalChestsOpened", 1);
		Debug.Log("Chests opened: " + SteamUserStats.GetStatInt("Chests opened"));
		SteamUserStats.AddStat("Chests opened", 1);
		SteamUserStats.StoreStats();
	}

	public void ItemCrafted(global::InventoryItem item, int craftAmount)
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		if (item.name == "Coin")
		{
			SteamUserStats.AddStat("CoinsCrafted", craftAmount);
			Debug.Log("Coins crafted: " + SteamUserStats.GetStatInt("CoinsCrafted"));
		}
		SteamUserStats.StoreStats();
	}

	public void BuildItem(int buildId)
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		SteamUserStats.AddStat("Builds", 1);
		Debug.Log("Builds: " + SteamUserStats.GetStatInt("Builds"));
		SteamUserStats.StoreStats();
	}

	public void NewDay(int currentDay)
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		int statInt = SteamUserStats.GetStatInt("Longest survived");
		if (currentDay > statInt)
		{
			SteamUserStats.SetStat("Longest survived", currentDay);
		}
		Debug.Log("Max sruvived days: " + SteamUserStats.GetStatInt("Longest survived"));
		SteamUserStats.StoreStats();
	}

	public void WieldedWeapon(global::InventoryItem item)
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		if (item.name == "Night Blade")
		{
			SteamUserStats.AddStat("The Black Swordsman", 1);
			Debug.Log("The Black Swordsman: " + SteamUserStats.GetStatInt("The Black Swordsman"));
		}
		SteamUserStats.StoreStats();
	}

	public void PickupPowerup(string powerupName)
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		if (powerupName == "Danis Milk" && PowerupInventory.Instance.GetAmount("Danis Milk") >= 10)
		{
			SteamUserStats.AddStat("Milkman", 1);
			Debug.Log("Milkman: " + SteamUserStats.GetStatInt("Milkman"));
		}
		SteamUserStats.StoreStats();
	}

	public void MoveDistance(int groundDist, int waterDist)
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		SteamUserStats.AddStat("Move Distance", groundDist);
		if (waterDist > 0)
		{
			SteamUserStats.AddStat("Swim distance", waterDist);
		}
		Debug.Log(string.Concat(new object[]
		{
			"Move dist: ",
			SteamUserStats.GetStatInt("Move Distance"),
			", added this one: ",
			groundDist
		}));
		Debug.Log(string.Concat(new object[]
		{
			"swim dist: ",
			SteamUserStats.GetStatInt("Swim distance"),
			", added this one: ",
			waterDist
		}));
		SteamUserStats.StoreStats();
	}

	public void EatFood(global::InventoryItem item)
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		if (item.name == "Gulpon Shroom")
		{
			SteamUserStats.AddStat("Red shrooms eaten", 1);
			Debug.Log("Red shrooms eaten: " + SteamUserStats.GetStatInt("Red shrooms eaten"));
		}
		SteamUserStats.StoreStats();
	}

	public void AddPlayerKill()
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		SteamUserStats.AddStat("Friendly Kills", 1);
		Debug.Log("Friendly Kills: " + SteamUserStats.GetStatInt("Friendly Kills"));
		SteamUserStats.StoreStats();
	}

	public void Jump()
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		SteamUserStats.AddStat("Jumps", 1);
		Debug.Log("Jumps: " + SteamUserStats.GetStatInt("Jumps"));
		SteamUserStats.StoreStats();
	}

	public void PickupItem(global::InventoryItem item)
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		global::InventoryItem[] array = this.gems;
		int i = 0;
		while (i < array.Length)
		{
			global::InventoryItem inventoryItem = array[i];
			if (item != null && inventoryItem.id == item.id)
			{
				Debug.Log("Found gem, testing");
				bool flag = true;
				foreach (global::InventoryItem inventoryItem2 in this.gems)
				{
					if (inventoryItem2.id != item.id && !InventoryUI.Instance.HasItem(inventoryItem2))
					{
						Debug.Log("Couldnt find item: " + inventoryItem2.name);
						flag = false;
						break;
					}
				}
				if (flag)
				{
					SteamUserStats.AddStat("AllGems", 1);
					Debug.Log("AllGems: " + SteamUserStats.GetStatInt("AllGems"));
					break;
				}
				break;
			}
			else
			{
				i++;
			}
		}
		SteamUserStats.StoreStats();
	}

	public void Karlson()
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		SteamUserStats.AddStat("Karlson monitor", 1);
		Debug.Log("Karlson monitor: " + SteamUserStats.GetStatInt("Karlson monitor"));
		SteamUserStats.StoreStats();
	}

	private void GameStarted()
	{
		SteamUserStats.AddStat("Muck started", 1);
		Debug.Log("Muck started: " + SteamUserStats.GetStatInt("Muck started"));
		SteamUserStats.StoreStats();
	}

	public void StartGame(GameSettings.Difficulty d)
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		SteamUserStats.AddStat("GamesStarted", 1);
		Debug.Log("GamesStarted: " + SteamUserStats.GetStatInt("GamesStarted"));
		switch (d)
		{
		case GameSettings.Difficulty.Easy:
			SteamUserStats.AddStat("Easy", 1);
			Debug.Log("Easy: " + SteamUserStats.GetStatInt("Easy"));
			break;
		case GameSettings.Difficulty.Normal:
			SteamUserStats.AddStat("Normal", 1);
			Debug.Log("Normal: " + SteamUserStats.GetStatInt("Normal"));
			break;
		case GameSettings.Difficulty.Gamer:
			SteamUserStats.AddStat("Gamer", 1);
			Debug.Log("Gamer: " + SteamUserStats.GetStatInt("Gamer"));
			break;
		}
		SteamUserStats.StoreStats();
	}

	public void OpenChiefChest()
	{
		if (!this.CanUseAchievements())
		{
			return;
		}
		SteamUserStats.AddStat("ChiefChests", 1);
		Debug.Log("ChiefChests: " + SteamUserStats.GetStatInt("ChiefChests"));
		SteamUserStats.StoreStats();
	}

	public static AchievementManager Instance;

	public global::InventoryItem[] gems;
}
