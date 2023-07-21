using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[DisallowMultipleComponent]
public class TreasureText : MonoBehaviour
{
	void OnEnable()
	{
		StartCoroutine(TextDisableCoroutine());
	}

	//Automatic disable for treasure found text
	public IEnumerator TextDisableCoroutine()
	{
		yield return new WaitForSecondsRealtime(2);
		gameObject.SetActive(false);
	}

}
