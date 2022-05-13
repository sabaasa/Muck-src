﻿using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	private void Awake()
	{
		ProjectileController.Instance = this;
	}

	public void SpawnProjectileFromPlayer(Vector3 spawnPos, Vector3 direction, float force, int arrowId, int fromPlayer)
	{
		InventoryItem inventoryItem = ItemManager.Instance.allItems[arrowId];
		InventoryUI.Instance.arrows.UpdateCell();
		GameObject gameObject = Object.Instantiate<GameObject>(inventoryItem.prefab);
		gameObject.GetComponent<Renderer>().material = inventoryItem.material;
		gameObject.transform.position = spawnPos;
		gameObject.transform.rotation = Quaternion.LookRotation(direction);
		gameObject.GetComponent<Rigidbody>().AddForce(direction * force);
		Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GameManager.players[fromPlayer].GetCollider(), true);
		gameObject.GetComponent<Arrow>().otherPlayersArrow = true;
	}

	public void SpawnMobProjectile(Vector3 spawnPos, Vector3 direction, float force, int itemId, int mobObjectId)
	{
		InventoryItem inventoryItem = ItemManager.Instance.allItems[itemId];
		GameObject gameObject = Object.Instantiate<GameObject>(inventoryItem.prefab, spawnPos, Quaternion.LookRotation(direction));
		int attackDamage = inventoryItem.attackDamage;
		float projectileSpeed = inventoryItem.bowComponent.projectileSpeed;
		float colliderDisabledTime = inventoryItem.bowComponent.colliderDisabledTime;
		gameObject.transform.rotation = Quaternion.LookRotation(direction);
		Rigidbody component = gameObject.GetComponent<Rigidbody>();
		if (component)
		{
			component.AddForce(direction * force * projectileSpeed);
			component.angularVelocity = inventoryItem.rotationOffset;
		}
		MonoBehaviour.print(string.Concat(new object[]
		{
			"mob id: ",
			mobObjectId,
			", in mob manager: ",
			MobManager.Instance.mobs.ContainsKey(mobObjectId).ToString()
		}));
		if (MobManager.Instance.mobs.ContainsKey(mobObjectId))
		{
			Collider component2 = gameObject.GetComponent<Collider>();
			if (component2 != null)
			{
				Collider[] componentsInChildren = MobManager.Instance.mobs[mobObjectId].gameObject.transform.root.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					Physics.IgnoreCollision(componentsInChildren[i], component2, true);
				}
			}
		}
		float multiplier = MobManager.Instance.mobs[mobObjectId].multiplier;
		gameObject.GetComponent<EnemyProjectile>().DisableCollider(colliderDisabledTime);
		gameObject.GetComponent<EnemyProjectile>().damage = (int)((float)attackDamage * multiplier);
		MonoBehaviour.print("setting damage to: " + (float)attackDamage * multiplier);
	}

	public static ProjectileController Instance;
}
