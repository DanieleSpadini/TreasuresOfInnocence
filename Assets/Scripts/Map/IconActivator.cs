using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class IconActivator : MonoBehaviour
{
	#region	Private variables
	[SerializeField]
	[Tooltip("Pin over tile")]
	private GameObject icon;
	/// <summary>
	/// When isInExcavationRange is true activates pin icon and highlights the tile
	/// </summary>
	internal bool isInExcavationRange
	{
		get => icon.activeSelf;
		set
		{
			if (value)
			{
				icon.SetActive(true);
				if (gameObject.GetComponent<Renderer>().material.color == selected)
					gameObject.GetComponent<Renderer>().material.color = selected * 1.2f;
			}
			else
			{
				icon.SetActive(false);
				if (gameObject.GetComponent<Renderer>().material.color != selected)
					gameObject.GetComponent<Renderer>().material.color = selected;
			}
		}
	}

	private Color selected;
	#endregion
	#region	Lifecycle

	void Start()
	{
		selected = gameObject.GetComponent<Renderer>().material.color;
	}

	//Deactivates pin when opening inventory
	void LateUpdate()
	{
		if(Input.GetKeyUp(KeyCode.E) && GameManager.instance.inventory.activeInHierarchy && icon.activeInHierarchy)
		{
			icon.SetActive(false);
			if (gameObject.GetComponent<Renderer>().material.color != selected)
				gameObject.GetComponent<Renderer>().material.color = selected;
		}

	}
	#endregion
}
