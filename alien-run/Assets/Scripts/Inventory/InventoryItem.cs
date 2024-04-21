using UnityEngine;

public class InventoryItem : MonoBehaviour
{
	// [SerializeField]
	public Texture2D InventoryImage;

	public string ItemName;

	// item size for inventory
	[Range(1, 6)]
	public int ItemLength = 1;

	[Range(1, 6)]
	public int ItemWidth = 1;
}