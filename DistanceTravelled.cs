using System;
using UnityEngine;

public class DistanceTravelled : MonoBehaviour
{
	private void Start()
	{
		this.lastPos = this.rb.position;
		base.InvokeRepeating("SlowUpdate", this.interval, this.interval);
	}

	private void SlowUpdate()
	{
		int groundDist = (int)this.groundTravelled;
		int waterDist = (int)this.waterTravelled;
		if (AchievementManager.Instance)
		{
			AchievementManager.Instance.MoveDistance(groundDist, waterDist);
		}
		this.groundTravelled = 0f;
		this.waterTravelled = 0f;
	}

	private void FixedUpdate()
	{
		float num = Vector3.Distance(VectorExtensions.XZVector(this.rb.position), VectorExtensions.XZVector(this.lastPos));
		if (this.playerMovement.IsUnderWater())
		{
			this.waterTravelled += num;
		}
		else
		{
			this.groundTravelled += num;
		}
		this.lastPos = this.rb.transform.position;
	}

	public float groundTravelled;

	public float waterTravelled;

	public PlayerMovement playerMovement;

	public Rigidbody rb;

	public Vector3 lastPos;

	private float interval = 5f;
}
