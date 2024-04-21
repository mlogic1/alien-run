using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inventory System
// A grid based item container

public class Inventory : MonoBehaviour
{
	private const int INVENTORY_SIZE_WIDTH = 6;
	private const int INVENTORY_SIZE_HEIGHT = 4;

	private List<InventoryItem> m_inventoryItems = new List<InventoryItem>();
	private Dictionary<InventoryItem, Vector2Int> m_inventoryItemPositions = new Dictionary<InventoryItem, Vector2Int>();

	// Adds item to inventory, returns true on success, returns false on fail (if inventory has no room)
	public bool AddItem(InventoryItem item)
	{
		// Find first available spot to place the item
		Vector2Int pos = new Vector2Int();
		bool foundPos = false;
		for (int i = 0; i < INVENTORY_SIZE_HEIGHT; ++i)
		{
			for (int j = 0; j < INVENTORY_SIZE_WIDTH; ++j)
			{
				pos.Set(i, j);
				if (CanItemBePlaced(item, pos))
				{
					foundPos = true;
					break;
				}
				++j;
			}
			++i;
		}

		if (!foundPos)  // no room for the item
		{
			return false;
		}

		Debug.LogError("player added item to inventory: " + item.ItemName);
		m_inventoryItems.Add(item);
		m_inventoryItemPositions.Add(item, pos);
		return true;
	}

	public bool CanItemBePlaced(InventoryItem item, Vector2Int position)
	{

		return true;
	}
}
