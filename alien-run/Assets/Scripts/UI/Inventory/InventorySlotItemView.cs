using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotItemView : MonoBehaviour
{
	public List<Image> HightlightImages;
    private IInventoryWindow m_inventoryWindow;
	private bool m_isSelected = false;

	private Color m_colorGreen = new Color(27, 255, 6, 11);
	private Color m_colorRed = new Color(255, 7, 7, 11);
	private Color m_colorNeutral = new Color(255, 255, 255, 11);
	private Color m_colorHidden = new Color(255, 255, 255, 0);
	

	/*public void OnPointerEnter(PointerEventData eventData)
	{
        Debug.Log("new new Pointer enter item: ");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("new new Pointer exit item");
	}*/

	public void Initialize(IInventoryWindow inventoryWindow)
    {
		m_inventoryWindow = inventoryWindow;
	}

	void Start()
    {
		InventorySlotItemViewSelectableArea[] selectableAreas = GetComponentsInChildren<InventorySlotItemViewSelectableArea>();
		foreach (InventorySlotItemViewSelectableArea selectableArea in selectableAreas)
		{
			selectableArea.Initialize(this);
		}
    }

	public void SetHighlightNeutral()
	{
		foreach (Image image in HightlightImages)
		{
			image.color = m_colorNeutral;
		}
	}

	public void SetHighlightHidden()
	{
		foreach (Image image in HightlightImages)
		{
			image.color = m_colorHidden;
		}
	}

	public void SetHighlightGreen()
	{
		foreach (Image image in HightlightImages)
		{
			image.color = m_colorGreen;
		}
	}

	public void SetHighlightRed()
	{
		foreach (Image image in HightlightImages)
		{
			image.color = m_colorRed;
		}
	}


	public void OnSelectableAreaEnter()
	{
		m_isSelected = true;
	}

	public void OnSelectableAreaExit()
	{
		m_isSelected = false;
	}

	public void OnSelectedAreaClicked()
	{
		m_inventoryWindow.OnItemViewClick(this);
	}
}
