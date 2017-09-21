using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WieldTool : MonoBehaviour {

    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId padButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    public bool gripButtonDown = false;
    public bool gripButtonUp = false;
    public bool triggerButtonDown = false;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    // device to get easy access to the controller
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    // reference to object being tracked
    private SteamVR_TrackedObject trackedObj;

    // Use this for initialization
    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /*
     *
     * fuck it, I'll finish this later 
     * 
     * (but basically, if the player double clicks the trigger button near the tool object, they 
     *  will then wield the tool object. This will prevent hand fatigue since they won't have to 
     *  physically be holding the tool the entire time they want to use it. This will also orient
     *  the tool in such a way that it's always in the proper position in their hand, so that it 
     *  doesn't matter what angle their hand approaches it from when they go to pick it up. When 
     *  they are done holding the tool and want to drop it, they will double click the trigger 
     *  button again and the tool will drop. While they're holding the tool, they should not be 
     *  allowed to pick up another object with that hand. This script, once working, should probably 
     *  just be added as an extension of GrabObject.cs so that we don't have two competing functionalities.)
     * 
     */
}
