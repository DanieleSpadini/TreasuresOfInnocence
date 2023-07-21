using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzleSoundManager : MonoBehaviour
{
	[SerializeField]
	[Range(0f, 1f)]
	[Tooltip("Delay for the success/fail puzzle sound")]
	private float soundDelay;
	
	private bool puzzleCoroutineStarted = false;

	private void Update()
	{
		if (GameManager.instance.minigameCamera.GetComponent<MinigameCamera>().movesCounter == 0 && !puzzleCoroutineStarted)
			StartCoroutine(PuzzleSoundCoroutine());
	}

	//Plays win or lose sound depending on the number of remaining children objects
	private IEnumerator PuzzleSoundCoroutine()
	{
		puzzleCoroutineStarted = true;
		yield return new WaitForSeconds(soundDelay);
		if (transform.childCount == 0)
		{
			GameManager.instance.puzzleEndText.GetComponent<TextMeshProUGUI>().text = "treasure restored!";
			GameManager.instance.puzzleSuccess.Play();
		}
		else
		{
			GameManager.instance.puzzleEndText.GetComponent<TextMeshProUGUI>().text = "ran out of moves! try again";
			GameManager.instance.puzzleFail.Play();
		}
	}
}
