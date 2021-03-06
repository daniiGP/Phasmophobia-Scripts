using System;
using UnityEngine;
using UnityEngine.AI;

public class GhostEvent_1 : IState
{
	public GhostEvent_1(GhostAI ghostAI, GhostInteraction ghostInteraction)
	{
		this.ghostAI = ghostAI;
		this.ghostInteraction = ghostInteraction;
	}

	public void Enter()
	{
		this.player = GameController.instance.playersData[UnityEngine.Random.Range(0, GameController.instance.playersData.Count)].player;
		float num = Vector3.Distance(this.ghostAI.raycastPoint.transform.position, this.player.headObject.transform.position);
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (GameController.instance.playersData[i] != null && !GameController.instance.playersData[i].player.isDead && Vector3.Distance(this.ghostAI.raycastPoint.transform.position, GameController.instance.playersData[i].player.headObject.transform.position) < num)
			{
				num = Vector3.Distance(this.ghostAI.raycastPoint.transform.position, GameController.instance.playersData[i].player.headObject.transform.position);
				this.player = GameController.instance.playersData[i].player;
			}
		}
		if (this.player == null)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.player.isDead)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		this.room = this.player.currentRoom;
		Collider collider = this.room.colliders[UnityEngine.Random.Range(0, this.room.colliders.Count)];
		Vector3 vector = new Vector3(UnityEngine.Random.Range(collider.bounds.min.x, collider.bounds.max.x), collider.bounds.min.y, UnityEngine.Random.Range(collider.bounds.min.z, collider.bounds.max.z));
		this.ghostInteraction.CreateAppearedEMF(this.ghostAI.transform.position);
		if (this.player == null)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.room.doors.Length == 0 && !this.room.isBasementOrAttic)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.room.floorType != LevelController.instance.currentGhostRoom.floorType)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (Vector3.Distance(this.player.headObject.transform.position, this.ghostAI.raycastPoint.position) > 15f)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (Vector3.Distance(this.player.headObject.transform.position, vector) < 2f)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (MissionGhostEvent.instance != null && !MissionGhostEvent.instance.completed)
		{
			MissionGhostEvent.instance.CompleteMission();
		}
		for (int j = 0; j < this.room.doors.Length; j++)
		{
			this.room.doors[j].view.RequestOwnership();
		}
		for (int k = 0; k < this.room.doors.Length; k++)
		{
			if (!this.room.doors[k].photonInteract.isGrabbed)
			{
				Rigidbody component = this.room.doors[k].GetComponent<Rigidbody>();
				component.mass = 1f;
				component.isKinematic = false;
				component.AddTorque(new Vector3(0f, (component.GetComponent<HingeJoint>().limits.min == 0f) ? -6f : 6f, 0f), ForceMode.VelocityChange);
				this.ghostAI.StartCoroutine(this.ghostAI.ResetRigidbody(component, this.room.doors[k]));
				this.ghostInteraction.CreateDoorNoise(this.room.doors[k].transform.position);
			}
		}
		for (int l = 0; l < this.room.lightSwitches.Count; l++)
		{
			this.room.lightSwitches[l].TurnOff();
		}
		this.ghostAI.agent.Warp(this.GetPositionOnNavMesh(vector));
		this.ghostAI.Appear(true);
		this.ghostAI.ghostAudio.TurnOnOrOffAppearSource(true);
		this.ghostAI.ghostAudio.PlayOrStopAppearSource(true);
		for (int m = 0; m < this.room.doors.Length; m++)
		{
			if (!this.room.doors[m].photonInteract.isGrabbed)
			{
				Rigidbody component2 = this.room.doors[m].GetComponent<Rigidbody>();
				component2.isKinematic = false;
				component2.AddTorque(new Vector3(0f, -3f, 0f), ForceMode.Impulse);
				this.ghostAI.StartCoroutine(this.ghostAI.ResetRigidbody(component2, this.room.doors[m]));
				this.ghostInteraction.CreateDoorNoise(this.room.doors[m].transform.position);
			}
		}
		this.ghostAI.agent.speed = this.ghostAI.agent.speed / 2f;
	}

	public void Execute()
	{
		this.timer -= Time.deltaTime;
		this.ghostAI.agent.SetDestination(this.player.headObject.transform.position);
		RaycastHit raycastHit;
		if (Physics.Raycast(this.player.cam.transform.position, this.player.cam.transform.forward, out raycastHit, 2f, this.player.ghostRaycastMask) && raycastHit.collider.CompareTag("Ghost"))
		{
			this.ghostAI.ghostAudio.PlaySound(1, false, false);
			this.player.ChangeSanity(-20);
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (Vector3.Distance(this.player.headObject.transform.position, this.ghostAI.raycastPoint.position) < 1.5f)
		{
			this.player.ChangeSanity(-20);
			this.ghostAI.ghostAudio.PlaySound(1, false, false);
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.player.currentRoom != this.room)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
		if (this.timer < 0f)
		{
			this.ghostAI.ChangeState(GhostAI.States.idle, null, null);
			return;
		}
	}

	public void Exit()
	{
		this.ghostAI.ghostAudio.TurnOnOrOffAppearSource(false);
		this.ghostAI.ghostAudio.PlayOrStopAppearSource(false);
		this.ghostAI.UnAppear(false);
		this.ghostAI.agent.speed = this.ghostAI.defaultSpeed;
	}

	private Vector3 GetPositionOnNavMesh(Vector3 pos)
	{
		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(pos, out navMeshHit, 2f, 1);
		return navMeshHit.position;
	}

	private GhostAI ghostAI;

	private GhostInteraction ghostInteraction;

	private Player player;

	private LevelRoom room;

	private float timer = UnityEngine.Random.Range(5f, 15f);
}

