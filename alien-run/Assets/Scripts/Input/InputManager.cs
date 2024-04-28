using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Input manager handles input based on game state.
// By default, the character receives inputs.
// At any time another system can call TakeInput to take over the input to itself.
// This can be a popup window, pause menu, dialog etc...
// To give input back to the character the system can call ReleaseInput

public class InputManager : MonoBehaviour
{
	public Player Player;

	public InventoryWindow InventoryWindow;

	private InputActionMap m_playerActionMap;
	private InputActionMap m_uiActionMap;

	private IInputReceiver m_currentInputReceiver = null;

	private void Start()
	{
		InventoryWindow.InitializeWindow(this, Player.GetComponent<Inventory>());

		PlayerInput playerInput = GetComponent<PlayerInput>();
		m_playerActionMap = playerInput.actions.FindActionMap("PlayerActionMap");
		m_uiActionMap = playerInput.actions.FindActionMap("UIActionMap");

		if (m_playerActionMap == null)
		{
			Debug.LogError("Unable to find PlayerActionMap");
		}

		if (m_uiActionMap == null)
		{
			Debug.LogError("Unable to find UIActionMap");
		}
	}

	public void TakeInput(IInputReceiver inputReceiver)
	{
		m_currentInputReceiver = inputReceiver;
		m_playerActionMap.Disable();
		m_uiActionMap.Enable();
	}

	public void ReleaseInput(IInputReceiver inputReceiver)
	{
		if (inputReceiver != m_currentInputReceiver) 
		{
			Debug.LogError("Only the current input 'owner' can release it's input");
			return;
		}
		else
		{
			m_currentInputReceiver = null;
			m_playerActionMap.Enable();
			m_uiActionMap.Disable();
		}
	}

	public void ShowHideInventory(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			if (!InventoryWindow.gameObject.activeSelf)
			{
				TakeInput(InventoryWindow);
				InventoryWindow.gameObject.SetActive(true);
			}
		}
	}

	public void OnDirectionalActionButton(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			string controlShortName = context.control.shortDisplayName;
			
			if (controlShortName == null)
			{
				// input comes from keyboard. I was having issues with ReadValue<Vector2>() always resulting in (0,0), but due to time constraints i didn't 
				// debug it further. So finally I resolved it dirty like this
				switch(context.control.name)
				{
					case "w":
						controlShortName = "D-Pad Up";
						break;
					case "a":
						controlShortName = "D-Pad Left";
						break;
					case "s":
						controlShortName = "D-Pad Down";
						break;
					case "d":
						controlShortName = "D-Pad Right";
						break;
					default:
						return;
				}
			}

			if (controlShortName.Equals("D-Pad Up"))										 // a dictionary could map these string to enum conversions, but no need 
			{                                                                               // to overcomplicate such a simple scenario
				m_currentInputReceiver.OnReceiveInputDirectional(DirectionalInput.UP);
			}
			else if (controlShortName.Equals("D-Pad Down"))
			{
				m_currentInputReceiver.OnReceiveInputDirectional(DirectionalInput.DOWN);
			}
			else if (controlShortName.Equals("D-Pad Left"))
			{
				m_currentInputReceiver.OnReceiveInputDirectional(DirectionalInput.LEFT);
			}
			else if (controlShortName.Equals("D-Pad Right"))
			{
				m_currentInputReceiver.OnReceiveInputDirectional(DirectionalInput.RIGHT);
			}
			else
			{	
				Debug.LogError("Unknown directional control: " + context.ToString());
			}
		}
	}

	public void OnActionConfirm(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			m_currentInputReceiver.OnReceiveInputAction();
		}
	}

	public void OnActionCancel(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			m_currentInputReceiver.OnReceiveInputCancel();
		}
	}
}
