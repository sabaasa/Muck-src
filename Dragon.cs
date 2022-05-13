using System;
using UnityEngine;

public class Dragon : MonoBehaviour
{
	private void Awake()
	{
		Dragon.Instance = this;
		base.transform.rotation = Quaternion.LookRotation(Vector3.up);
	}

	private void Start()
	{
		MusicController.Instance.FinalBoss();
	}

	public void PlayWingFlap()
	{
		this.wingFlap.Randomize(0f);
	}

	private void OnDestroy()
	{
		Debug.LogError("Game is over lol");
		Object.Instantiate<GameObject>(this.roar, base.transform.position, Quaternion.identity);
		if (LocalClient.serverOwner)
		{
			GameManager.instance.GameOver(-3, 8f);
			ServerSend.GameOver(-3);
		}
	}

	public RandomSfx wingFlap;

	public GameObject roar;

	public static Dragon Instance;
}
