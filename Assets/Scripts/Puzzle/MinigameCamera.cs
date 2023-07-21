using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using S=System;


[DisallowMultipleComponent]
public class MinigameCamera : MonoBehaviour
{
	[SerializeField]
	private GameManager gameManager;
	[SerializeField]
	private GameObject items;
	[SerializeField]
	private GameObject minigameHud;
	[SerializeField]
	private GameObject firstTreasure;
	[SerializeField]
	private GameObject secondTreasure;
	[SerializeField]
	private GameObject thirdTreasure;
	[SerializeField]
	private GameObject fourthTreasure;
	[SerializeField]
	private GameObject fifthTreasure;
	[SerializeField]
	[Tooltip("List of sprites for treasure buttons")]
	private List<Sprite> sprites;
	[SerializeField]
	private GameObject nextDay;
	[SerializeField]
	private GameObject puzzleRestartButton;
	[SerializeField]
	private GameObject puzzleBackButton;
	[SerializeField]
	private GameObject puzzleRulesButton;
	[SerializeField]
	private GameObject puzzleEndText;
	[SerializeField]
	private TextMeshProUGUI tutorialTreasureText;


	internal int index;
	internal int movesCounter;
	internal bool puzzleWin;
	internal bool coroutineStarted;

	void OnEnable()
	{
		GameManager.instance.inventory.SetActive(false);
		Input.ResetInputAxes();
		puzzleWin = false;
		MovesTextEditor();
		coroutineStarted = false;
	}

	//Second check for win and win or restart routine
	void Update()
	{
		if (transform.childCount == 0)
		{
			puzzleEndText.GetComponent<TextMeshProUGUI>().text = "";
			Time.timeScale = 0;
			GameManager.instance.inventory.SetActive(true);
			items.SetActive(true);
			gameManager.CameraChanger();
			puzzleRestartButton.SetActive(true);
			puzzleBackButton.SetActive(true);
			puzzleRulesButton.SetActive(true);
			nextDay.GetComponent<NextDay>().mapIndex = index;
			nextDay.SetActive(true);
			gameManager.puzzleOST.Stop();
			gameManager.gameplayOST.Play();

			switch (index)
			{
				case 0:
					firstTreasure.GetComponent<Button>().image.sprite = sprites[0];
					firstTreasure.GetComponent<DescriptionEditor>().puzzleDone = true;
					tutorialTreasureText.text = "select a restored treasure to view its information. use w/a/s/d to rotate it.";
					break;
				case 1:
					secondTreasure.GetComponent<Button>().image.sprite = sprites[1];
					secondTreasure.GetComponent<DescriptionEditor>().puzzleDone = true;
					break;
				case 2:
					thirdTreasure.GetComponent<Button>().image.sprite = sprites[2];
					thirdTreasure.GetComponent<DescriptionEditor>().puzzleDone = true;
					break;
				case 3:
					fourthTreasure.GetComponent<Button>().image.sprite = sprites[3];
					fourthTreasure.GetComponent<DescriptionEditor>().puzzleDone = true;
					break;
				case 4:
					fifthTreasure.GetComponent<Button>().image.sprite = sprites[4];
					fifthTreasure.GetComponent<DescriptionEditor>().puzzleDone = true;
					break;

			}
		}
		else if (movesCounter == 0 && !coroutineStarted && !puzzleWin)
		{
			puzzleRestartButton.SetActive(false);
			puzzleBackButton.SetActive(false);
			puzzleRulesButton.SetActive(false);
			StartCoroutine(WaitBeforeRestartCoroutine());
		}

		if (Input.GetKeyDown(KeyCode.R) && movesCounter > 0)
			Restart();
	}

	/// <summary>
	/// Destroys current puzzle, instantiate it again based on index and resets moves counter
	/// </summary>
	public void Restart()
	{
		puzzleEndText.GetComponent<TextMeshProUGUI>().text = "";
		puzzleEndText.SetActive(false);
		Destroy(transform.GetChild(0).gameObject);

		switch(index)
		{
			case 0:
				firstTreasure.GetComponent<Minigame>().Puzzle(index);
				movesCounter = firstTreasure.GetComponent<Minigame>().puzzleMoves;
				break;
			case 1:
				secondTreasure.GetComponent<Minigame>().Puzzle(index);
				movesCounter = secondTreasure.GetComponent<Minigame>().puzzleMoves;
				break;
			case 2:
				thirdTreasure.GetComponent<Minigame>().Puzzle(index);
				movesCounter = thirdTreasure.GetComponent<Minigame>().puzzleMoves;
				break;
			case 3:
				fourthTreasure.GetComponent<Minigame>().Puzzle(index);
				movesCounter = fourthTreasure.GetComponent<Minigame>().puzzleMoves;
				break;
			case 4:
				fifthTreasure.GetComponent<Minigame>().Puzzle(index);
				movesCounter = fifthTreasure.GetComponent<Minigame>().puzzleMoves;
				break;
		}
		MovesTextEditor();
		if(!puzzleRestartButton.activeSelf)
			puzzleRestartButton.SetActive(true);
		if(!puzzleBackButton.activeSelf)
			puzzleBackButton.SetActive(true);
		if(!puzzleRulesButton.activeSelf)
			puzzleRulesButton.SetActive(true);
		coroutineStarted = false;
	}

	//Deactivates minigame hud and destroys puzzle if not completed
	void OnDisable()
	{
		minigameHud.SetActive(false);
		if(gameObject.transform.childCount!=0)
		{
			Destroy(transform.GetChild(0).gameObject);
			Time.timeScale = 0;
			if(gameManager.puzzleOST != null)
				gameManager.puzzleOST.Stop();
			if (gameManager.gameplayOST != null)
				gameManager.gameplayOST.Play();
		}
	}

	//Waits 3 seconds before restart
	public IEnumerator WaitBeforeRestartCoroutine()
	{
		coroutineStarted = true;
		yield return new WaitForSeconds(3f);
		Restart();
	}

	/// <summary>
	/// Updates moves counter on screen
	/// </summary>
	public void MovesTextEditor()
	{
		if (movesCounter == 0)
		{
			puzzleEndText.SetActive(true);
		}
		minigameHud.GetComponentInChildren<TextMeshProUGUI>().text = $"{movesCounter}";
	}
}
