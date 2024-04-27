using UnityEngine.InputSystem;

public interface IInputReceiver
{
	public void OnReceiveInputDirectional(DirectionalInput directionalInput);
	public void OnReceiveInputAction();
	public void OnReceiveInputCancel();
}
