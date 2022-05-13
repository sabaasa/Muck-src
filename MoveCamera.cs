using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
	public static MoveCamera Instance { get; private set; }

	private void Start()
	{
		MoveCamera.Instance = this;
		this.cam = base.transform.GetChild(0).GetComponent<Camera>();
		this.rb = PlayerMovement.Instance.GetRb();
		this.UpdateFov((float)CurrentSettings.Instance.fov);
	}

	private void LateUpdate()
	{
		switch (this.state)
		{
		case MoveCamera.CameraState.Player:
			this.PlayerCamera();
			return;
		case MoveCamera.CameraState.PlayerDeath:
			this.PlayerDeathCamera();
			return;
		case MoveCamera.CameraState.Spectate:
			this.SpectateCamera();
			return;
		case MoveCamera.CameraState.Freecam:
			this.FreeCam();
			return;
		default:
			return;
		}
	}

	public MoveCamera.CameraState state { get; set; }

	public void PlayerRespawn(Vector3 pos)
	{
		base.transform.position = pos;
		this.state = MoveCamera.CameraState.Player;
		base.transform.parent = null;
		base.CancelInvoke("SpectateCamera");
	}

	public void PlayerDied(Transform ragdoll)
	{
		this.target = ragdoll;
		this.state = MoveCamera.CameraState.PlayerDeath;
		this.desiredDeathPos = base.transform.position + Vector3.up * 3f;
		if (GameManager.state != GameManager.GameState.GameOver)
		{
			base.Invoke("StartSpectating", 4f);
		}
	}

	private void StartSpectating()
	{
		if (GameManager.state == GameManager.GameState.GameOver || !PlayerStatus.Instance.IsPlayerDead())
		{
			return;
		}
		this.target = null;
		this.state = MoveCamera.CameraState.Spectate;
		PPController.Instance.Reset();
	}

	private void SpectateCamera()
	{
		if (this.TryStartFreecam())
		{
			this.state = MoveCamera.CameraState.Freecam;
			this.desiredX = base.transform.rotation.x;
			this.yRotation = base.transform.rotation.y;
			this.target = null;
			return;
		}
		if (Input.GetKeyDown(InputManager.rightClick))
		{
			this.SpectateToggle(1);
		}
		else if (Input.GetKeyDown(InputManager.leftClick))
		{
			this.SpectateToggle(-1);
		}
		if (!this.target || !this.playerTarget)
		{
			foreach (PlayerManager playerManager in GameManager.players.Values)
			{
				if (!(playerManager == null) && !playerManager.dead)
				{
					this.target = new GameObject("cameraOrbit").transform;
					this.playerTarget = playerManager.transform;
					base.transform.parent = this.target;
					base.transform.localRotation = Quaternion.identity;
					base.transform.localPosition = new Vector3(0f, 0f, -10f);
					this.spectatingId = playerManager.id;
				}
			}
			if (!this.target)
			{
				return;
			}
		}
		Vector2 vector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		this.desiredSpectateRotation += new Vector3(-vector.y, vector.x, 0f) * 1.5f;
		this.target.position = this.playerTarget.position;
		this.target.rotation = Quaternion.Lerp(this.target.rotation, Quaternion.Euler(this.desiredSpectateRotation), Time.deltaTime * 10f);
		Vector3 direction = base.transform.position - this.target.position;
		RaycastHit raycastHit;
		float num;
		if (Physics.Raycast(this.target.position, direction, out raycastHit, 10f, this.whatIsGround))
		{
			Debug.DrawLine(this.target.position, raycastHit.point);
			num = 10f - raycastHit.distance + 0.8f;
			num = Mathf.Clamp(num, 0f, 10f);
		}
		else
		{
			num = 0f;
		}
		base.transform.localPosition = new Vector3(0f, 0f, -10f + num);
	}

	private void FreeCam()
	{
		if (this.TryStartLockedCam())
		{
			this.state = MoveCamera.CameraState.Spectate;
			return;
		}
		this.FreeCamRotation();
		bool key = Input.GetKey(InputManager.forward);
		bool key2 = Input.GetKey(InputManager.backwards);
		bool key3 = Input.GetKey(InputManager.right);
		bool key4 = Input.GetKey(InputManager.left);
		bool key5 = Input.GetKey(InputManager.sprint);
		bool key6 = Input.GetKey(InputManager.jump);
		float d = 0f;
		float d2 = 0f;
		float d3 = 1f;
		float d4 = 0f;
		if (key)
		{
			d = 1f;
		}
		else if (key2)
		{
			d = -1f;
		}
		if (key3)
		{
			d2 = 0.5f;
		}
		else if (key4)
		{
			d2 = -0.5f;
		}
		if (key5)
		{
			d3 = 4f;
		}
		if (key6)
		{
			d4 = 1f;
		}
		Vector3 a = (base.transform.forward * d + base.transform.right * d2) * d3 + Vector3.up * d4;
		float d5 = 15f;
		base.transform.position += a * Time.deltaTime * d5;
	}

	private void FreeCamRotation()
	{
		float num = this.playerInput.GetMouseX();
		float num2 = Input.GetAxis("Mouse Y") * this.playerInput.sensitivity * 0.02f * PlayerInput.sensMultiplier;
		if (CurrentSettings.invertedHor)
		{
			num = -num;
		}
		if (CurrentSettings.invertedVer)
		{
			num2 = -num2;
		}
		Debug.LogError(string.Concat(new object[]
		{
			"mouseX: ",
			num,
			", mouseY: ",
			num2
		}));
		this.desiredX += num;
		this.yRotation -= num2;
		this.yRotation = Mathf.Clamp(this.yRotation, -90f, 90f);
		this.cameraRot = new Vector3(this.yRotation, this.desiredX, 0f);
		base.transform.rotation = Quaternion.Euler(this.cameraRot);
	}

	private bool TryStartFreecam()
	{
		return Input.GetKey(InputManager.left) || Input.GetKey(InputManager.right) || Input.GetKey(InputManager.forward) || Input.GetKey(InputManager.backwards) || Input.GetKey(InputManager.jump);
	}

	private bool TryStartLockedCam()
	{
		return Input.GetKey(InputManager.rightClick) || Input.GetKey(InputManager.leftClick);
	}

	private void SpectateToggle(int dir)
	{
		int num = this.spectatingId;
		List<int> list = new List<int>();
		for (int i = 0; i < GameManager.players.Count; i++)
		{
			if (GameManager.players.ContainsKey(i) && !(GameManager.players[i] == null))
			{
				PlayerManager playerManager = GameManager.players[i];
				if (!(playerManager == null) && !playerManager.dead)
				{
					if (dir > 0 && playerManager.id > num)
					{
						list.Add(i);
					}
					if (dir < 0 && playerManager.id < num)
					{
						list.Add(i);
					}
				}
			}
		}
		if (list.Count < 1)
		{
			return;
		}
		list.Sort();
		PlayerManager playerManager2 = GameManager.players[list[0]];
		if (dir > 0)
		{
			playerManager2 = GameManager.players[list[0]];
		}
		if (dir < 0)
		{
			playerManager2 = GameManager.players[list[list.Count - 1]];
		}
		this.spectatingId = playerManager2.id;
		this.playerTarget = playerManager2.transform;
		Debug.LogError("nextId: " + this.spectatingId);
	}

	private void PlayerDeathCamera()
	{
		if (this.target == null)
		{
			return;
		}
		base.transform.position = Vector3.Lerp(base.transform.position, this.desiredDeathPos, Time.deltaTime * 1f);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.LookRotation(this.target.position - base.transform.position), Time.deltaTime);
	}

	private void PlayerCamera()
	{
		this.UpdateBob();
		this.MoveGun();
		base.transform.position = this.player.transform.position + this.bobOffset + this.desyncOffset + this.vaultOffset + this.offset;
		if (this.cinematic)
		{
			return;
		}
		Vector3 vector = this.playerInput.cameraRot;
		vector.x = Mathf.Clamp(vector.x, -90f, 90f);
		base.transform.rotation = Quaternion.Euler(vector);
		this.desyncOffset = Vector3.Lerp(this.desyncOffset, Vector3.zero, Time.deltaTime * 15f);
		this.vaultOffset = Vector3.Slerp(this.vaultOffset, Vector3.zero, Time.deltaTime * 7f);
		if (PlayerMovement.Instance.IsCrouching())
		{
			this.desiredTilt = 6f;
		}
		else
		{
			this.desiredTilt = 0f;
		}
		this.tilt = Mathf.Lerp(this.tilt, this.desiredTilt, Time.deltaTime * 8f);
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		eulerAngles.z = this.tilt;
		base.transform.rotation = Quaternion.Euler(eulerAngles);
	}

	private void MoveGun()
	{
		if (!this.rb)
		{
			return;
		}
		if (Mathf.Abs(this.rb.velocity.magnitude) >= 4f && PlayerMovement.Instance.grounded)
		{
			PlayerMovement.Instance.IsCrouching();
		}
	}

	public void UpdateFov(float f)
	{
		this.mainCam.fieldOfView = f;
		this.gunCamera.fieldOfView = f;
	}

	public void BobOnce(Vector3 bobDirection)
	{
		Vector3 a = this.ClampVector(bobDirection * 0.15f, -3f, 3f);
		this.desiredBob = a * this.bobMultiplier;
	}

	private void UpdateBob()
	{
		this.desiredBob = Vector3.Lerp(this.desiredBob, Vector3.zero, Time.deltaTime * this.bobSpeed * 0.5f);
		this.bobOffset = Vector3.Lerp(this.bobOffset, this.desiredBob, Time.deltaTime * this.bobSpeed);
	}

	private Vector3 ClampVector(Vector3 vec, float min, float max)
	{
		return new Vector3(Mathf.Clamp(vec.x, min, max), Mathf.Clamp(vec.y, min, max), Mathf.Clamp(vec.z, min, max));
	}

	public Transform player;

	public Vector3 offset;

	public Vector3 desyncOffset;

	public Vector3 vaultOffset;

	private Camera cam;

	private Rigidbody rb;

	public PlayerInput playerInput;

	public bool cinematic;

	private float desiredTilt;

	private float tilt;

	private Vector3 desiredDeathPos;

	private Transform target;

	private Vector3 desiredSpectateRotation;

	private Transform playerTarget;

	public LayerMask whatIsGround;

	private int spectatingId;

	private float yRotation;

	private float desiredX;

	public Vector3 cameraRot;

	private Vector3 desiredBob;

	private Vector3 bobOffset;

	private float bobSpeed = 15f;

	private float bobMultiplier = 1f;

	private readonly float bobConstant = 0.2f;

	public Camera mainCam;

	public Camera gunCamera;

	public enum CameraState
	{
		Player,
		PlayerDeath,
		Spectate,
		Freecam
	}
}
