using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
	public InventoryWindow InventoryWindow;
	public Inventory PlayerInventory;
	// Start is called before the first frame update
	void Awake()
	{
		InventoryWindow.InitializeWindow(PlayerInventory);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			if (!InventoryWindow.gameObject.activeSelf)
			{
				InventoryWindow.Refresh();
				InventoryWindow.gameObject.SetActive(true);
			}
			else
			{
				InventoryWindow.gameObject.SetActive(false);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			InventoryWindow.gameObject.SetActive(false);
		}
	}
}
