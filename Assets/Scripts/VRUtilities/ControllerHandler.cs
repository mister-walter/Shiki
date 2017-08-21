using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHandler : MonoBehaviour {

	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	public bool gripButtonDown = false;
	public bool gripButtonUp = false;

	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	// device to get easy access to the controller
	private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

	// reference to object being tracked
	private SteamVR_TrackedObject trackedObj;

	// Use this for initialization
	void Start(){
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	// Update is called once per frame
	void Update(){
		if(controller == null){
			Debug.Log("Controller not initialized");
			return;
		}
	}

}
