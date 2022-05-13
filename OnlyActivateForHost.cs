using System;
using Steamworks;
using UnityEngine;

public class OnlyActivateForHost : MonoBehaviour
{
	public void Kick()
	{
		using (Packet packet = new Packet(6))
		{
			ServerSend.SendTCPDataToSteamId(this.steamId, packet);
		}
	}

	private void OnEnable()
	{
	}

	public GameObject kickBtn;

	public SteamId steamId;
}
