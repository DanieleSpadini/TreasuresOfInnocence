using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using S=System;


[DisallowMultipleComponent]
public class SkyboxRotator : MonoBehaviour
{
	//Rotates skybox over time
	void Update()
	{
		RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.3f);
	}
}
