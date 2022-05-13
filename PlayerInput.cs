﻿using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public float sensitivity { get; set; } = 50f;

	public float x { get; private set; }

	public float y { get; private set; }

	public bool jumping { get; private set; }

	public bool crouching { get; private set; }

	public bool sprinting { get; private set; }

	public static PlayerInput Instance { get; set; }

	private void Awake()
	{
		PlayerInput.Instance = this;
		this.playerMovement = (PlayerMovement)base.GetComponent("PlayerMovement");
		this.playerCam = this.playerMovement.playerCam;
		this.orientation = this.playerMovement.orientation;
	}

	private void Update()
	{
		if (!this.active)
		{
			return;
		}
		if (GameManager.state == GameManager.GameState.GameOver)
		{
			this.StopInput();
			return;
		}
		this.MyInput();
		this.Look();
	}

	private void FixedUpdate()
	{
		if (!this.active)
		{
			return;
		}
		this.playerMovement.Movement(this.x, this.y);
	}

	private void StopInput()
	{
		this.x = 0f;
		this.y = 0f;
		this.jumping = false;
		this.sprinting = false;
		this.mouseScroll = 0f;
		this.playerMovement.SetInput(new Vector2(this.x, this.y), this.crouching, this.jumping, this.sprinting);
	}

	private void MyInput()
	{
		if (OtherInput.Instance.OtherUiActive() && !Map.Instance.active)
		{
			this.StopInput();
			return;
		}
		if (!this.playerMovement)
		{
			return;
		}
		this.x = 0f;
		this.y = 0f;
		if (Input.GetKey(InputManager.forward))
		{
			float num = this.y;
			this.y = num + 1f;
		}
		else if (Input.GetKey(InputManager.backwards))
		{
			float num = this.y;
			this.y = num - 1f;
		}
		if (Input.GetKey(InputManager.left))
		{
			float num = this.x;
			this.x = num - 1f;
		}
		if (Input.GetKey(InputManager.right))
		{
			float num = this.x;
			this.x = num + 1f;
		}
		this.jumping = Input.GetKey(InputManager.jump);
		this.sprinting = Input.GetKey(InputManager.sprint);
		this.mouseScroll = Input.mouseScrollDelta.y;
		if (Input.GetKeyDown(InputManager.jump))
		{
			this.playerMovement.Jump();
		}
		if (Input.GetKey(InputManager.leftClick))
		{
			UseInventory.Instance.Use();
		}
		if (Input.GetKeyUp(InputManager.leftClick))
		{
			UseInventory.Instance.UseButtonUp();
		}
		if (Input.GetKeyDown(InputManager.rightClick))
		{
			BuildManager.Instance.RequestBuildItem();
		}
		if (this.mouseScroll != 0f)
		{
			BuildManager.Instance.RotateBuild((int)Mathf.Sign(this.mouseScroll));
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			BuildManager.Instance.RotateBuild(1);
		}
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.U) && Input.GetKeyDown(KeyCode.I))
		{
			UiController.Instance.ToggleHud();
		}
		this.playerMovement.SetInput(new Vector2(this.x, this.y), this.crouching, this.jumping, this.sprinting);
	}

	private void Look()
	{
		if (Cursor.lockState == CursorLockMode.None)
		{
			return;
		}
		if (OtherInput.lockCamera)
		{
			return;
		}
		float num = this.GetMouseX();
		float num2 = Input.GetAxis("Mouse Y") * this.sensitivity * 0.02f * PlayerInput.sensMultiplier;
		if (CurrentSettings.invertedHor)
		{
			num = -num;
		}
		if (CurrentSettings.invertedVer)
		{
			num2 = -num2;
		}
		Vector3 eulerAngles = this.playerCam.transform.localRotation.eulerAngles;
		this.desiredX = eulerAngles.y + num;
		this.xRotation -= num2;
		this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);
		this.actualWallRotation = Mathf.SmoothDamp(this.actualWallRotation, this.wallRunRotation, ref this.wallRotationVel, 0.2f);
		this.cameraRot = new Vector3(this.xRotation, this.desiredX, this.actualWallRotation);
		this.orientation.transform.localRotation = Quaternion.Euler(0f, this.desiredX, 0f);
	}

	public Vector2 GetAxisInput()
	{
		return new Vector2(this.x, this.y);
	}

	public float GetMouseX()
	{
		return Input.GetAxis("Mouse X") * this.sensitivity * 0.02f * PlayerInput.sensMultiplier;
	}

	public void SetMouseOffset(float o)
	{
		this.xRotation = o;
	}

	public float GetMouseOffset()
	{
		return this.xRotation;
	}

	private float xRotation;

	public static float sensMultiplier = 1f;

	private float desiredX;

	private float mouseScroll;

	private Transform playerCam;

	private Transform orientation;

	private PlayerMovement playerMovement;

	public bool active = true;

	private float actualWallRotation;

	private float wallRotationVel;

	public Vector3 cameraRot;

	private float wallRunRotation;

	public float mouseOffsetY;
}
