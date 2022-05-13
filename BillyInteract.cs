using System;
using UnityEngine;

public class BillyInteract : MonoBehaviour, SharedObject, Interactable
{
	public void SetId(int id)
	{
		this.id = id;
	}

	public int GetId()
	{
		return this.id;
	}

	public void Interact()
	{
		Application.OpenURL("https://store.steampowered.com/app/1228610/KARLSON/");
		AchievementManager.Instance.Karlson();
	}

	public void LocalExecute()
	{
	}

	public void AllExecute()
	{
	}

	public void ServerExecute(int fromClient = -1)
	{
	}

	public void RemoveObject()
	{
	}

	public string GetName()
	{
		return string.Format("<size=40%>Press {0} to wishlist KARLSON now gamer!", InputManager.interact);
	}

	public bool IsStarted()
	{
		return false;
	}

	public int id;
}
