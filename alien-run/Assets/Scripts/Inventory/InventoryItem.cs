using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
	// [SerializeField]
	public Texture2D InventoryImage;

	public string ItemName;

	public List<Vector2Int> ItemPositions;

	// Since item pickup is based on CollisionBox2D's TriggerEnter function,
	// multiple TriggerEnter can happen before the picked object is actually destroyed by Destroy().
	// This bool is set to true immediately when an item gets added to Inventory so the same item does not get added twice by accident
	[HideInInspector]
	public bool IsCollected = false;
}