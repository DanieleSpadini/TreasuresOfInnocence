using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using S = System;

[DisallowMultipleComponent]
public class Minigame : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Reference to minigame camera")]
	private Camera minigameCamera;
	[SerializeField]
	[Tooltip("Reference to inspector menu in canvas")]
	private GameObject inspector;
	[SerializeField]
	[Tooltip("Reference to minigame HUD in canvas")]
	private GameObject minigameHud;

	public GameObject puzzle;
	public int puzzleIndex;
	public int puzzleMoves;

	/// <summary>
	/// Instantiate puzzle, switches cameras and changes OST and interface
	/// </summary>
	public void Restoration()
	{
		inspector.SetActive(false);
		minigameHud.SetActive(true);
		minigameCamera.GetComponent<MinigameCamera>().movesCounter = puzzleMoves;
		GameManager.instance.CameraChanger();
		Puzzle(puzzleIndex);
		minigameCamera.GetComponent<MinigameCamera>().index = puzzleIndex;
		Time.timeScale = 1;
		GameManager.instance.gameplayOST.Pause();
		GameManager.instance.puzzleOST.Play();
	}

	public void Puzzle(int index)
	{
		Instantiate(puzzle, minigameCamera.transform.position + minigameCamera.transform.forward * 5, Quaternion.Euler(0, 45, 0), minigameCamera.transform);
	}
}
