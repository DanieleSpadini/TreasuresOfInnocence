using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	public Transform playerBody;
	internal float xRotation = 0f;

	[SerializeField]
	[Tooltip("How fast camera rotates horizontally in proportion to mouse movement")]
	private float mouseSensitivityX;
	[SerializeField]
	[Tooltip("How fast camera rotates vertically in proportion to mouse movement")]
	private float mouseSensitivityY;
	[SerializeField]
	[Tooltip("Lower camera limit in degrees (negative)")]
	private float cameraMinAngle;
	[SerializeField]
	[Tooltip("Upper camera limit in degrees (positive)")]
	private float cameraMaxAngle;
	internal float mouseX;
	internal float mouseY;

	//On start resets input, locks cursor at the center of the screen and makes it invisible
	void OnEnable()
	{
		Input.ResetInputAxes();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	//Handles player visual applaying horizontal rotation to the player and vertical rotation to the camera
	void Update()
	{
		mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
		mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

		playerBody.Rotate(Vector3.up * mouseX);
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, cameraMinAngle, cameraMaxAngle);
		transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
	}

	//On disable makes cursor visible and movable again
	void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}
