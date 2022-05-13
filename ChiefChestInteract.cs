using System;
using UnityEngine;

public class ChiefChestInteract : ChestInteract
{
	protected override void WhenOpened()
	{
		if (this.alreadyOpened)
		{
			return;
		}
		this.alreadyOpened = true;
		foreach (GameObject gameObject in MobZoneManager.Instance.zones[this.mobZoneId].entities)
		{
			gameObject.GetComponent<WoodmanBehaviour>().MakeAggressive(false);
		}
		if (AchievementManager.Instance)
		{
			AchievementManager.Instance.OpenChiefChest();
		}
	}

	public bool alreadyOpened;

	public int mobZoneId;
}
