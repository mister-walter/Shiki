using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class AdvancedToolScript : MonoBehaviour {

    Vector3 oldPos, newPos, deltaPos; //for storing the current position, the next position, and the change in those two
    DateTime oldTime, newTime; //for storing the current time and the next time
    Int32 deltaTime; //for storing the difference in time recorded (in ms)
    enum behavior { NONE, POUNDING, CHOPPING, DIGGING, PAINTING }; //labels for which activity is occuring
    behavior activity, lastKnown; //for storing the current activity and the last activity known
    float force; //for storing the collision velocity for determining object damage
    int numHits; //for keeping track of how many collisions have occurred
    public int hitThreshold; //how many collisions needed to occur to determine a behavior
    public int attentionSpan; //for determining if the length of time between collisions is concurrent with performing a behavior
    public float posXminThreshold, posYminThreshold, posZminThreshold; //thresholds determining how miniscule change must be to ignore
    public float posXmaxThreshold, posYmaxThreshold, posZmaxThreshold; //thresholds determining max limit of change to determine behavior


	// Use this for initialization
	void Start () {
        numHits = 0;
        activity = behavior.NONE;
        lastKnown = behavior.NONE;
        force = 0;
	}
	
    public void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision detected");
        //initialize position and time values
        if(numHits == 0) //no prior collision
        {
            oldPos = col.contacts[0].point; //get the first point to collide
            newPos = col.contacts[0].point; //get the first point to collide
            oldTime = DateTime.UtcNow;
            newTime = DateTime.UtcNow;
        }
        else //prior collision detected
        {
            oldPos = newPos;
            newPos = col.contacts[0].point; //get the first point to collide
            oldTime = newTime;
            newTime = DateTime.UtcNow;
        }
        //calculate deltas
        deltaPos = newPos - oldPos;
        deltaTime = (Int32) newTime.Subtract(oldTime).TotalMilliseconds;
        //increment number of collisions detected
        numHits += 1;
        //keep track of how much damage to deal if behavior is classified
        force += col.relativeVelocity.magnitude;

        //determine if a behavior might be happening
        if(numHits >= hitThreshold && deltaTime <= attentionSpan)
        {
            classify(col);
        }

    }

    void classify(Collision col)
    {
        //focus on x, ignore y and z
        if (Math.Abs(deltaPos.x) <= posXmaxThreshold && (Math.Abs(deltaPos.y) > posYminThreshold && Math.Abs(deltaPos.z) > posZminThreshold))
        {
            activity = behavior.PAINTING;
            Debug.Log(activity);
            if (activity != lastKnown && lastKnown != behavior.NONE) //user isn't hitting object in line with any behavior
                resetTracking();
            else
            {
                //col.gameObject.hit(activity, force);
                force = 0; //reset so that force does not linearly increase if activity continues (don't deal damage that's already been dealt)
                lastKnown = activity;
            }
        }
        //focus on y, ignore x and z
        else if (Math.Abs(deltaPos.y) <= posYmaxThreshold && (Math.Abs(deltaPos.x) > posXminThreshold && Math.Abs(deltaPos.z) > posZminThreshold))
        {
            activity = behavior.CHOPPING;
            Debug.Log(activity);
            if (activity != lastKnown && lastKnown != behavior.NONE) //user isn't hitting object in line with any behavior
                resetTracking();
            else
            {
                //col.gameObject.hit(activity, force);
                force = 0; //reset so that force does not linearly increase if activity continues (don't deal damage that's already been dealt)
                lastKnown = activity;
            }
        }
        //focus on z, ignore x and y
        else if (Math.Abs(deltaPos.z) <= posZmaxThreshold && (Math.Abs(deltaPos.x) > posXminThreshold && Math.Abs(deltaPos.y) > posYminThreshold))
        {
            activity = behavior.POUNDING;
            Debug.Log(activity);
            if (activity != lastKnown && lastKnown != behavior.NONE) //user isn't hitting object in line with any behavior
                resetTracking();
            else
            {
                //col.gameObject.hit(activity, force);
                force = 0; //reset so that force does not linearly increase if activity continues (don't deal damage that's already been dealt)
                lastKnown = activity;
            }
        }
        else
            return;
    }   

    void resetTracking()
    {
        numHits = 0;
        activity = behavior.NONE;
        lastKnown = behavior.NONE;
        force = 0;
        Debug.Log("Reset Tracking");
    }
}
