using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MinigameController : MonoBehaviour
{
	public Transform ArrowElementContainer;
	public MinigameChest Chest;

	public int SequenceCount = 10;
	public GameObject ArrowUIElement;
	public Sprite TextureArrowUp;
	public Sprite TextureArrowDown;
	public Sprite TextureArrowLeft;
	public Sprite TextureArrowRight;
	public Sprite TextureArrowUnknown;

	public enum ArrowDirection
	{
		UP,
		DOWN,
		LEFT,
		RIGHT
	}

	private Dictionary<ArrowDirection, Sprite> m_arrowTextureDict = new Dictionary<ArrowDirection, Sprite>();
	private List<ArrowDirection> m_gameSequence;
	private int m_currentIndex = 0;

	public void StartMinigame()
	{
		m_gameSequence = new List<ArrowDirection>();
		m_currentIndex = 0;
		// generate 10 random sequences
		for (int i = 0; i < SequenceCount; i++)
		{
			int randIndex = Random.Range(0, 3);
			KeyValuePair<ArrowDirection, Sprite> randomArrow = m_arrowTextureDict.ElementAt(randIndex);
			m_gameSequence.Add(randomArrow.Key);
		}	
	}

	private void Awake()
	{
		// Clear item container if there are any elements in it
		foreach (Transform child in ArrowElementContainer)
		{
			Destroy(child.gameObject);
		}

		for (int i = 0; i < SequenceCount; ++i)
		{
			Instantiate(ArrowUIElement, ArrowElementContainer);
		}

		// Setup texture references
		m_arrowTextureDict.Add(ArrowDirection.UP, TextureArrowUp);
		m_arrowTextureDict.Add(ArrowDirection.DOWN, TextureArrowDown);
		m_arrowTextureDict.Add(ArrowDirection.LEFT, TextureArrowLeft);
		m_arrowTextureDict.Add(ArrowDirection.RIGHT, TextureArrowRight);
	}

	void Start()
	{
		StartMinigame();
	}

	private void HideSequence()
	{
		foreach (Transform child in ArrowElementContainer)
		{
			child.GetComponent<Image>().sprite = TextureArrowUnknown;
		}
	}

	private void OnInputReceived(ArrowDirection arrowDirection)
	{
		if (m_gameSequence[m_currentIndex] == arrowDirection)
		{
			Image element = ArrowElementContainer.GetChild(m_currentIndex).GetComponent<Image>();
			element.sprite = m_arrowTextureDict[arrowDirection];
			++m_currentIndex;
			if (m_currentIndex == SequenceCount)
			{
				Chest.OnPlayerWin();
				Destroy(this.gameObject);
			}
		}
		else
		{
			HideSequence();
			m_currentIndex = 0;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			OnInputReceived(ArrowDirection.UP);
		}

		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			OnInputReceived(ArrowDirection.DOWN);
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			OnInputReceived(ArrowDirection.LEFT);
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			OnInputReceived(ArrowDirection.RIGHT);
		}
	}
}
