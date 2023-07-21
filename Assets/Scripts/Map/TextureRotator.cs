using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TextureRotator : MonoBehaviour
{
	void Start()
	{
		LookPlayer();
	}

	void Update()
	{
		LookPlayer();
	}

	/// <summary>
	/// Makes pin icon rotate towards player
	/// </summary>
	private void LookPlayer()
	{
		transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
	}
}
