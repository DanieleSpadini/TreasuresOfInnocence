using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using S = System;


[DisallowMultipleComponent]
public class ObjectRotation : MonoBehaviour
{
	#region Private  Variables
	[SerializeField]
	[Tooltip("Object rotation sensitivity")]
	private float sensitivity;
	private float horizontal;
	private float vertical;
	internal bool puzzlecomplete;


	private enum RotatedObjectType : byte
	{
		treasure = 1,
		puzzle = 2
	}
	[SerializeField]
	private RotatedObjectType rotatedObjectType = RotatedObjectType.treasure;
	#endregion
	#region Lifecycle

	//Rotation mode based on object type, treasure rotates on X and Y axis, treasure on X axis
	void Update()
	{
		switch (rotatedObjectType)
		{
			case RotatedObjectType.treasure:
				horizontal = Input.GetAxis("Horizontal");
				vertical = Input.GetAxis("Vertical");
				Vector3 rotation = -horizontal * GameManager.instance.playerCamera.transform.parent.transform.up + vertical * GameManager.instance.playerCamera.transform.parent.transform.right;
				transform.Rotate(sensitivity * Time.deltaTime * rotation, Space.World);
				break;

			case RotatedObjectType.puzzle:
				if (transform.childCount == 0)
				{
					StartCoroutine(DestroyCoroutine());
				}
				horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * sensitivity;
				transform.Rotate(0, horizontal, 0, Space.World);
				break;
		}

	}

	//Destroys gameobject when called
	private IEnumerator DestroyCoroutine()
	{
		puzzlecomplete = true;
		yield return new WaitForSeconds(2);
		GameManager.instance.puzzleEndText.SetActive(false);
		Destroy(gameObject);
	}
	#endregion
}
