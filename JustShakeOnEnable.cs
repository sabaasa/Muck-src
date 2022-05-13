using System;
using MilkShake;
using UnityEngine;

public class JustShakeOnEnable : MonoBehaviour
{
	private void OnEnable()
	{
		if (this.customShake && !this.customAndDist)
		{
			CameraShaker.Instance.ShakeWithPreset(this.customShake);
			return;
		}
		float num = Vector3.Distance(base.transform.position, PlayerMovement.Instance.playerCam.position);
		if (num > this.maxDistance)
		{
			return;
		}
		float num2 = 1f - num / this.maxDistance;
		float shakeRatio = this.shakeM * num2;
		if (this.customAndDist)
		{
			CameraShaker.Instance.ShakeWithPresetAndRatio(this.customShake, shakeRatio);
		}
		CameraShaker.Instance.StepShake(shakeRatio);
	}

	public ShakePreset customShake;

	public float maxDistance = 50f;

	public float shakeM;

	public bool customAndDist;
}
