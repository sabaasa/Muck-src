﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildDestruction : MonoBehaviour
{
	private void Awake()
	{
		base.Invoke("CheckDirectlyGrounded", 2f);
	}

	private void Start()
	{
		foreach (BoxCollider boxCollider in base.GetComponents<BoxCollider>())
		{
			if (boxCollider.isTrigger)
			{
				this.trigger = boxCollider;
				break;
			}
		}
		this.trigger.size *= 1.1f;
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
		this.destroyed = true;
		List<BuildDestruction> list = new List<BuildDestruction>();
		list.Add(this);
		for (int i = this.otherBuilds.Count - 1; i >= 0; i--)
		{
			if (!(this.otherBuilds[i] == null) && !this.otherBuilds[i].IsDirectlyGrounded(list))
			{
				this.otherBuilds[i].DestroyBuild();
			}
		}
	}

	private void DestroyBuild()
	{
		Hitable component = base.GetComponent<Hitable>();
		component.Hit(component.hp, 1f, 1, base.transform.position, -1);
	}

	public bool IsDirectlyGrounded(List<BuildDestruction> alreadyChecked)
	{
		if (this.directlyGrounded)
		{
			return true;
		}
		foreach (BuildDestruction buildDestruction in this.otherBuilds)
		{
			if (!(buildDestruction == null) && !alreadyChecked.Contains(buildDestruction))
			{
				alreadyChecked.Add(buildDestruction);
				if (buildDestruction.IsDirectlyGrounded(alreadyChecked))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void CheckDirectlyGrounded()
	{
		Object component = base.GetComponent<Rigidbody>();
		Object.Destroy(this.trigger);
		Object.Destroy(component);
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			this.directlyGrounded = true;
			this.connectedToGround = true;
		}
		if (collision.CompareTag("Build"))
		{
			BuildDestruction component = collision.GetComponent<BuildDestruction>();
			if (!this.otherBuilds.Contains(component))
			{
				MonoBehaviour.print("added a build: " + collision.gameObject.name);
				this.otherBuilds.Add(component);
			}
		}
	}

	private void OnDrawGizmos()
	{
	}

	public bool connectedToGround;

	public bool directlyGrounded;

	public bool started;

	public bool destroyed;

	private List<BuildDestruction> otherBuilds = new List<BuildDestruction>();

	private BoxCollider trigger;
}
