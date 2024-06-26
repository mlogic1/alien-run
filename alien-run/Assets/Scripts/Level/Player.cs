using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	public float Velocity = 1.0f;
	public float JumpStrength = 0.25f;

	private Inventory m_inventory;
	private Rigidbody2D m_body;
	private Animator m_animator;
	private bool m_isGrounded = false;
	private float m_horizontalInput = .0f;

	private void Awake()
	{
		m_body = GetComponent<Rigidbody2D>();
		m_animator = GetComponent<Animator>();
		m_inventory = GetComponent<Inventory>();
	}

	void Start()
	{
		m_body.freezeRotation = true;
	}

	void Update()
	{
		// flip character left or right
		if (m_horizontalInput > 0.0f)
		{
			transform.localScale = Vector3.one;
		}
		if (m_horizontalInput < 0.0f)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}

		UpdateAnimator();
	}

	private void FixedUpdate()
	{
		m_body.velocity = new Vector2(m_horizontalInput * Velocity, m_body.velocity.y);
	}

	private void UpdateAnimator()
	{
		if (m_isGrounded)
		{
			// player is either idle or moving
			m_animator.SetBool("IsMoving", !Mathf.Approximately(m_horizontalInput, 0.0f));
		}

		m_animator.SetBool("IsGrounded", m_isGrounded); // player is grounded (not jumping or falling)
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground" && collision.otherCollider.gameObject.tag == "PlayerFeet")
		{
			m_isGrounded = true;
			Debug.Log("Player collided with " + collision.gameObject.tag.ToString());
		}
	}

	public Inventory GetInventory()
	{
		return m_inventory;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Tilemap")
		{
			m_isGrounded = true;
		}
		
		if (collision.gameObject.tag == "Item")
		{
			// store item in inventory
			InventoryItem item = collision.gameObject.GetComponent<InventoryItem>();
			if (!item.IsCollected)
			{
				if (m_inventory.AddItem(item))
				{
					item.IsCollected = true;
					Destroy(collision.gameObject);
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Tilemap")
		{
			m_isGrounded = false;
		}
	}

	public void Jump()
	{
		if (m_isGrounded)
		{
			m_body.velocity = new Vector2(m_body.velocity.x, 0.0f); // reset player Y velocity. Otherwise the player can achieve higher jumps if walking diagonally
			m_body.velocity += JumpStrength * Vector2.up;
			m_isGrounded = false;
		}
	}

	public void Move(InputAction.CallbackContext context)
	{
		if (context.started || context.performed)
		{
			m_horizontalInput = context.ReadValue<float>();
		}
		else if (context.canceled)
		{
			m_horizontalInput = 0.0f;
		}
	}
}
