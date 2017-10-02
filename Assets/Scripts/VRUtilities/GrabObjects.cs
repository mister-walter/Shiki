using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;
using Shiki.EventSystem.InternalEvents;
using Shiki.Inventory;

public class GrabObjects : MonoBehaviour {

    // https://www.raywenderlich.com/149239/htc-vive-tutorial-unity

    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    // device to get easy access to the controller
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    // reference to object being tracked
    private SteamVR_TrackedObject trackedObj;

    private GameObject objInHand;       // currently holding
    private GameObject collidingObj;    // object that currently colliding with

    void Start() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update() {
        CheckButtonStatus();
    }

    private void CheckButtonStatus() {
        if(controller.GetPressDown(triggerButton)) {
            if(collidingObj) {
                GrabObject();
            }
        }

        if(controller.GetPressUp(triggerButton)) {
            if(objInHand) {
                ReleaseObject();
            }
        }

        if(controller.GetPressDown(gripButton)) {
            EventManager.FireEvent(new PlayerOpenedInventoryEvent());
        }
    }

    public void OnTriggerEnter(Collider c) {
        SetCollidingObject(c);
    }

    public void OnTriggerStay(Collider c) {
        SetCollidingObject(c);
    }

    public void OnTriggerExit(Collider c) {
        if(!collidingObj) {
            return;
        }
        collidingObj = null;
    }

    private void SetCollidingObject(Collider c) {
        // can't grab the thing if it's not a rigidbody or if player is already holding something
        if(collidingObj || !c.attachedRigidbody) {
            return;
        }
        collidingObj = c.attachedRigidbody.gameObject;
    }

    private void GrabObject() {
        objInHand = collidingObj;
        collidingObj = null;

        // this connects the new object to the controller so it acts as part of the controller collision-wise
        var joint = AddFixedJoint();
        joint.connectedBody = objInHand.GetComponent<Rigidbody>();

        // Get the seasonal effect of the object, if any
        var seasonalEffect = objInHand.GetComponentInSelfOrImmediateParent<SeasonalEffect>();
        // Fire an event to notify any listeners
        EventManager.FireEvent(new ObjectPickedUpEvent(objInHand, seasonalEffect));
    }

    private FixedJoint AddFixedJoint() {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void DestroyAllJoints() {
        foreach(var joint in this.gameObject.GetComponents<FixedJoint>()) {
            joint.connectedBody = null;
            Destroy(joint);
        }
    }

    private void ReleaseObject() {
        if(GetComponent<FixedJoint>()) {
            DestroyAllJoints();
            if(objInHand.HasComponentAnd<InventoryItemBehavior>((iib) => iib.IsInsideTarget())) {
                EventManager.FireEvent(new ObjectPlacedOnInventoryTargetEvent(objInHand));
            } else {
                if(objInHand.HasComponentAnd<InventoryItemBehavior>((iib) => iib.HasTarget())) {
                    EventManager.FireEvent(new ObjectRemovedFromInventoryTargetEvent(objInHand));
                }

                var rigidbody = objInHand.GetComponent<Rigidbody>();
                // code borrowed from https://www.reddit.com/r/vrdev/comments/51l5dy/unity_physics_problem_with_vive_thrown_objects/de16oon/?st=j89mbmud&sh=64f3bd29
                Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
                if(origin != null) {
                    rigidbody.velocity = origin.TransformVector(controller.velocity);
                    rigidbody.GetRelativePointVelocity(origin.TransformVector(controller.angularVelocity));
                }
                rigidbody.AddForce(controller.velocity);
                rigidbody.angularVelocity = controller.angularVelocity;
                // end borrowed code

                // original throwing thing that didn't work always
//                rigidbody.velocity = controller.velocity;
//                rigidbody.angularVelocity = controller.angularVelocity;
                // Get the seasonal effect of the object, if any
                var seasonalEffect = objInHand.GetComponentInSelfOrImmediateParent<SeasonalEffect>();
                // Fire an event to notify any listeners
                EventManager.FireEvent(new ObjectPlacedEvent(objInHand, seasonalEffect));
            }
        }
        objInHand = null;
    }
}
