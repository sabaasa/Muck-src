﻿using System;
using UnityEngine;
using UnityEngine.AI;

public class DelayNavmesh : MonoBehaviour
{
	private void Awake()
	{
		base.Invoke("ResetObstacle", Random.Range(5f, 15f));
	}

	private void ResetObstacle()
	{
		NavMeshObstacle component = base.GetComponent<NavMeshObstacle>();
		component.enabled = false;
		component.enabled = true;
	}
}
