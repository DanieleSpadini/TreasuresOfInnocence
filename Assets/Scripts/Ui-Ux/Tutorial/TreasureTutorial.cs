using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TreasureTutorial : MonoBehaviour
{
	[SerializeField]
	private GameObject tutorial;
	[SerializeField]
	private GameObject TTT;

	void OnDestroy()
	{
		tutorial.GetComponent<Tutorial>().firstTreasureActive = true;
		TTT.SetActive(true);
	}
}
