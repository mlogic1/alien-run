using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
	public TextMeshProUGUI DebugTextComponent;
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

	private void WriteDebugData()
	{
		if (DebugTextComponent == null) 
		{
			return;
		}

		string debugText = "Horizontal input: " + m_horizontalInput.ToString() + "\r\n";
		debugText += "Is Gounded: " + m_isGrounded;
		DebugTextComponent.SetText(debugText);
	}

	void Update()
	{
		m_horizontalInput = Input.GetAxis("Horizontal");
		WriteDebugData();
		m_body.velocity = new Vector2(m_horizontalInput * Velocity, m_body.velocity.y);

		if (Input.GetKey(KeyCode.Space) && m_isGrounded)// || Input.GetButton("JUMP"))
		{
			// m_body.AddForce(new Vector2(0, 0.15f), ForceMode2D.Impulse);
			m_body.velocity += JumpStrength * Vector2.up;
			m_isGrounded = false;
		}

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

	private void UpdateAnimator()
	{
		if (m_isGrounded)
		{
			// player is either idle or moving
			m_animator.SetBool("IsMoving", !Mathf.Approximately(m_horizontalInput, 0.0f));
		}

		m_animator.SetBool("IsGrounded", m_isGrounded); // player is in the air
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground" && collision.otherCollider.gameObject.tag == "PlayerFeet")
		{
			m_isGrounded = true;
			Debug.Log("Player collided with " + collision.gameObject.tag.ToString());
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground" && collision.otherCollider.gameObject.tag == "PlayerFeet")
		{
			m_isGrounded = false;
			Debug.Log("Player uncollided");
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Item")
		{
			Debug.Log("Something something item: " + collision.gameObject.GetComponent<InventoryItem>().ItemName);
			// collision.gameObject.dest
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
		Debug.Log("Something something exit");
	}
}
