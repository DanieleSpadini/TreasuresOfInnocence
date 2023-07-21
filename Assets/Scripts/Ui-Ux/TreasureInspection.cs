using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using S=System;


[DisallowMultipleComponent]
public class TreasureInspection : MonoBehaviour
{
	[SerializeField]
	[Tooltip("List of treasures prefabs")]
	private List<GameObject> treasureList = new List<GameObject>();
	[SerializeField]
	private PlayerCamera playerCamera;

	internal int index;
	//Instatiates treasure based on index in the right part of the screen
	void OnEnable()
	{
		Input.ResetInputAxes();
		Time.timeScale = 1;
		Instantiate(treasureList[index], playerCamera.transform.position + playerCamera.transform.forward * 1f + playerCamera.transform.right * 0.45f, Quaternion.identity, playerCamera.transform);
	}

	//destroys treasure instatiated
	void OnDisable()
	{
		Time.timeScale = 0;
		if(playerCamera != null)
			if(playerCamera.gameObject.transform.childCount > 1)
				Destroy(playerCamera.gameObject.transform.GetChild(2).gameObject);
	}
}
