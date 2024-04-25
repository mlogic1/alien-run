using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameChest : MonoBehaviour
{
	public MinigameController MinigameController;
    public GameObject HintPopup;
	public GameObject HiddenItem;

	public void OnPlayerWin()
	{
		HiddenItem.SetActive(true);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "PlayerFeet")
		{
			HintPopup.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "PlayerFeet")
		{
			HintPopup.SetActive(false);
		}
	}
}
