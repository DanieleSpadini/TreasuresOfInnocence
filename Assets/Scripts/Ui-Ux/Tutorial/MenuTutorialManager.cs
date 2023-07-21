using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class MenuTutorialManager : MonoBehaviour
{
	[SerializeField]
	[Tooltip("List of tutorial texts that need to be disabled at the end of the first scene")]
	private List<TextMeshProUGUI> tutorialTexts;

	void OnDisable()
	{
		foreach (TextMeshProUGUI text in tutorialTexts)
			text.gameObject.SetActive(false);
	}
}