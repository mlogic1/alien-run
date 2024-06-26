using System.Collections.Generic;
using UnityEngine;

// Inventory System
// A grid based item container
// Items are defined by a unique Name and a shape.

// Shape defines how many and which slots an item takes up. Shape is an array of { X, Y } indicies,
// and of course, every item needs to have at least the 1 shape point {0, 0} defined.
// for instance, a T shaped item which takes 4 slots would be defined as:
// Vector2[] =	{
//					{ 0,0 } ,	{0, 1},		{ 0,2 },
//								{1 ,1},
//				}

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
		Vector2Int pos = new Vector2Int(0, 0);
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
			}
			if (foundPos)
				break;
		}

		if (!foundPos)  // no room for the item
		{
			Debug.LogWarning($"Can not store item {item.ItemName} in inventory");
			return false;
		}
		
		// Debug.LogWarning("player added item to inventory: " + item.ItemName + ". At position: " + pos.ToString());
		m_inventoryItems.Add(item);
		m_inventoryItemPositions.Add(item, pos);
		return true;
	}

	public bool HasItem(string itemName)
	{
		foreach (InventoryItem item in m_inventoryItems)
		{
			if (item.ItemName == itemName)
			{
				return true;
			}
		}
		return false;
	}

	public bool MoveItem(InventoryItem item, Vector2Int position)
	{
		if (!m_inventoryItems.Contains(item))
		{
			return false; // inventory does not contain this item, function call is not valid
		}

		if (CanItemBePlaced(item, position))
		{
			m_inventoryItemPositions[item] = position;
			return true;
		}

		return false;	// that slot is taken or item is too big and goes out of bounds
	}

	public Dictionary<InventoryItem, Vector2Int> GetInventoryItems() 
	{
		return m_inventoryItemPositions;
	}

	public int GetInventoryItemCount()
	{
		return m_inventoryItems.Count;
	}

	public bool CanItemBePlaced(InventoryItem item, Vector2Int position)
	{
		foreach(Vector2Int itemBlock in item.ItemPositions)
		{
			if (!IsPositionFree(itemBlock + position))
			{
				return false;
			}
		}
		return true;
	}

	public bool IsPositionFree(Vector2Int position)
	{
		if (position.x >= INVENTORY_SIZE_HEIGHT || position.y >= INVENTORY_SIZE_WIDTH)
		{
			return false;
		}
		foreach (var kvp in m_inventoryItemPositions)
		{
			Vector2Int pos = kvp.Value;	// starting point of item in the inventory grid ( like a relative 0,0 )
			foreach (Vector2Int blockPos in kvp.Key.ItemPositions)
			{
				if (blockPos + pos == position)
				{
					return false;
				}
			}
		}
		return true;
	}
}
