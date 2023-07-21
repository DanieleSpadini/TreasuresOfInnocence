using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using S = System;

[DisallowMultipleComponent]
public class DigOrFillHole : MonoBehaviour
{
	#region Private  Variables
	private RaycastHit hit;
	private RaycastHit lastHit;
	private Ray ray;
	[SerializeField]
	[Tooltip("From what distance player can click on blocks")]
	public float excavateRange;
	[SerializeField]
	[Tooltip("Reference to the dug tile prefab")]
	private GameObject holePrefab;
	[SerializeField]
	[Tooltip("Reference to the plain sand tile prefab")]
	private GameObject terrainPrefab;
	[SerializeField]
	[Tooltip("Reference to sand particle prefab")]
	private GameObject sandParticle;
	private Vector3 holePosition;
	private Transform parentTransform;
	[SerializeField]
	[Tooltip("Reference to dig sound audio source")]
	private AudioSource digSound;


	#endregion
	#region Lifecycle

	private void Start()
	{

	}

	void Update()
	{
		ray.origin = gameObject.transform.position;
		ray.direction = transform.forward;
		Debug.DrawRay(ray.origin, ray.direction * excavateRange, Color.red);
		bool hitAnything = Physics.Raycast(ray, out hit, excavateRange);

		if (!hitAnything || hit.collider != lastHit.collider)
		{
			if (lastHit.collider != null)
				if (lastHit.collider.GetComponent<IconActivator>() != null)
					lastHit.collider.GetComponent<IconActivator>().isInExcavationRange = false;
		}

		// If raycast hits something, the hit object has a valid tag and the player clicks on it, it recalls the ReplaceOnClick method
		if (hitAnything)
		{
			lastHit = hit;
			if (hit.collider.CompareTag("Excavable") && Input.GetKeyDown(KeyCode.Mouse0) && GameManager.instance.blackScreen.GetComponent<Image>().color.a <= 0.7f)
			{
				ReplaceOnClick(holePrefab);
				digSound.Play();
			}
			else if (hit.collider.CompareTag("Excavated") && Input.GetKeyDown(KeyCode.Mouse0) && GameManager.instance.blackScreen.GetComponent<Image>().color.a <= 0.7f)
			{
				ReplaceOnClick(terrainPrefab);
				digSound.Play();
			}

			if (hit.collider.TryGetComponent<IconActivator>(out IconActivator activator))
			{
				hit.collider.GetComponent<IconActivator>().isInExcavationRange = true;
			}
		}
	}
	#endregion
	#region Private Methods
	/// <summary>
	/// Method takes in a prefab, destroys the clicked gameobject and replaces it with the prefab
	/// </summary>
	/// <param name="prefab">
	/// Prefab will replace the gameObject the raycast hits
	/// </param>
	private void ReplaceOnClick(GameObject prefab)
	{
		parentTransform = hit.collider.gameObject.transform.parent.transform;
		holePosition = hit.collider.transform.localPosition;
		Destroy(hit.collider.gameObject);
		Instantiate(sandParticle, holePosition, sandParticle.transform.rotation);
		Instantiate(prefab, holePosition, Quaternion.identity, parentTransform);
	}
}
#endregion

