using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolScript : MonoBehaviour {

    private GameObject collidingObject;

    //public ObjectScript colObject;
	// Use this for initialization
	void Start () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        collidingObject = col.gameObject;
        float strength = col.relativeVelocity.magnitude;

        if (collidingObject.GetComponent("ObjectScript"))
        {
            collidingObject.GetComponent<ObjectScript>().hit(strength);
            Debug.Log(strength);
        }
        /*collidingObject = col.gameObject;
        ObjectScript other = (ObjectScript) collidingObject.GetComponent(typeof(ObjectScript));
        other.changeObject();*/
    }
}
