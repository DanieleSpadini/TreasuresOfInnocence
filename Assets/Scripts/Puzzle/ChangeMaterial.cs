using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
	public Material weakMaterial;
	public GameObject block;
	[SerializeField]
	[Tooltip("Offset for collision detection")]
	private float offset;
	private bool isFalling = false;
	internal bool isCracked;
	private List<ChangeMaterial> changeMaterials = new List<ChangeMaterial>();
	private ChangeMaterial changeMaterial;
	[SerializeField]
	[Tooltip("Material index: 0 if starts whole, 1 if starts cracked")]
	private int materialCounter = 0;
	private Renderer prefabRenderer;
	private Color selected;
	private Vector3 startPosition;
	[SerializeField]
	[Tooltip("Reference to crack sound audio source")]
	private AudioSource crackSound;
	[SerializeField]
	[Tooltip("Reference to break sound audio source")]
	private AudioSource breakSound;

	void Start()
	{
		if (materialCounter == 1)
			isCracked = true;
		prefabRenderer = GetComponent<Renderer>();
		selected = prefabRenderer.material.color;
		startPosition = transform.localPosition;
		//Adds bodies connected with joints to changeMaterials list
		if (gameObject.TryGetComponent<FixedJoint>(out FixedJoint joint))
		{
			changeMaterials.Add(gameObject.GetComponent<FixedJoint>().connectedBody.GetComponent<ChangeMaterial>());
			gameObject.GetComponent<FixedJoint>().connectedBody.GetComponent<ChangeMaterial>().changeMaterials.Add(gameObject.GetComponent<ChangeMaterial>());
		}
	}

	void Update()
	{
		Vector3 vel = gameObject.GetComponent<Rigidbody>().velocity;
		if (vel.y > 0)
		{
			gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, Mathf.Min(vel.y, 0), 0);
		}
		transform.localPosition = new Vector3(startPosition.x, transform.localPosition.y, startPosition.z);
		if (gameObject.GetComponent<Rigidbody>().velocity.y < -0.3f)
		{
			isFalling = true;
		}
	}

	//On collision cracks game object and colliding blocks checking if conditions are met
	private void OnCollisionEnter(Collision collision)
	{
		changeMaterial = collision.gameObject.GetComponent<ChangeMaterial>();

		if (isFalling)
		{
			Vector3 localDistance = transform.InverseTransformPoint(collision.GetContact(collision.contactCount / 2).point);
			Vector3 localBoundsExtents = transform.InverseTransformDirection(gameObject.GetComponent<Collider>().bounds.extents);

			if (collision.transform.localPosition.y + Mathf.Abs(collision.transform.InverseTransformDirection(collision.gameObject.GetComponent<Collider>().bounds.extents).y) - offset <=
				transform.localPosition.y - Mathf.Abs(transform.InverseTransformDirection(gameObject.GetComponent<Collider>().bounds.extents).y) + offset)
			{
				if (!isCracked)
				{
					MaterialChanger();
				}

				if (changeMaterial != null)
					if (!changeMaterial.isCracked)
					{
						changeMaterial.MaterialChanger();
					}
			}
		}
	}

	//Adds colliding blocks to changeMaterials list
	void OnCollisionStay(Collision collision)
	{
		changeMaterial = collision.gameObject.GetComponent<ChangeMaterial>();
		if (changeMaterial != null)
			changeMaterials.Add(collision.gameObject.GetComponent<ChangeMaterial>());
	}

	//Removes blocks that exit collision from changeMaterials list
	private void OnCollisionExit(Collision collision)
	{
		changeMaterials.Remove(collision.gameObject.GetComponent<ChangeMaterial>());
	}

	//Destroys adiacent cracked blocks and checks for win condition
	private void OnDestroy()
	{
		foreach (ChangeMaterial change in changeMaterials)
		{
			if (change.isCracked)
				change.MaterialChanger();
			else
				GameManager.instance.minigameCamera.GetComponent<MinigameCamera>().puzzleWin = false;
		}
	}

	//Makes block material darker when mouse enters its bounds
	void OnMouseEnter()
	{
		prefabRenderer.material.color = selected * 0.6f;
	}

	//Reverts block material to original color when mouse exits its bounds
	void OnMouseExit()
	{
		prefabRenderer.material.color = selected;
	}

	void OnMouseDown()
	{
		if (GameManager.instance.minigameCamera.GetComponent<MinigameCamera>().movesCounter > 0 && !GameManager.instance.rulePage.activeSelf)
		{
			MaterialChanger();
			GameManager.instance.minigameCamera.GetComponent<MinigameCamera>().movesCounter--;
			GameManager.instance.minigameCamera.GetComponent<MinigameCamera>().MovesTextEditor();
		}
		if (changeMaterials.Count == GameManager.instance.minigameCamera.GetComponentInChildren<ObjectRotation>().transform.childCount)
		{
			GameManager.instance.minigameCamera.GetComponent<MinigameCamera>().puzzleWin = true;

		}
	}

	/// <summary>
	///
	/// </summary>
	// Method that when you click a block from the puzzle, changes the color (texture for later)
	// 1 time and if you click 2 time destroys the prefab
	private void MaterialChanger()
	{
		materialCounter++;
		if (materialCounter == 1)
		{
			isCracked = true;
			prefabRenderer.material = weakMaterial;
			selected = prefabRenderer.material.color;
			crackSound.Play();
		}

		if (materialCounter == 2)
		{
			if (breakSound.enabled)
				breakSound.Play();
			Destroy(block);

		}
	}
}
