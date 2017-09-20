using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine;

public class DataCollection : MonoBehaviour {

    //***********************************//
    // BEGIN REFERENCE TO VR CONTROLLERS //
    //***********************************//
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
    //*********************************//
    // END REFERENCE TO VR CONTROLLERS //
    //*********************************//

    private GameObject collidingObject;
    string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    // Use this for initialization
    void Start() {

        trackedObj = GetComponent<SteamVR_TrackedObject>();

        using (StreamWriter datafile = new StreamWriter(mydocpath + @"\data.csv", true))
        {
            datafile.WriteLine("v-x, v-y, v-z, av-x, av-y, av-z"); //remember to change this back later to user-id, activity, time, pos-x, pos-y, pos-z
        }

        
    }

    // Update is called once per frame
 /*   void Update() {
        triggerButtonDown = controller.GetPressDown(triggerButton);

        if (triggerButtonDown)
            record();
        
    }*/

    void OnCollisionEnter(Collision col)
    {
        collidingObject = col.gameObject;
        float vx = col.relativeVelocity.x;
        float vy = col.relativeVelocity.y;
        float vz = col.relativeVelocity.z;
        float avx = col.rigidbody.angularVelocity.x;
        float avy = col.rigidbody.angularVelocity.y;
        float avz = col.rigidbody.angularVelocity.z;

        record(vx, vy, vz, avx, avy, avz);

    }

    public void record(float vx, float vy, float vz, float avx, float avy, float avz)
    {
        string row;

       /* int user_id = 1;
        string activity = "pounding";

        Int32 i_time = (Int32)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        string time = i_time.ToString();
        float pos_x = trackedObj.transform.position.x;
        float pos_y = trackedObj.transform.position.y;
        float pos_z = trackedObj.transform.position.z;*/

        row = vx + "," + vy + "," + vz + "," + avx + "," + avy + "," + avz; //remember to change this back later to user_id + activity + time + pos_x + pos_y + pos_z

        using (StreamWriter datafile = new StreamWriter(mydocpath + @"\data.csv", true)) {
            datafile.WriteLine(row);
        }
        
    }
}
