using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using S = System;


[DisallowMultipleComponent]
public class Treasure : MonoBehaviour
{
	#region Private Variables
	[SerializeField]
	[Tooltip("Reference to treasure button of the day")]
	private GameObject treasureButton;
	[SerializeField]
	[Tooltip("Reference to treasure particle game object")]
	private ParticleSystem treasureParticle;
	[SerializeField]
	[Tooltip("Reference to treasure sound audio source")]
	private AudioSource treasureSound;
	
	#endregion
	#region Lifecycle
	//On treasure tile destruction plays sound and particle and activates treasure found text
	void OnDestroy()
	{
		if(treasureSound != null)
			treasureSound.Play();
		if(treasureParticle != null)
			treasureParticle.Play();
		if(treasureButton != null)
			treasureButton.SetActive(true);
		if (GameManager.instance != null)
			GameManager.instance.treasureFoundText.GetComponent<TextMeshProUGUI>().gameObject.SetActive(true);

	}
	#endregion
}
