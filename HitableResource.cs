﻿using System;
using UnityEngine;

public class HitableResource : Hitable
{
	protected void Start()
	{
		Material material = base.GetComponentInChildren<Renderer>().materials[0];
		this.materialText = material.mainTexture;
	}

	public override void Hit(int damage, float sharpness, int hitEffect, Vector3 pos, int hitWeaponType)
	{
		if (damage > 0)
		{
			ClientSend.PlayerHitObject(damage, this.id, hitEffect, pos, hitWeaponType);
			return;
		}
		Vector3 vector = GameManager.players[LocalClient.instance.myId].transform.position + Vector3.up * 1.5f;
		Vector3 normalized = (vector - pos).normalized;
		pos = this.hitCollider.ClosestPoint(vector);
		this.SpawnParticles(pos, normalized, hitEffect);
		float d = Vector3.Distance(pos, vector);
		pos += normalized * d * 0.5f;
		Object.Instantiate<GameObject>(this.numberFx, pos, Quaternion.identity).GetComponent<HitNumber>().SetTextAndDir(0f, normalized, (HitEffect)hitEffect);
	}

	protected override void SpawnDeathParticles()
	{
		Object.Instantiate<GameObject>(this.destroyFx, base.transform.position, this.destroyFx.transform.rotation).GetComponent<ParticleSystemRenderer>().material.mainTexture = this.materialText;
	}

	protected override void SpawnParticles(Vector3 pos, Vector3 dir, int hitEffect)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.hitFx);
		gameObject.transform.position = pos;
		gameObject.transform.rotation = Quaternion.LookRotation(dir);
		gameObject.GetComponent<ParticleSystemRenderer>().material.mainTexture = this.materialText;
		if (hitEffect != 0)
		{
			HitParticles componentInChildren = gameObject.GetComponentInChildren<HitParticles>();
			if (componentInChildren != null)
			{
				componentInChildren.SetEffect((HitEffect)hitEffect);
			}
		}
	}

	public override void OnKill(Vector3 dir)
	{
		ResourceManager.Instance.RemoveItem(this.id);
	}

	private void OnEnable()
	{
		this.desiredScale = base.transform.localScale;
		this.currentScale = base.transform.localScale;
	}

	private new void Awake()
	{
		base.Awake();
	}

	public void SetDefaultScale(Vector3 scale)
	{
		this.defaultScale = scale;
		this.desiredScale = scale;
	}

	protected override void ExecuteHit()
	{
		MonoBehaviour.print("changing scale lol");
		this.currentScale = this.defaultScale * 0.7f;
	}

	public void PopIn()
	{
	}

	private void Update()
	{
		if (Mathf.Abs(base.transform.localScale.x - this.desiredScale.x) < 0.002f && Mathf.Abs(this.desiredScale.x - this.currentScale.x) < 0.002f)
		{
			return;
		}
		this.currentScale = Vector3.Lerp(this.currentScale, this.desiredScale, Time.deltaTime * 10f);
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.currentScale, Time.deltaTime * 15f);
	}

	public InventoryItem.ItemType compatibleItem;

	public int minTier;

	[Header("Loot")]
	public InventoryItem dropItem;

	public InventoryItem[] dropExtra;

	public float[] dropChance;

	public int amount;

	public bool dontScale;

	private Texture materialText;

	public int poolId;

	private Vector3 defaultScale;

	private float scaleMultiplier;

	private Vector3 desiredScale;

	private Vector3 currentScale;
}
