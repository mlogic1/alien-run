using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SaveManager would normally serialize data to a file and reload it when the scene is changed or the game is restarted.
// But for this simple scenario it just stores inventory data and restores it when the scene is changed. It's important for inventory to carry over.

public class SaveManager : MonoBehaviour
{
	public Inventory PlayerInventory;
	public GameObject Doughnut;
	public GameObject Chest;
	private static List<InventoryItem> m_playerInventoryItems;

	private static bool m_doughnutcollected = false;
	private static bool m_chestOpened = false;

	public void OnSceneChanging()
	{
		foreach (KeyValuePair<InventoryItem, Vector2Int> item in PlayerInventory.GetInventoryItems())
		{
			m_playerInventoryItems.Add(item.Key);

			if (item.Key.ItemName == "DoughnutPink")
			{
				m_doughnutcollected = true;
			}

			if (item.Key.ItemName == "TripleBoneKey")
			{
				m_chestOpened = true;
			}
		}
	}

	private void Start()
	{
		if (m_playerInventoryItems == null)
		{
			m_playerInventoryItems = new List<InventoryItem>();
		}
		else
		{
			foreach (InventoryItem item in m_playerInventoryItems)
			{
				PlayerInventory.AddItem(item);
			}
			m_playerInventoryItems.Clear();
		}

		if (m_doughnutcollected && Doughnut != null)
		{
			Doughnut.SetActive(false);
		}

		if (m_chestOpened && Chest != null)
		{
			Chest.SetActive(false);
		}
	}
}
