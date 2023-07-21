using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI tutorialText;
	[SerializeField]
	private GameObject pauseMenu;
	[SerializeField]
	internal GameObject hud;
	[SerializeField]
	internal GameObject treasureFoundText;
	[SerializeField]
	internal GameObject inventory;
	[SerializeField]
	internal GameObject items;
	[SerializeField]
	private GameObject inspector;
	[SerializeField]
	internal GameObject puzzleEndText;
	[SerializeField]
	private GameObject credits;

	public GameObject minigameCamera;
	public GameObject minigameHud;
	[SerializeField]
	private GameObject background;
	[SerializeField]
	private GameObject controls;
	[SerializeField]
	internal GameObject rulePage;
	[SerializeField]
	internal GameObject blackScreen;
	[SerializeField]
	private GameObject puzzleBack;
	[SerializeField]
	private GameObject nextDay;


	[SerializeField]
	private GameObject player;
	internal Vector3 playerStartPosition;
	internal Quaternion playerStartRotation;
	internal Quaternion playerCameraRotation;
	[SerializeField]
	internal PlayerCamera playerCamera;
	[SerializeField]
	private DigOrFillHole digOrFillHole;
	[SerializeField]
	private MovePlayer movePlayer;
	[SerializeField]
	private GameObject scoop;

	[SerializeField]
	private AudioSource paperSound;
	[SerializeField]
	internal AudioSource gameplayOST;
	[SerializeField]
	internal AudioSource puzzleOST;
	[SerializeField]
	internal AudioSource puzzleFail;
	[SerializeField]
	internal AudioSource puzzleSuccess;


	[SerializeField]
	private TreasureInspection treasureInspection;

	public static GameManager instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		if (SceneManager.GetActiveScene().name == "GameScene")
		{
			playerStartRotation = player.transform.rotation;
			playerStartPosition = player.transform.position;
			playerCameraRotation = playerCamera.gameObject.transform.rotation;
		}
	}
	void OnEnable()
	{
		if (Time.timeScale == 0)
			Time.timeScale = 1;
	}

	//Handles inputs to open and close menus
	void Update()
	{
		//Game scene inputs
		if (SceneManager.GetActiveScene().name == "GameScene")
		{

			//takes esc or tab input if not inventory,minigame hud or black screen are active
			if ((Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Tab)) && !inventory.activeInHierarchy && !minigameHud.activeInHierarchy && !blackScreen.activeInHierarchy)
			{
				if (!pauseMenu.activeInHierarchy)
				{
					if (controls.activeSelf)
					{
						controls.SetActive(false);
						background.SetActive(true);
					}
					PauseInventoryResume(pauseMenu, 0, true, false, false);
					scoop.SetActive(false);
				}
				else
				{
					PauseInventoryResume(pauseMenu, 1, false, true, true);
					scoop.SetActive(true);
				}
			}

			//opens or close inventory when not in pause or in minigame or while black screen
			if ((Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.I)) && !pauseMenu.activeInHierarchy && !minigameHud.activeInHierarchy && !blackScreen.activeInHierarchy)
			{
				if (!inventory.activeInHierarchy && !inspector.activeInHierarchy)
				{
					PauseInventoryResume(inventory, 0, true, false, false);
					scoop.SetActive(false);
				}
				else
				{
					PauseInventoryResume(inventory, 1, false, true, true);
					scoop.SetActive(true);
				}
				if (items.transform.GetChild(3).gameObject.activeSelf)
					tutorialText.gameObject.SetActive(false);
			}

			if (Input.GetKeyDown(KeyCode.Escape) && minigameHud.activeInHierarchy && puzzleBack.activeInHierarchy)
			{
				if (rulePage.activeInHierarchy)
					BackToPuzzle();
				else
					MinigameBack();
			}

			if (Input.GetKeyDown(KeyCode.Escape) && (inventory.activeInHierarchy || inspector.activeInHierarchy))
			{
				PauseInventoryResume(inventory, 1, false, true, true);
				scoop.SetActive(true);
			}
		}
		//Main menu inputs
		else
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (controls.activeInHierarchy)
					BackToPause();
				if (credits.activeInHierarchy)
					CloseCredits();
			}
		}
	}

	void OnDestroy()
	{
		instance = null;
	}

	/// <summary>
	/// Loads game scene
	/// </summary>
	public void NewGameButton()
	{
		SceneManager.LoadScene("GameScene");
	}

	/// <summary>
	/// Loads main menu
	/// </summary>
	public void MainMenuButton()
	{
		SceneManager.LoadScene("MainMenu");
	}

	/// <summary>
	/// Quits application
	/// </summary>
	public void ExitButton()
	{
		Application.Quit();
	}

	/// <summary>
	/// Opens inspector and sets index for treasure or puzzle instatiation
	/// </summary>
	/// <param name="index">
	/// Index of the treasure
	/// </param>
	public void OpenInspector(int index)
	{
		treasureInspection.index = index;
		items.SetActive(false);
		inspector.SetActive(true);
	}

	/// <summary>
	/// Back from inspector to menu
	/// </summary>
	public void Back()
	{
		inspector.SetActive(false);
		items.SetActive(true);
	}

	/// <summary>
	/// Back from minigame to items in inventory
	/// </summary>
	public void MinigameBack()
	{
		CameraChanger();
		inventory.SetActive(true);
		items.SetActive(true);
	}

	/// <summary>
	/// Closes inventory
	/// </summary>
	public void CloseInventory()
	{
		PauseInventoryResume(inventory, 1, false, true, true);
		scoop.SetActive(true);
	}

	/// <summary>
	/// Exits pause menu
	/// </summary>
	public void Resume()
	{
		PauseInventoryResume(pauseMenu, 1, false, true, true);
		scoop.SetActive(true);
	}

	/// <summary>
	/// Opens controls panel
	/// </summary>
	public void Controls()
	{
		background.SetActive(false);
		controls.SetActive(true);
	}

	/// <summary>
	/// Opens credits page
	/// </summary>
	public void Credits()
	{
		background.SetActive(false);
		credits.SetActive(true);
	}

	/// <summary>
	/// Close credits
	/// </summary>
	public void CloseCredits()
	{
		credits.SetActive(false);
		background.SetActive(true);
	}

	/// <summary>
	/// Back to pause menu
	/// </summary>
	public void BackToPause()
	{
		background.SetActive(true);
		controls.SetActive(false);
	}

	/// <summary>
	/// Opens rules page
	/// </summary>
	public void Rules()
	{
		Input.ResetInputAxes();
		rulePage.SetActive(true);
		Time.timeScale = 0;
	}

	/// <summary>
	/// Closes rules page
	/// </summary>
	public void BackToPuzzle()
	{
		rulePage.SetActive(false);
		Time.timeScale = 1;
	}

	/// <summary>
	/// Handles time and player refered scripts when opening or closing inventory
	/// </summary>
	/// <param name="menu">
	/// Gameobject reference to menu that needs to be opened or closed
	/// </param>
	/// <param name="timeScale">
	/// Sets timescale to the value, when opening menu should be 0 and when closing should be 1
	/// </param>
	/// <param name="menuState">
	/// When true activates the referenced menu, if false deactivates it
	/// </param>
	/// <param name="hudState">
	/// When true activates hud and tutorial text, if false deactivates them
	/// </param>
	/// <param name="scriptState">
	/// When true enables MovePlayer, PlayerCamera and DigOrFillHole scripts
	/// </param>
	public void PauseInventoryResume(GameObject menu, int timeScale, bool menuState, bool hudState, bool scriptState)
	{
		movePlayer.enabled = scriptState;
		playerCamera.enabled = scriptState;
		digOrFillHole.enabled = scriptState;
		menu.SetActive(menuState);
		items.SetActive(menuState);
		hud.SetActive(hudState);
		if (tutorialText != null)
			tutorialText.enabled = hudState;
		if (!blackScreen.activeInHierarchy)
			paperSound.Play();
		if (inspector.activeSelf)
			inspector.SetActive(false);
		if (treasureFoundText.activeSelf)
		{
			StopCoroutine(treasureFoundText.GetComponent<TreasureText>().TextDisableCoroutine());
			treasureFoundText.SetActive(false);
		}
		Time.timeScale = timeScale;
	}

	/// <summary>
	/// Switches between cameras
	/// </summary>
	public void CameraChanger()
	{
		if (playerCamera.gameObject.activeSelf)
		{
			playerCamera.gameObject.SetActive(false);
			minigameCamera.SetActive(true);
		}
		else
		{
			playerCamera.gameObject.SetActive(true);
			minigameCamera.SetActive(false);
		}
	}
}
