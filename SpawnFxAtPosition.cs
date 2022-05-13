using System;
using UnityEngine;

public class SpawnFxAtPosition : MonoBehaviour
{
	public void SpawnFx(int n)
	{
		Object.Instantiate<GameObject>(this.fx[n], this.positions[n].position, this.fx[n].transform.rotation);
	}

	public GameObject[] fx;

	public Transform[] positions;
}
