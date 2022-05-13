using System;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCamp : MonoBehaviour
{
	private void Awake()
	{
	}

	private void GenerateZone(ConsistentRandom rand)
	{
		this.zone = base.gameObject.AddComponent<MobZone>();
		this.zone.mobType = this.mobType;
		this.zone.respawnTime = -1f;
		this.zone.roamDistance = this.campRadius;
		this.zone.renderDistance = this.campRadius * 3f;
		this.zone.entityCap = rand.Next(this.min, this.max);
		this.zone.whatIsGround = this.whatIsGround;
		int nextId = MobZoneManager.Instance.GetNextId();
		this.zone.SetId(nextId);
		MobZoneManager.Instance.AddZone(this.zone, nextId);
	}

	public void MakeCamp(ConsistentRandom rand)
	{
		this.rand = rand;
		this.GenerateZone(rand);
		this.GenerateStructures(rand);
	}

	private void GenerateStructures(ConsistentRandom rand)
	{
		int num = 1;
		int num2 = rand.Next(2, 4);
		int amount = rand.Next(2, 3);
		int num3 = rand.Next(2, 4);
		int num4 = rand.Next(2, 7);
		int num5 = rand.Next(2, 8);
		int num6 = rand.Next(2, 5);
		int num7 = rand.Next(2, 5);
		List<GameObject> list = this.SpawnObjects(this.chiefChest, num, rand);
		foreach (GameObject gameObject in list)
		{
			gameObject.GetComponentInChildren<ChiefChestInteract>().mobZoneId = this.zone.id;
		}
		Debug.Log(string.Format("spawned {0} / {1}", list.Count, num));
		List<GameObject> list2 = this.SpawnObjects(this.hut, num2, rand);
		foreach (GameObject gameObject2 in list2)
		{
			gameObject2.GetComponent<SpawnChestsInLocations>().SetChests(rand);
		}
		Debug.Log(string.Format("spawned {0} / {1}", list2.Count, num2));
		Debug.Log(string.Format("spawned {0} / {1}", this.SpawnObjects(this.fireplace, num3, rand).Count, num3));
		Debug.Log(string.Format("spawned {0} / {1}", this.SpawnObjects(this.barrel, num4, rand).Count, num4));
		Debug.Log(string.Format("spawned {0} / {1}", this.SpawnObjects(this.log, num5, rand).Count, num5));
		Debug.Log(string.Format("spawned {0} / {1}", this.SpawnObjects(this.logPile, num6, rand).Count, num6));
		Debug.Log(string.Format("spawned {0} / {1}", this.SpawnObjects(this.rockPile, num7, rand).Count, num7));
		this.SpawnObjects(this.houseSpawner, amount, rand);
	}

	private List<GameObject> SpawnObjects(GameObject obj, int amount, ConsistentRandom rand)
	{
		if (obj == null)
		{
			return new List<GameObject>();
		}
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < amount; i++)
		{
			RaycastHit raycastHit = this.FindPos(rand);
			if (!(raycastHit.collider == null))
			{
				GameObject gameObject = Object.Instantiate<GameObject>(obj, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
				gameObject.transform.Rotate(gameObject.transform.right, 90f, Space.World);
				Hitable component = gameObject.GetComponent<Hitable>();
				if (component)
				{
					int nextId = ResourceManager.Instance.GetNextId();
					component.SetId(nextId);
					ResourceManager.Instance.AddObject(nextId, gameObject);
				}
				list.Add(gameObject);
			}
		}
		return list;
	}

	private List<GameObject> SpawnObjects(StructureSpawner houses, int amount, ConsistentRandom rand)
	{
		List<GameObject> list = new List<GameObject>();
		houses.CalculateWeight();
		for (int i = 0; i < amount; i++)
		{
			GameObject original = houses.FindObjectToSpawn(houses.structurePrefabs, houses.totalWeight, rand);
			RaycastHit raycastHit = this.FindPos(rand);
			if (!(raycastHit.collider == null))
			{
				GameObject gameObject = Object.Instantiate<GameObject>(original, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
				int nextId = ResourceManager.Instance.GetNextId();
				gameObject.GetComponent<Hitable>().SetId(nextId);
				ResourceManager.Instance.AddObject(nextId, gameObject);
				SpawnChestsInLocations componentInChildren = gameObject.GetComponentInChildren<SpawnChestsInLocations>();
				if (componentInChildren)
				{
					componentInChildren.SetChests(rand);
				}
				SpawnPowerupsInLocations componentInChildren2 = gameObject.GetComponentInChildren<SpawnPowerupsInLocations>();
				if (componentInChildren2)
				{
					componentInChildren2.SetChests(rand);
				}
				Hitable component = gameObject.GetComponent<Hitable>();
				if (component)
				{
					int nextId2 = ResourceManager.Instance.GetNextId();
					component.SetId(nextId2);
					ResourceManager.Instance.AddObject(nextId2, gameObject);
				}
				list.Add(gameObject);
			}
		}
		return list;
	}

	private RaycastHit FindPos(ConsistentRandom rand)
	{
		RaycastHit result = default(RaycastHit);
		Vector3 a = base.transform.position + Vector3.up * 200f;
		Vector3 b = this.RandomSpherePos(rand) * this.campRadius;
		if (Physics.SphereCast(a + b, 1f, Vector3.down, out result, 400f, this.whatIsGround))
		{
			if (result.collider.CompareTag("Camp"))
			{
				result = default(RaycastHit);
			}
			if (WorldUtility.WorldHeightToBiome(result.point.y) == TextureData.TerrainType.Water)
			{
				result = default(RaycastHit);
			}
		}
		return result;
	}

	private Vector3 RandomSpherePos(ConsistentRandom rand)
	{
		float x = (float)rand.NextDouble() * 2f - 1f;
		float y = (float)rand.NextDouble() * 2f - 1f;
		float z = (float)rand.NextDouble() * 2f - 1f;
		return new Vector3(x, y, z).normalized;
	}

	public void AssignRoles()
	{
		if (this.rolesAssigned)
		{
			return;
		}
		this.rolesAssigned = true;
		List<GameObject> entities = this.zone.entities;
		int num = this.rand.Next(1, this.min);
		int num2 = 0;
		foreach (GameObject gameObject in entities)
		{
			gameObject.GetComponent<WoodmanBehaviour>().AssignRole(this.rand);
			num2++;
			if (num2 >= num)
			{
				break;
			}
		}
		foreach (GameObject gameObject2 in entities)
		{
			gameObject2.GetComponent<hahahayes>().Randomize(this.rand);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, this.campRadius);
	}

	public GameObject zonePrefab;

	public MobType mobType;

	private MobZone zone;

	private float campRadius = 80f;

	private int min = 6;

	private int max = 10;

	private bool rolesAssigned;

	private ConsistentRandom rand;

	public LayerMask whatIsGround;

	public GameObject chiefChest;

	public GameObject hut;

	public GameObject barrel;

	public GameObject log;

	public GameObject logPile;

	public GameObject rockPile;

	public GameObject fireplace;

	public StructureSpawner houseSpawner;

	public bool testing;
}
