using System;
using UnityEngine;

public class ContinousHitbox : MonoBehaviour
{
	private void Awake()
	{
		base.InvokeRepeating("ResetHitbox", this.resetTime, this.resetTime);
	}

	private void ResetHitbox()
	{
		this.hitbox.Reset();
	}

	public HitboxDamage hitbox;

	public float resetTime = 0.1f;
}
