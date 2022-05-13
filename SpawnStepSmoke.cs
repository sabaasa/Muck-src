using System;
using UnityEngine;

public class SpawnStepSmoke : MonoBehaviour
{
	public void LeftStep()
	{
		Object.Instantiate<GameObject>(this.stepFx, this.leftFoot.position, this.stepFx.transform.rotation);
	}

	public void RightStep()
	{
		Object.Instantiate<GameObject>(this.stepFx, this.rightFoot.position, this.stepFx.transform.rotation);
	}

	public Transform leftFoot;

	public Transform rightFoot;

	public GameObject stepFx;
}
