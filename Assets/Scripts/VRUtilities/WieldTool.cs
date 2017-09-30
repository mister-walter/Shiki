using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.EventSystem.InternalEvents;

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
    // reference to object colliding with trigger
    private GameObject collidingObj;
    // reference to object in hand
    private GameObject objInHand;
    // reference to tools hold position
    private Transform holdPosition;
    

    private bool firstClick = false; //true when player clicks trigger button for the first time
    private float clickTimer = 0.0f;


    // Use this for initialization
    void Start() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update() {
        CheckButtonStatus();
    }

    private IEnumerator DoubleClick() {
        yield return new WaitForEndOfFrame();
        firstClick = true;
        while(clickTimer < 0.2f) {
            if(controller.GetPressDown(triggerButton)) {
                Debug.Log("Double click");
                if(collidingObj.GetComponent<ToolFunction>() != null) {
                    GrabObject();
                }
                break;
            }
            clickTimer += Time.deltaTime;
            yield return null;
        }
        firstClick = false;
        clickTimer = 0.0f;
    }

    private void CheckButtonStatus() {
        if(controller.GetPressUp(triggerButton) && !firstClick) {
            StartCoroutine(DoubleClick());
        }


        // grip button let go of
        if(controller.GetPressDown(gripButton)) {
            if(objInHand) {
                ReleaseObject();
            }
        }
    }

    public void OnTriggerEnter(Collider c) {
        Debug.Log("Trigger entered");
        SetCollidingObject(c);
    }

    public void OnTriggerStay(Collider c) {
        SetCollidingObject(c);
    }

    public void OnTriggerExit(Collider c) {
        //		Debug.Log("Tigger exited");
        if(!collidingObj) {
            return;
        }
        collidingObj = null;
    }

    private void SetCollidingObject(Collider c) {
        // can't grab the thing if it's not a rigidbody or if player is already holding something
        if(collidingObj != null || c.attachedRigidbody == null) {
            Debug.Log("Cant grab this");
            return;
        }
        collidingObj = c.attachedRigidbody.gameObject;
    }

    private void GrabObject() {
        objInHand = collidingObj;
        collidingObj = null;

        Vector3 hand = gameObject.transform.position;
        Quaternion angle = gameObject.transform.rotation;

        

        if((objInHand.GetComponent<isTool>()) != null)
        {
            holdPosition = objInHand.transform.GetChild(0); //get hold position transform
            var worldCoord = holdPosition.position;

            Vector3 positionDifference = hand - worldCoord;

            objInHand.transform.Rotate(90, 0, 0, Space.Self); //adjust rotation for pointing tool
            objInHand.transform.Translate(positionDifference); //adjust position for where the hold position is (hopefully)
        } else {
            objInHand.transform.position = hand; //set position of tool to hand position
            objInHand.transform.rotation = angle; //set rotation of tool to hand rotation
        }
         

        

        // this connects the new object to the controller so it acts as part of the controller collision-wise
        var joint = AddFixedJoint();
        joint.connectedBody = objInHand.GetComponent<Rigidbody>();
        //objInHand.transform.localPosition.Set(0, 0, 0);

        // Get the seasonal effect of the object, if any
        var seasonalEffect = objInHand.GetComponent<SeasonalEffect>();
        // Fire an event to notify any listeners
        EventManager.FireEvent(new ObjectPickedUpEvent(objInHand, seasonalEffect));
    }

    private FixedJoint AddFixedJoint() {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject() {
        if(GetComponent<FixedJoint>()) {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            objInHand.GetComponent<Rigidbody>().velocity = controller.velocity;
            objInHand.GetComponent<Rigidbody>().angularVelocity = controller.angularVelocity;

            // Get the seasonal effect of the object, if any
            var seasonalEffect = objInHand.GetComponent<SeasonalEffect>();
            // Fire an event to notify any listeners
            EventManager.FireEvent(new ObjectPlacedEvent(objInHand, seasonalEffect));
        }
        objInHand = null;
    }
}
