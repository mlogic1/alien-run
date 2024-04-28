using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WallWithItemRequirement : MonoBehaviour
{
	public string RequiredItemName;
	public GameObject HintPopup;
	public GameObject Wall;
	public TextMeshPro RequirementText;

	private bool m_completed = false;
	
	void Start()
	{
		if (RequiredItemName == string.Empty)
		{
			Debug.LogError("Wall item is not set up.");
		}

		RequirementText.text = "Requirement:\r\n" + RequiredItemName;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "PlayerFeet")
        {
			HintPopup.SetActive(true);

			Inventory playerInventory = collision.gameObject.GetComponentInParent<Player>().GetInventory();
            if (playerInventory.HasItem(RequiredItemName))
            {
				m_completed = true;
				HintPopup.SetActive(false);
				Wall.SetActive(false);
            }
        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!m_completed)
		{
			if (collision.gameObject.tag == "PlayerFeet")
			{
				HintPopup.SetActive(false);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
