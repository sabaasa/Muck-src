using System;
using System.Collections.Generic;
using UnityEngine;

public class StructureSpawner : MonoBehaviour
{
	public float worldScale { get; set; } = 12f;

	public void CalculateWeight()
	{
		this.totalWeight = 0f;
		foreach (StructureSpawner.WeightedSpawn weightedSpawn in this.structurePrefabs)
		{
			this.totalWeight += weightedSpawn.weight;
		}
	}

	private void Start()
	{
		this.structures = new List<GameObject>();
		this.randomGen = new ConsistentRandom(GameManager.GetSeed() + ResourceManager.GetNextGenOffset());
		this.shrines = new Vector3[this.nShrines];
		this.mapChunkSize = MapGenerator.mapChunkSize;
		this.worldScale *= this.worldEdgeBuffer;
		this.CalculateWeight();
		int num = 0;
		for (int i = 0; i < this.nShrines; i++)
		{
			float x = (float)(this.randomGen.NextDouble() * 2.0 - 1.0) * (float)this.mapChunkSize / 2f;
			float z = (float)(this.randomGen.NextDouble() * 2.0 - 1.0) * (float)this.mapChunkSize / 2f;
			Vector3 vector = new Vector3(x, 0f, z) * this.worldScale;
			vector.y = 200f;
			Debug.DrawLine(vector, vector + Vector3.down * 500f, Color.cyan, 50f);
			RaycastHit hit;
			if (Physics.Raycast(vector, Vector3.down, out hit, 500f, this.whatIsTerrain) && WorldUtility.WorldHeightToBiome(hit.point.y) == TextureData.TerrainType.Grass)
			{
				this.shrines[i] = hit.point;
				num++;
				GameObject gameObject = this.FindObjectToSpawn(this.structurePrefabs, this.totalWeight, this.randomGen);
				GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, hit.point, gameObject.transform.rotation);
				if (!this.dontAddToResourceManager)
				{
					gameObject2.GetComponentInChildren<SharedObject>().SetId(ResourceManager.Instance.GetNextId());
				}
				this.structures.Add(gameObject2);
				this.Process(gameObject2, hit);
			}
		}
		if (!this.dontAddToResourceManager)
		{
			ResourceManager.Instance.AddResources(this.structures);
		}
		MonoBehaviour.print("spawned: " + this.structures.Count);
	}

	public virtual void Process(GameObject newStructure, RaycastHit hit)
	{
	}

	private void OnDrawGizmos()
	{
	}

	public float totalWeight { get; set; }

	public GameObject FindObjectToSpawn(StructureSpawner.WeightedSpawn[] structurePrefabs, float totalWeight, ConsistentRandom rand)
	{
		float num = (float)rand.NextDouble();
		float num2 = 0f;
		for (int i = 0; i < structurePrefabs.Length; i++)
		{
			num2 += structurePrefabs[i].weight;
			if (num < num2 / totalWeight)
			{
				return structurePrefabs[i].prefab;
			}
		}
		return structurePrefabs[0].prefab;
	}

	public StructureSpawner.WeightedSpawn[] structurePrefabs;

	private int mapChunkSize;

	private float worldEdgeBuffer = 0.6f;

	public int nShrines = 50;

	protected ConsistentRandom randomGen;

	public LayerMask whatIsTerrain;

	private List<GameObject> structures;

	public bool dontAddToResourceManager;

	private Vector3[] shrines;

	[Serializable]
	public class WeightedSpawn
	{
		public GameObject prefab;

		public float weight;
	}
}
