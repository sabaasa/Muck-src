using System;
using UnityEngine;
using UnityEngine.AI;

public class JumpAttack : MonoBehaviour
{
	private void Update()
	{
		if (this.agent.enabled)
		{
			return;
		}
		this.currentTime += Time.deltaTime;
		float num = 10f;
		float num2 = this.currentTime;
		float y = Physics.gravity.y;
		float num3 = 2f;
		float d = (num * num2 + y * Mathf.Pow(num2, 2f)) * num3;
		base.transform.position = Vector3.Lerp(this.startJumpPos, this.desiredPos, this.currentTime / this.jumpTime);
		base.transform.position += Vector3.up * d;
	}

	private void Jump()
	{
		this.startJumpPos = base.transform.position;
		if (!this.mob.target)
		{
			this.desiredPos = base.transform.position;
		}
		else
		{
			Vector3 direction = this.mob.target.position - this.raycastPos.position;
			RaycastHit raycastHit;
			if (Physics.Raycast(this.raycastPos.position, direction, out raycastHit, 200f, this.whatIsHittable))
			{
				RaycastHit raycastHit2;
				if (Physics.Raycast(raycastHit.point, Vector3.down, out raycastHit2, 500f, this.whatIsGroundOnly))
				{
					this.desiredPos = raycastHit2.point - Vector3.up * this.agent.baseOffset;
				}
				else
				{
					this.desiredPos = base.transform.position;
				}
			}
			else
			{
				this.desiredPos = base.transform.position;
			}
		}
		this.agent.enabled = false;
		this.rangedRotation.enabled = false;
		this.currentTime = 0f;
		Object.Instantiate<GameObject>(this.warningPrefab, this.desiredPos, this.warningPrefab.transform.rotation).GetComponent<EnemyAttackIndicator>().SetWarning(1f, 13.5f);
		Object.Instantiate<GameObject>(this.jumpFx, base.transform.position, this.landingFx.transform.rotation);
	}

	private void Land()
	{
		this.agent.enabled = true;
		this.rangedRotation.enabled = true;
		Object.Instantiate<GameObject>(this.landingFx, base.transform.position, this.landingFx.transform.rotation);
	}

	public RotateWhenRangedAttack rangedRotation;

	public NavMeshAgent agent;

	public Mob mob;

	public GameObject warningPrefab;

	public GameObject jumpFx;

	public GameObject landingFx;

	public float jumpTime = 1f;

	private float currentTime;

	private Vector3 startJumpPos;

	private Vector3 desiredPos;

	public Transform raycastPos;

	public LayerMask whatIsHittable;

	public LayerMask whatIsGroundOnly;
}
