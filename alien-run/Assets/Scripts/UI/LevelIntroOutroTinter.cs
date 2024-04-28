using UnityEngine;
using UnityEngine.UI;

public class LevelIntroOutroTinter : MonoBehaviour
{
	public bool HideTint = true; // Whether the script hides the tint or brings it back
	private Image m_imageComponent;

	public float TintSpeed = 6.0f;

	void Awake()
	{
		m_imageComponent = GetComponent<Image>();
		m_imageComponent.enabled = true;
	}

	public void HideLevelTint()
	{
		HideTint = true;
	}

	public void ShowLevelTint()
	{
		HideTint = false;
	}

	void Update()
	{
		float targetAlpha = 1.0f;
		float currentAlpha = m_imageComponent.color.a;
		if (HideTint)
		{
			targetAlpha = 0.0f;
		}

		float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * TintSpeed);
		Color newColor = m_imageComponent.color;
		newColor.a = newAlpha;
		m_imageComponent.color = newColor;	
	}
}
