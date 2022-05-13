using System;
using Steamworks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPrefab : MonoBehaviour
{
	public void SetAchievement(Achievement a)
	{
		if (a.GetIcon() == null)
		{
			Debug.LogError("no img");
		}
		else
		{
			Steamworks.Data.Image value = a.GetIcon().Value;
			this.img.texture = AchievementPrefab.GetSteamImageAsTexture2D(value);
		}
		this.title.text = a.Name;
		this.desc.text = a.Description;
	}

	public static Texture2D GetSteamImageAsTexture2D(Steamworks.Data.Image img)
	{
		Texture2D texture2D = new Texture2D((int)img.Width, (int)img.Height, TextureFormat.RGBA32, false, true);
		texture2D.LoadRawTextureData(img.Data);
		texture2D.Apply();
		return texture2D;
	}

	public RawImage img;

	public TextMeshProUGUI title;

	public TextMeshProUGUI desc;
}
