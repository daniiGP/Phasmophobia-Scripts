﻿using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class HighlightOwnedGameObj : MonoBehaviourPunCallbacks
{
	private void Update()
	{
		if (base.photonView.IsMine)
		{
			if (this.markerTransform == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PointerPrefab);
				gameObject.transform.parent = base.gameObject.transform;
				this.markerTransform = gameObject.transform;
			}
			Vector3 position = base.gameObject.transform.position;
			this.markerTransform.position = new Vector3(position.x, position.y + this.Offset, position.z);
			this.markerTransform.rotation = Quaternion.identity;
			return;
		}
		if (this.markerTransform != null)
		{
			UnityEngine.Object.Destroy(this.markerTransform.gameObject);
			this.markerTransform = null;
		}
	}

	public GameObject PointerPrefab;

	public float Offset = 0.5f;

	private Transform markerTransform;
}

