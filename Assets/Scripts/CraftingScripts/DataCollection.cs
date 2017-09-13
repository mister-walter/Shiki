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
    public bool gripButtonDown = false;
    public bool gripButtonUp = false;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    // device to get easy access to the controller
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    // reference to object being tracked
    private SteamVR_TrackedObject trackedObj;
    //*********************************//
    // END REFERENCE TO VR CONTROLLERS //
    //*********************************//

    string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    // Use this for initialization
    void Start() {

        trackedObj = GetComponent<SteamVR_TrackedObject>();

        using (StreamWriter datafile = new StreamWriter(mydocpath + @"\data.csv"))
        {
            datafile.WriteLine("time, p-x, p-y, p-z");
        }

        
    }

    // Update is called once per frame
    void Update() {
        record();
    }

    public void record()
    {
        string row;

        Int32 i_time = (Int32)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        string time = i_time.ToString();
        float pos_x = trackedObj.transform.position.x;
        float pos_y = trackedObj.transform.position.y;
        float pos_z = trackedObj.transform.position.z;

        row = time + "," + pos_x + "," + pos_y + "," + pos_z;

        using (StreamWriter datafile = new StreamWriter(mydocpath + @"\data.csv", true)) {
            datafile.WriteLine(row);
        }
        
    }
}
