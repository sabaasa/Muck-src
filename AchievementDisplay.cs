using System;
using System.Linq;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

public class AchievementDisplay : MonoBehaviour
{
	private void OnEnable()
	{
		this.currentPage = 0;
		if (this.achievements.Length < 1)
		{
			this.achievements = SteamUserStats.Achievements.ToArray<Achievement>();
		}
		this.nAchievements = this.achievements.Length;
		this.nPages = Mathf.FloorToInt((float)this.nAchievements / (float)this.achievementsPerPage);
		this.LoadPage(this.currentPage);
	}

	private void LoadPage(int page)
	{
		for (int i = this.achievementParent.childCount - 1; i >= 0; i--)
		{
			Object.Destroy(this.achievementParent.GetChild(i).gameObject);
		}
		int num = this.achievementsPerPage * page;
		for (int j = num; j < this.achievements.Length; j++)
		{
			Object.Instantiate<GameObject>(this.achievementPrefab, this.achievementParent).GetComponent<AchievementPrefab>().SetAchievement(this.achievements[j]);
			if (j >= num + this.achievementsPerPage - 1)
			{
				break;
			}
		}
	}

	public void NextPage(int dir)
	{
		if (dir < 0 && this.currentPage == 0)
		{
			return;
		}
		if (dir > 0 && this.currentPage >= this.nPages)
		{
			return;
		}
		this.currentPage += dir;
		this.LoadPage(this.currentPage);
	}

	public GameObject achievementPrefab;

	public Transform achievementParent;

	private int achievementsPerPage = 8;

	private int nAchievements;

	private int nPages;

	private int currentPage;

	private Achievement[] achievements = new Achievement[0];

	public enum WinState
	{
		Won = -3,
		Lost,
		Draw
	}
}
