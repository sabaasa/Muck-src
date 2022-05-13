using System;
using UnityEngine;

public class WoodmanBehaviour : MonoBehaviour
{
	private void Awake()
	{
		this.interactObject = base.GetComponent<hahahayes>().interact;
	}

	private void Start()
	{
		this.mob = base.GetComponent<Mob>();
		if (LocalClient.serverOwner)
		{
			Object.Destroy(base.gameObject.GetComponent<MobServer>());
			base.GetComponent<MobServer>().enabled = false;
			this.neutral = base.gameObject.AddComponent<MobServerNeutral>();
			this.neutral.mobZoneId = this.mobZoneId;
		}
		this.hitable = base.GetComponent<Hitable>();
		base.InvokeRepeating("SlowUpdate", 0.25f, 0.25f);
		this.AssignRole(new ConsistentRandom(GameManager.GetSeed() + this.hitable.GetId()));
		this.mob.agent.speed /= 2f;
	}

	private void AssignRoles()
	{
		MobZoneManager.Instance.zones[this.mobZoneId].GetComponent<GenerateCamp>().AssignRoles();
	}

	public void AssignRole(ConsistentRandom rand)
	{
		hahahayes component = base.transform.root.GetComponent<hahahayes>();
		component.SkinColor(rand);
		if (rand.NextDouble() < 0.4)
		{
			return;
		}
		WoodmanBehaviour.WoodmanType type = (WoodmanBehaviour.WoodmanType)rand.Next(1, Enum.GetValues(typeof(WoodmanBehaviour.WoodmanType)).Length);
		TraderInteract traderInteract = this.interactObject.AddComponent<TraderInteract>();
		int nextId = ResourceManager.Instance.GetNextId();
		traderInteract.SetId(nextId);
		ResourceManager.Instance.AddObject(nextId, traderInteract.gameObject);
		traderInteract.SetType(type, rand);
		component.SetType(type);
		component.Randomize(rand);
		Object.Destroy(this.neutral);
	}

	private void SlowUpdate()
	{
		if (this.hitable.hp < this.hitable.maxHp)
		{
			this.MakeAggressive(true);
		}
	}

	public void MakeAggressive(bool first)
	{
		if (this.aggressive)
		{
			return;
		}
		this.aggressive = true;
		this.mob.ready = true;
		try
		{
			Object.Destroy(this.neutral);
		}
		catch (Exception)
		{
		}
		if (this.mob.mobType.behaviour == MobType.MobBehaviour.Enemy)
		{
			base.gameObject.AddComponent<MobServerEnemy>();
		}
		else
		{
			base.gameObject.AddComponent<MobServerEnemyMeleeAndRanged>();
		}
		if (first)
		{
			foreach (GameObject gameObject in MobZoneManager.Instance.zones[this.mobZoneId].entities)
			{
				gameObject.GetComponent<WoodmanBehaviour>().MakeAggressive(false);
			}
		}
		this.mob.agent.speed = this.mob.mobType.speed;
		Object.Destroy(this);
		Object.Destroy(this.interactObject);
	}

	private Mob mob;

	private Vector3 headOffset;

	public int mobZoneId;

	private MobServerNeutral neutral;

	private Hitable hitable;

	public GameObject interactObject;

	private bool aggressive;

	public enum WoodmanType
	{
		None,
		Archer,
		Smith,
		Woodcutter,
		Chef,
		Wildcard
	}
}
