using System.Collections;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class Tutorial : MonoBehaviour
{
	private string introduction = "movement: w/a/s/d \n\rlook around: move the mouse";
	private string afterIntroduction = "now check the treasure map \n\rin the pause menu \n\rmap and pause: esc/tab \n\rdig: left click";
	private string afterFirstTreasure = "open the inventory \n\rto inspect the new treasure \n\rinventory: e/i";
	internal bool firstTreasureActive;
	[SerializeField]
	[Tooltip("higher value for faster fade effect")]
	private float alfaPerFrame;
	private bool fadein;
	private bool treasureCoroutineStart;
	private bool treasureCoroutineEnd;
	private bool firstMessageStop;
	private Coroutine firstMessage;

	internal Color tutorialTextColor = new Color(255, 255, 255, 0);

	void Awake()
	{
		firstMessage = StartCoroutine(FirstMessageCoroutine());
	}

	//Handles tutorial texts fade based on instructions recived from coroutines or treasureTutorial script
	void Update()
	{
		if (firstTreasureActive && !firstMessageStop)
		{
			StopCoroutine(firstMessage);
			firstMessageStop = true;
		}

		if (firstTreasureActive && !treasureCoroutineStart)
		{
			gameObject.GetComponent<TextMeshProUGUI>().color = tutorialTextColor;
			StartCoroutine(FirstTreasureFoundCoroutine());
		}

		if (fadein && gameObject.GetComponent<TextMeshProUGUI>().color.a <=1)
			TextFade(1);
		else if(!fadein && gameObject.GetComponent<TextMeshProUGUI>().color.a >= 0)
			TextFade(-1);

		if (treasureCoroutineEnd && gameObject.GetComponent<TextMeshProUGUI>().color.a <= 0)
			gameObject.SetActive(false);
	}

	//Handles the tutorial text before finding treasure
	public IEnumerator FirstMessageCoroutine()
	{
		gameObject.GetComponent<TextMeshProUGUI>().text = introduction;
		fadein = true;
		yield return new WaitForSeconds(7);
		fadein = false;
		yield return new WaitForSeconds(3);
		gameObject.GetComponent<TextMeshProUGUI>().text = afterIntroduction;
		fadein=true;
		yield return new WaitForSeconds(10);
		fadein = false;
	}

	//Handles tutorial text after finding treasure
	public IEnumerator FirstTreasureFoundCoroutine()
	{
		fadein = true;
		treasureCoroutineStart = true;
		gameObject.GetComponent<TextMeshProUGUI>().text = afterFirstTreasure;
		yield return new WaitForSeconds(10);
		fadein=false;
		treasureCoroutineEnd = true;
	}

	/// <summary>
	/// handles fade in and fade out effects using alfaPerFrame for fade speed and sign for fade type
	/// </summary>
	/// <param name="sign">
	/// +1 for fade in, -1 for fade out of black screen
	/// </param>
	private void TextFade(float sign)
	{
		gameObject.GetComponent<TextMeshProUGUI>().alpha += alfaPerFrame * sign;
	}


}
