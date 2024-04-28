using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MinigameController : MonoBehaviour, IInputReceiver
{
	public InputManager InputManager;
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

	private void StartMinigame()
	{
		string secret = "";
		m_gameSequence = new List<ArrowDirection>();
		m_currentIndex = 0;
		// generate 10 random sequences
		for (int i = 0; i < SequenceCount; i++)
		{
			int randIndex = Random.Range(0, 3);
			KeyValuePair<ArrowDirection, Sprite> randomArrow = m_arrowTextureDict.ElementAt(randIndex);
			m_gameSequence.Add(randomArrow.Key);

			secret += randomArrow.Key.ToString() + " ";
		}

		Debug.LogWarning("Secret: " + secret); // uncomment this to see the solution in the log
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

		StartMinigame();
	}

	public void ShowWindow()
	{
		gameObject.SetActive(true);
		InputManager.TakeInput(this);
	}

	public void HideWindow()
	{
		gameObject.SetActive(false);
		InputManager.ReleaseInput(this);
	}

	private void HideSequence()
	{
		foreach (Transform child in ArrowElementContainer)
		{
			child.GetComponent<Image>().sprite = TextureArrowUnknown;
		}
	}

	private void ProcessInput(ArrowDirection arrowDirection)
	{
		if (m_gameSequence[m_currentIndex] == arrowDirection)
		{
			Image element = ArrowElementContainer.GetChild(m_currentIndex).GetComponent<Image>();
			element.sprite = m_arrowTextureDict[arrowDirection];
			++m_currentIndex;
			if (m_currentIndex == SequenceCount)
			{
				Chest.OnPlayerWin();
				InputManager.ReleaseInput(this);
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

	}

	public void OnReceiveInputDirectional(DirectionalInput directionalInput)
	{
		switch (directionalInput)
		{
			case DirectionalInput.UP:
				ProcessInput(ArrowDirection.UP);
				break;
			case DirectionalInput.DOWN:
				ProcessInput(ArrowDirection.DOWN);
				break;
			case DirectionalInput.LEFT:
				ProcessInput(ArrowDirection.LEFT);
				break;
			case DirectionalInput.RIGHT:
				ProcessInput(ArrowDirection.RIGHT);
				break;
		}
	}

	public void OnReceiveInputAction()
	{
		
	}

	public void OnReceiveInputCancel()
	{
		HideWindow();
	}
}
