using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using S = System;


[DisallowMultipleComponent]
public class DescriptionEditor : MonoBehaviour
{
	[SerializeField]
	private string descriptionText;
	[SerializeField]
	private string thoughtsText;
	[SerializeField]
	private string treasureName;


	[SerializeField]
	private GameObject description;
	[SerializeField]
	private GameObject thoughts;
	[SerializeField]
	private GameObject treasure;



	internal bool puzzleDone = false;

	/// <summary>
	/// If not already done starts minigame, else edits inspector menu desctiptions
	/// </summary>
	public void DescriptionEdit()
	{
		if (!puzzleDone)
		{
			gameObject.GetComponent<Minigame>().Restoration();
		}
		else
		{
			treasure.GetComponent<TextMeshProUGUI>().text = treasureName;
			description.GetComponent<TextMeshProUGUI>().text = descriptionText;
			thoughts.GetComponent<TextMeshProUGUI>().text = thoughtsText;
		}
	}
}
