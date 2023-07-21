using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
	
	CharacterController controller;
	[SerializeField]
	[Tooltip("Player movement speed")]
	private float speedMovement;
	private Vector3 movement;
	[SerializeField]
	[Tooltip("Reference to walking sound audio source")]
	private AudioSource walking;

	void Start()
	{
		controller = gameObject.GetComponent<CharacterController>();
	}

	//Handles player movement based on W/A/S/D input
	void Update()
	{
		float moveX = Input.GetAxis("Horizontal");
		float moveZ = Input.GetAxis("Vertical");

		if (moveX == 0 && moveZ == 0)
		{
			walking.Pause();
		}
		else
		{
			if (!walking.isPlaying)
				walking.Play();
		}

		movement = transform.right * moveX * speedMovement + transform.forward * moveZ * speedMovement + transform.up * -9.81f;
		controller.Move(movement * Time.deltaTime);
	}

	void OnDisable()
	{
		if(walking != null)
			walking.Pause();
	}
}
