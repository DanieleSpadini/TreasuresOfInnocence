using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using S = System;


[DisallowMultipleComponent]
public class NextDay : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI blackScreenText;
	[SerializeField]
	[Tooltip("List of texts that appear between days, starts from day 2")]
	private List<string> daysPhrases;
	internal int mapIndex;
	[SerializeField]
	private GameObject blackScreen;

	//Edits black screend text and, if last day, button text too
	void OnEnable()
	{
		if (mapIndex == 4)
		{
			gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "end game";
			blackScreenText.text = "i hope dad comes home soon. \n\rthen we can play together!";
		}
		else
			blackScreenText.text = daysPhrases[mapIndex];

	}

	/// <summary>
	/// changes map or starts lastDayCoroutine if last day
	/// </summary>
	public void MapChanger()
	{
		if (mapIndex == 4)
		{
			StartCoroutine(LastDayCoroutine());
		}
		else
		{
			blackScreen.SetActive(true);
		}
	}

	//activates blackscreen and returns to main menu
	private IEnumerator LastDayCoroutine()
	{
		blackScreen.GetComponent<BlackScreen>().lastDay = true;
		blackScreen.SetActive(true);
		yield return new WaitForSecondsRealtime(5);
		SceneManager.LoadScene("MainMenu");
	}
}
