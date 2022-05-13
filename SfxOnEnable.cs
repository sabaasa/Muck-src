using System;
using UnityEngine;

public class SfxOnEnable : MonoBehaviour
{
	private void OnEnable()
	{
		this.sfx.Play();
	}

	public AudioSource sfx;
}
