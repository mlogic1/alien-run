using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameChest : MonoBehaviour
{
	public MinigameController MinigameController;
    public GameObject HintPopup;
	public GameObject HiddenItem;
	public Sprite ChestCompletedTexture;

	private bool m_playerCollidesWithChest = false;
	private bool m_chestCompleted = false;

	public void OnPlayerWin()
	{
		m_chestCompleted = true;
		HiddenItem.SetActive(true);
		HintPopup.SetActive(false);
		GetComponent<SpriteRenderer>().sprite = ChestCompletedTexture;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "PlayerFeet" && !m_chestCompleted)
		{
			HintPopup.SetActive(true);
			m_playerCollidesWithChest = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "PlayerFeet")
		{
			HintPopup.SetActive(false);
			m_playerCollidesWithChest = false;
		}
	}

	// This is a bit of a hack. The input system directly calls "PlayerActionActivated" of this specific minigame chest.
	// This is on purpose for simplicity, but in a complex game this would be handled through a controller which checks and maps which
	// interactable item in the world the player is standing at
	public void OnPlayerActionActivated(InputAction.CallbackContext context)
	{
		if (context.performed && m_playerCollidesWithChest)
		{
			MinigameController.ShowWindow();
		}
	}
}
