using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using S = System;


[DisallowMultipleComponent]
public class BlackScreen : MonoBehaviour
{
	[SerializeField]
	private MovePlayer movePlayer;
	[SerializeField]
	private GameObject nextDay;
	[SerializeField]
	private GameObject items;
	[SerializeField]
	private GameObject inspector;
	[SerializeField]
	[Tooltip("List of maps in scene")]
	private List<GameObject> maps;
	[SerializeField]
	private GameObject player;
	[SerializeField]
	private GameObject playerCamera;
	[SerializeField]
	private AudioSource nextDaySound;
	[SerializeField]
	[Tooltip("List of map images that appear in pause menu, starts from the second map")]
	private List<Sprite> mapImages;
	[SerializeField]
	private GameObject mapDrawing;
	[SerializeField]
	private TextMeshProUGUI nextDayText;
	[SerializeField]
	[Tooltip("Lower values for longer Fade time")]
	private float alfaPerFrame;
	[SerializeField]
	[Tooltip("Black screen duration")]
	private float duration;
	internal bool lastDay = false;
	private bool firstDay = true;
	private bool firstCoroutine = false;

	private Color blackScreenColor = new Color(0, 0, 0, 1);
	private Color nextDayTextColor = new Color(255, 255, 255, 1);
	private bool coroutineStarted = false;


	void OnEnable()
	{
		Time.timeScale = 0;
		if (firstDay)
		{
			StartCoroutine(FirstDayCoroutine());
		}
		else if (!lastDay)
		{
			StartCoroutine(BlackScreenCoroutine());
		}
		if(lastDay)
			Time.timeScale = 1;
	}
	void FixedUpdate()
	{
		if ((coroutineStarted || lastDay) && gameObject.GetComponent<Image>().color.a < 1)
		{
			Fade(1);
		}
		else if (firstCoroutine && firstDay)
		{
			Fade(-1);
			if (gameObject.GetComponent<Image>().color.a <= 0)
			{
				firstCoroutine = false;
				firstDay = false;
				gameObject.SetActive(false);
			}
		}
		else if (!coroutineStarted && !lastDay && !firstDay)
		{
			Fade(-1);
			if (gameObject.GetComponent<Image>().color.a <= 0)
			{
				gameObject.SetActive(false);
			}
		}


		if (gameObject.GetComponent<Image>().color.a == 1)
		{
			if (items.activeInHierarchy)
				items.SetActive(false);
			else
				inspector.SetActive(false);
		}
	}

	private IEnumerator BlackScreenCoroutine()
	{
		Time.timeScale = 1;
		coroutineStarted = true;
		nextDaySound.Play();
		player.GetComponent<CharacterController>().enabled = false;
		yield return new WaitForSecondsRealtime(duration);
		player.transform.rotation = GameManager.instance.playerStartRotation;
		player.transform.position = GameManager.instance.playerStartPosition;
		playerCamera.transform.localRotation = GameManager.instance.playerCameraRotation;
		playerCamera.GetComponent<PlayerCamera>().mouseX = 0;
		playerCamera.GetComponent<PlayerCamera>().mouseY = 0;
		playerCamera.GetComponent<PlayerCamera>().xRotation = 0;
		mapDrawing.GetComponent<Image>().sprite = mapImages[nextDay.GetComponent<NextDay>().mapIndex];
		maps[nextDay.GetComponent<NextDay>().mapIndex].SetActive(false);
		maps[nextDay.GetComponent<NextDay>().mapIndex + 1].SetActive(true);
		nextDay.SetActive(false);
		GameManager.instance.CloseInventory();
		player.GetComponent<CharacterController>().enabled = true;
		coroutineStarted = false;
	}

	private IEnumerator FirstDayCoroutine()
	{
		movePlayer.GetComponent<MovePlayer>().enabled = false;
		yield return new WaitForSecondsRealtime(duration / 2);
		movePlayer.GetComponent<MovePlayer>().enabled = true;
		firstCoroutine = true;
		Input.ResetInputAxes();
		Time.timeScale = 1;
	}

	private void Fade(float sign)
	{
		gameObject.GetComponent<Image>().color = blackScreenColor;
		nextDayText.color = nextDayTextColor;
		blackScreenColor.a += alfaPerFrame * sign;
		nextDayTextColor.a += alfaPerFrame * sign;
	}
}