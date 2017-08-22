using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonalEffect : MonoBehaviour {

    private string seasonName;
	
	// Update is called once per frame
	void Update () {
        UpdateColor();
	}

    void UpdateColor ()
    {
        seasonName = this.gameObject.scene.name;

        switch (seasonName)
        {
            case "Winter": this.GetComponent<Renderer>().material.SetColor("_Color", Color.blue); break; //turns blue
            case "Spring": this.GetComponent<Renderer>().material.SetColor("_Color", Color.magenta); break; //turns pink
            case "Summer": this.GetComponent<Renderer>().material.SetColor("_Color", Color.green); break; //turns green
            case "Fall": this.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow); break; //turns yellow
            default: return; //unexpected? Don't change
        }
    }
}
