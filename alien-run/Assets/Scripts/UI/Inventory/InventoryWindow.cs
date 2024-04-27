using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct InventoryItemView
{
	public string ItemName;
	public GameObject Prefab;
}

public class InventoryWindow : MonoBehaviour, IInputReceiver
{
	public List<InventoryItemView> InventoryItemViews;
	public GameObject ContentContainer;

	private Inventory m_inventoryRef; // rename this..
	private InputManager m_inputManager;
	private int m_lastInventoryItemCount = 0;

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
		ClearContentContainer(); // Clear all existing slots
		foreach (Transform child in ContentContainer.transform)
		{
			
		}

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
						Debug.Log("ready to spawn prefab: " + prefabtoSpawn + "at: " + itemInInventory.Value.ToString());
						Instantiate(prefabtoSpawn, slotView.gameObject.transform);
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
		if (currentInventoryItemCount != m_lastInventoryItemCount)
		{
			m_lastInventoryItemCount = currentInventoryItemCount;
			Refresh();
		}
	}

	public void OnReceiveInputDirectional(DirectionalInput directionalInput)
	{
		Debug.Log("Important input in inventory window");
	}

	public void OnReceiveInputAction()
	{
		Debug.Log("TODO: Implement action");
	}

	public void OnReceiveInputCancel()
	{
		// Close the inventory window
		m_inputManager.ReleaseInput(this);
		gameObject.SetActive(false);
	}
}
