using System;
using UnityEngine;

public class DragonFire : MonoBehaviour
{
	private void Awake()
	{
		base.InvokeRepeating("UpdateHitbox", 0.1f, 0.1f);
		base.Invoke("StartHitbox", 1.35f);
		this.c = base.GetComponent<Collider>();
		this.c.enabled = false;
	}

	private void StartHitbox()
	{
		base.Invoke("StopHitbox", 1.5f);
		this.c.enabled = true;
	}

	private void StopHitbox()
	{
		this.c.enabled = false;
	}

	private void UpdateHitbox()
	{
		this.hitbox.Reset();
	}

	private void Update()
	{
		Vector3 euler = new Vector3(0f, base.transform.parent.rotation.eulerAngles.y, 0f);
		base.transform.rotation = Quaternion.Euler(euler);
	}

	private Collider c;

	public HitboxDamage hitbox;

	private float yHeight;
}
