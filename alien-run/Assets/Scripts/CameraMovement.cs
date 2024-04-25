using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CameraMovement is a script which moves (lerps) the camera after the TrackedObject
public class CameraMovement : MonoBehaviour
{
	public Transform TrackedGameObject;
	public float CameraOffsetX = 0.0f;
	public float CameraOffsetY = 0.0f;
	public float CameraFollowSpeed = 2.50f;

	void Update()
	{
		if (TrackedGameObject == null)
		{
			return;
		}
		Vector3 newPos = new Vector3(TrackedGameObject.position.x + CameraOffsetX, TrackedGameObject.position.y + CameraOffsetY, transform.position.z);
		Vector3 newPosLRP = Vector3.Lerp(transform.position, newPos, Time.deltaTime * CameraFollowSpeed);

		transform.SetPositionAndRotation(newPosLRP, transform.rotation);
	}
}
