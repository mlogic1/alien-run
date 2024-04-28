using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotItemViewSelectableArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	private InventorySlotItemView m_parentInventorySlotItemView;

	public void Initialize(InventorySlotItemView inventorySlotItemView)
	{
		m_parentInventorySlotItemView = inventorySlotItemView;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			m_parentInventorySlotItemView.OnSelectedAreaClicked();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		m_parentInventorySlotItemView.OnSelectableAreaEnter();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		m_parentInventorySlotItemView.OnSelectableAreaExit();
	}
}