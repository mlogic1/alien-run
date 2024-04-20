using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
	public TextMeshProUGUI DebugTextComponent;
	public float Velocity = 1.0f;
	public float JumpStrength = 0.25f;

	private Rigidbody2D m_body;
	private bool m_isGrounded = false;


	private void Awake()
	{
		m_body = GetComponent<Rigidbody2D>();
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

		string debugText = "Horizontal input: " + Input.GetAxis("Horizontal").ToString() + "\r\n";
		debugText += "Is Gounded: " + m_isGrounded;
		DebugTextComponent.SetText(debugText);
	}

	void Update()
	{
		WriteDebugData();
		m_body.velocity = new Vector2(Input.GetAxis("Horizontal") * Velocity, m_body.velocity.y);

		if (Input.GetKey(KeyCode.Space) && m_isGrounded)// || Input.GetButton("JUMP"))
		{
			// m_body.AddForce(new Vector2(0, 0.15f), ForceMode2D.Impulse);
			m_body.velocity += JumpStrength * Vector2.up;
			m_isGrounded = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			m_isGrounded = true;
			Debug.Log("Player collided with " + collision.gameObject.tag.ToString());
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			m_isGrounded = false;
			Debug.Log("Player uncollided");
		}
	}
}
