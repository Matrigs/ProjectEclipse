using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

[RequireComponent(typeof(PlayerComponent))]
public class InputController : MonoBehaviour 
{
	PlayerComponent playerComp;
	[Range(1,2)]
	public int playerNumber;
	public bool isis = false;
	InputDevice device;	

	private bool _controls;
	[SerializeField]
	private playerKeyBindings pKeys;
	void OnEnable()
	{
		playerComp = GetComponent<PlayerComponent>();
		string filePath;
		if(!isis) filePath  = Path.Combine(Application.streamingAssetsPath, "PKEYS" + playerNumber + ".json");
		else filePath = Path.Combine(Application.streamingAssetsPath, "PKEYSISIS" + ".json");
		
		if(File.Exists(filePath))
		{
			string dataAsJson = File.ReadAllText(filePath);
			pKeys = JsonUtility.FromJson<playerKeyBindings>(dataAsJson);
		}
		else
		{
			Debug.Log("Error Finding Files");
		}
	}
	void Update()
	{		
		if(InputManager.Devices.Count == playerNumber)
		{
			device = InputManager.Devices[playerNumber-1];
			if(device.Action1.WasPressed)
			{
				playerComp.OnJumpInputDown();
			}
			if(device.Action1.WasReleased)
			{
				playerComp.OnJumpInputUp();
			}
			if(device.Action2.WasPressed)
			{
				playerComp.OnActionDown();
			}
		}

		Vector2 directionalInput;
		_controls = (InputManager.ActiveDevices.Count != 0);

		if(!_controls)
		{
			directionalInput = new Vector2(Input.GetAxisRaw(pKeys.axis), Input.GetAxisRaw("Vertical"));
		}
		else
		{
			directionalInput = device.LeftStick;
		}

		playerComp.SetDirectionalInput(directionalInput);		
		
		if(Input.GetKeyDown(pKeys.jumpKey.ToLower()))
		{
			playerComp.OnJumpInputDown();
		}
		if(Input.GetKeyUp(pKeys.jumpKey.ToLower()))
		{
			playerComp.OnJumpInputUp();
		}
        if(Input.GetKeyDown(pKeys.actionKey))
        {
            playerComp.OnActionDown();
        }
    }
}
[System.Serializable]
public class playerKeyBindings
{
	public string axis;
	public string jumpKey;
	public string actionKey;
}
