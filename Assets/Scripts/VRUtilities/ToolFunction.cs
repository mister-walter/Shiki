﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

// ToolFunction.cs
//
// This is the first iteration of a tool component.
// It's current function is to check whether or not the tool has hit an object, 
// and if so, if it has hit it hard enough. If the object is hit hard enough, the
// tool will destroy the object. Obviously this is not the function we actually want,
// but is something easily testable and integratable to the project currently. Once
// we figure out how we want our tools to affect other objects, we can add that functionally.

public class ToolFunction : MonoBehaviour {
    private GameObject material; //object player is colliding tool with
    private float velocity; //velocity with which to apply tool force
    public float strengthThreshold = 0.25f; //strength at which collision of object produces a result

    void OnCollisionEnter(Collision col) {
        if (col.other.GetComponent<ReplaceableBehavior>() != null)
        {
            velocity = col.relativeVelocity.magnitude;
            material = col.gameObject;
            hit(material, velocity);
        }
    }

    void hit(GameObject material, float velocity) {
        if(velocity >= strengthThreshold) //if player collides tool with object hard enough
            EventManager.FireEvent(new ObjectHitEvent(material));
    }
}
