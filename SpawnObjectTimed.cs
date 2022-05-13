using System;
using UnityEngine;

public class SpawnObjectTimed : MonoBehaviour
{
	private void Awake()
	{
		base.Invoke("SpawnObject", this.time);
	}

	private void SpawnObject()
	{
		Object.Instantiate<GameObject>(this.objectToSpawn, base.transform.position, this.objectToSpawn.transform.rotation);
		Object.Destroy(this);
	}

	public float time;

	public GameObject objectToSpawn;
}
