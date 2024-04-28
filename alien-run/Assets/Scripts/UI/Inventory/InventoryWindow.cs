using System;
using System.Collections.Generic;
using UnityEngine;


public interface IInventoryWindow
{
	public void OnItemViewClick(InventorySlotItemView inventorySlotItemView);
}

[Serializable]
public struct InventoryItemView
{
	public string ItemName;
	public GameObject Prefab;
}

public class InventoryWindow : MonoBehaviour, IInputReceiver, IInventoryWindow
{
	public List<InventoryItemView> InventoryItemViews;	// Map of item id's and its respective UI inventory slot prefabs
	public GameObject ContentContainer;

	private Inventory m_inventoryRef; // rename this..
	private InputManager m_inputManager;
	private int m_lastInventoryItemCount = 0;
	private Dictionary<InventorySlotItemView, InventoryItem> m_instantiatedViews = new Dictionary<InventorySlotItemView, InventoryItem>();

	private InventorySlotItemView m_currentlyDraggedObject = null;

	public void InitializeWindow(InputManager inputManager, Inventory inventory)
	{
		m_inputManager = inputManager;
		m_inventoryRef = inventory;
	}

	// Clears all instantiated sprites in inventory slots (weather they're leftover in the editor or previous time the inventory was open)
	void ClearContentContainer()
	{
		foreach (InventorySlotView slotView in ContentContainer.GetComponentsInChildren<InventorySlotView>())
		{
			foreach(Transform child in slotView.transform)
			{
				Destroy(child.gameObject);
			}
		}
	}

	public void Refresh()
	{
		m_instantiatedViews.Clear();    // remove all references to views
		ClearContentContainer();        // Clear all existing slots
		m_currentlyDraggedObject = null;

		foreach (KeyValuePair<InventoryItem, Vector2Int> itemInInventory in m_inventoryRef.GetInventoryItems())
		{
			var prefabtoSpawn = InventoryItemViews.Find(p =>
			{
				return p.ItemName == itemInInventory.Key.ItemName;
			}).Prefab;

			if (prefabtoSpawn != null)
			{                
				foreach (InventorySlotView slotView in ContentContainer.GetComponentsInChildren<InventorySlotView>())
				{
					if (slotView.SlotIndex == itemInInventory.Value)
					{
						GameObject itemView = Instantiate(prefabtoSpawn, slotView.gameObject.transform);
						
						InventorySlotItemView viewScript = itemView.GetComponent<InventorySlotItemView>();
						viewScript.Initialize(this);
						m_instantiatedViews.Add(viewScript, itemInInventory.Key);
					}
				}
			}
		}
	}

	// Start is called before the first frame update
	void Awake()
	{
		ClearContentContainer();
	}

	void Update()
	{
		int currentInventoryItemCount = m_inventoryRef.GetInventoryItemCount();
		if (currentInventoryItemCount != m_lastInventoryItemCount)	// automatically refresh inventory if an item was picked up
		{
			m_lastInventoryItemCount = currentInventoryItemCount;
			Refresh();
		}

		if (m_currentlyDraggedObject != null)
		{
			bool itemHoveringASlot = false;
			foreach (RectTransform slot in ContentContainer.transform)
			{
				Vector2 localMousePos = slot.InverseTransformPoint(Input.mousePosition);
				if (slot.rect.Contains(localMousePos))
				{
					Vector2Int slotIndex = slot.GetComponent<InventorySlotView>().SlotIndex;

					InventoryItem inventoryItem = m_instantiatedViews[m_currentlyDraggedObject];
					if (m_inventoryRef.CanItemBePlaced(inventoryItem, slotIndex))	// BUG: CanItemBePlaced does not ignore the item you're trying to move. In a scenario where you have
					{																// a large object (like the T shaped key) you're unable to place the key 1 and 2 slot to the right
						// item can be moved										// or 1 slot below. There should be another function called CanItemBeMoved, which should ignore the 'hovered' item
						if (Input.GetMouseButtonDown(0))							// so the item can properly be moved. This isn't implemented because of the time contstraint
						{
							if (m_inventoryRef.MoveItem(inventoryItem, slotIndex))
							{
								// Succesfully moved item to another slot in inventory
								Refresh();
								return;
							}
						}
					}

					itemHoveringASlot = true;
					m_currentlyDraggedObject.transform.position = slot.position;        // position the item in the slot place
					break;
				}
			}

			if (!itemHoveringASlot)
			{
				// item should just hover and follow the cursor
				m_currentlyDraggedObject.transform.position = Input.mousePosition;
			}
			// Check if hovering over a slot.
			// If yes, check if item can be placed there
			//		If yes, hover item over that slot
			//		if not, hover item over that slot but in red
			// if no, hover the item behind the mouse
		}
	}

	public void OnReceiveInputDirectional(DirectionalInput directionalInput)
	{
		// Not implemented: Inventory view does not have controller directional inputs supported. Only mouse.
	}

	public void OnReceiveInputAction()
	{
		// Not implemented: Inventory view does not have controller directional inputs supported. Only mouse.
	}

	public void OnReceiveInputCancel()
	{
		// Close the inventory window
		m_inputManager.ReleaseInput(this);
		Refresh();
		gameObject.SetActive(false);
	}

	public void OnItemViewClick(InventorySlotItemView inventorySlotItemView)
	{
		if (m_currentlyDraggedObject != null)
		{
			return; // Ignore clicks on items while drag-n-dropping an item
		}
		m_currentlyDraggedObject = inventorySlotItemView;
	}
}
