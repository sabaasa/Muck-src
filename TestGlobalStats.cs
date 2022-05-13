using System;
using System.Threading.Tasks;
using Steamworks;
using UnityEngine;

public class TestGlobalStats : MonoBehaviour
{
	private void Start()
	{
		if (SteamUserStats.RequestCurrentStats())
		{
			this.a = SteamUserStats.RequestGlobalStatsAsync(60);
			Debug.LogError("REquesting global stats");
		}
		SteamUserStats.OnUserStatsReceived += this.ReceivedStats;
	}

	private void ReceivedStats(SteamId id, Result r)
	{
	}

	private Task<Result> a;
}
