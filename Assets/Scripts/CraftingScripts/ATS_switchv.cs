using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ATS_switchv : MonoBehaviour {

    Vector3 oldPos, newPos, deltaPos;
    DateTime oldTime, newTime;
    Int32 deltaTime;
    Vector3 oldNormal, newNormal, deltaNormal;

    enum behavior { NONE, POUNDING, CHOPPING, DIGGING, PAINTING };
    behavior activity, lastKnown;

    float force;
    int numHits;

    public int hitThreshold;
    public int attentionSpan;
    public float posXmin, posYmin, posZmin;
    public float posXmax, posYmax, posZmax;

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

        //intialize position, time, and normal values
        if (numHits == 0) { //no prior collision
            oldPos = col.contacts[0].point;
            newPos = col.contacts[0].point;
            oldNormal = col.contacts[0].normal;
            newNormal = col.contacts[0].normal;
            oldTime = DateTime.UtcNow;
            newTime = DateTime.UtcNow;
        }
        else { //prior collision detected
            oldPos = newPos;
            newPos = col.contacts[0].point;
            oldNormal = newNormal;
            newNormal = col.contacts[0].normal;
            oldTime = newTime;
            newTime = DateTime.UtcNow;
        }

        //calculate deltas
        deltaPos = newPos - oldPos;
        deltaTime = (Int32)newTime.Subtract(oldTime).TotalMilliseconds;
        deltaNormal = newNormal - oldNormal;

        string debug = "oldPos: " + oldPos + "\tnewPos: " + newPos + "\tdeltaPos: " + deltaPos +
                       "\noldTime: " + oldTime + "\tnewTime: " + newTime + "\tdeltaTime: " + deltaTime +
                       "\noldNormal: " + oldNormal + "\tnewNormal: " + newNormal + "\tdeltaNormal: " + deltaNormal;
        Debug.Log(debug);

        //increment number of collisions detected
        numHits += 1;

        //keep track of how much damage to deal if behavior is classified
        force += col.relativeVelocity.magnitude;

        //determine if a behavior is happening, if so, classify it
        if (numHits >= hitThreshold && deltaTime <= attentionSpan)
            classify(col);
    }

    void classify(Collision col)
    {
        Debug.Log("Classifying...");
    }

    void resetTracking()
    {
        Debug.Log("Reset Tracking");
        numHits = 0;
        activity = behavior.NONE;
        lastKnown = behavior.NONE;
        force = 0;
    }
}
