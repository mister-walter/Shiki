using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using System.Collections;

public class GrabObjects : MonoBehaviour {

	// https://www.raywenderlich.com/149239/htc-vive-tutorial-unity

	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	// device to get easy access to the controller
	private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

	// reference to object being tracked
	private SteamVR_TrackedObject trackedObj;

	private GameObject objInHand;		// currently holding
	private GameObject collidingObj;	// object that currently colliding with


    private bool firstClick = false; //true when player clicks trigger button for the first time
    private float clickTimer = 0.0f;

    void Start(){
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void Update () {
		CheckButtonStatus();
	}

    private IEnumerator DoubleClick() {
        yield return new WaitForEndOfFrame();
        firstClick = true;
        while(clickTimer < 0.2f) {
            if(controller.GetPressDown(triggerButton)) {
                Debug.Log("Double click");
                if(collidingObj.GetComponent<ToolScript>() != null) {
                    WieldTool();
                }
                break;
            }
            clickTimer += Time.deltaTime;
            yield return null;
        }
        firstClick = false;
        clickTimer = 0.0f;
    }

    private void CheckButtonStatus(){
        if(controller.GetPressUp(triggerButton) && !firstClick) {
            StartCoroutine(DoubleClick());
        }

        // grip button pressed down
        if(controller.GetPressDown(gripButton)){
			if(collidingObj){
				GrabObject();
			}
		}

		// grip button let go of
		if(controller.GetPressUp(gripButton)){
			if(objInHand){
				ReleaseObject();
			}
		}
	}

	public void OnTriggerEnter(Collider c){
		//		Debug.Log("Trigger entered");
		SetCollidingObject(c);
	}

	public void OnTriggerStay(Collider c){
		SetCollidingObject(c);
	}

	public void OnTriggerExit(Collider c){
		//		Debug.Log("Trigger exited");
		if(!collidingObj){
			return;
		}
		collidingObj = null;
	}

	private void SetCollidingObject(Collider c){
		// can't grab the thing if it's not a rigidbody or if player is already holding something
		if(collidingObj || !c.GetComponent<Rigidbody>()){
			return;
		}
		collidingObj = c.gameObject;
	}

	private void GrabObject(){
		objInHand = collidingObj;
		collidingObj = null;

		// this connects the new object to the controller so it acts as part of the controller collision-wise
		var joint = AddFixedJoint();
		joint.connectedBody = objInHand.GetComponent<Rigidbody>();
    
        // Get the seasonal effect of the object, if any
        var seasonalEffect = objInHand.GetComponent<SeasonalEffect>();
        // Fire an event to notify any listeners
        GameEventSystem.FireEvent(new ObjectPickedUpEvent(objInHand, seasonalEffect));
    }

    private void WieldTool() {
        objInHand = collidingObj;
        collidingObj = null;

        Vector3 hand = gameObject.transform.position;
        Quaternion angle = gameObject.transform.rotation;

        objInHand.transform.position = hand; //set position of tool to hand position
        objInHand.transform.rotation = angle; //set rotation of tool to hand rotation
        objInHand.transform.Rotate(90, 0, 0, Space.Self); //adjust rotation for pointing tool
        objInHand.transform.Translate(0, 0.5f, 0, Space.Self); //adjust position for where tool is in hand

        // this connects the new object to the controller so it acts as part of the controller collision-wise
        var joint = AddFixedJoint();
        joint.connectedBody = objInHand.GetComponent<Rigidbody>();
        //objInHand.transform.localPosition.Set(0, 0, 0);

        // Get the seasonal effect of the object, if any
        var seasonalEffect = objInHand.GetComponent<SeasonalEffect>();
        // Fire an event to notify any listeners
        GameEventSystem.FireEvent(new ObjectPickedUpEvent(objInHand, seasonalEffect));
    }

    private FixedJoint AddFixedJoint(){
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	private void ReleaseObject(){
		if(GetComponent<FixedJoint>()){
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy(GetComponent<FixedJoint>());

			objInHand.GetComponent<Rigidbody>().velocity = controller.velocity;
			objInHand.GetComponent<Rigidbody>().angularVelocity = controller.angularVelocity;

            // Get the seasonal effect of the object, if any
            var seasonalEffect = objInHand.GetComponent<SeasonalEffect>();
            // Fire an event to notify any listeners
            GameEventSystem.FireEvent(new ObjectPlacedEvent(objInHand, seasonalEffect));
        }
		objInHand = null;
	}
		
}
