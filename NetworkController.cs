﻿using System;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviour
{
	public bool loading { get; set; }

	private void Awake()
	{
		if (NetworkController.Instance)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		NetworkController.Instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void LoadGame(string[] names)
	{
		this.loading = true;
		this.playerNames = names;
		LoadingScreen.Instance.Show(1f);
		base.Invoke("StartLoadingScene", LoadingScreen.Instance.totalFadeTime);
	}

	private void StartLoadingScene()
	{
		SceneManager.LoadScene("GameAfterLobby");
	}

	public NetworkController.NetworkType networkType;

	public GameObject steam;

	public GameObject classic;

	public int nPlayers;

	public static int maxPlayers = 10;

	public Lobby lobby;

	public string[] playerNames;

	public static NetworkController Instance;

	public enum NetworkType
	{
		Steam,
		Classic
	}
}
