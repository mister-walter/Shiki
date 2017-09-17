using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{

    //private string tooltype;
    private float hp;
    public Material newMaterial;
    private bool isChanged = false;
    

    // Use this for initialization
    void Start()
    {
       //tooltype = "stick";
        hp = 5.0f;
    }

    public void hit(float velocityMag)
    {
        hp -= velocityMag;

        if (hp <= 0)
        {
            hp = 0;
            if(!isChanged)
                changeObject();
        }
    }

    public void changeObject()
    {
        gameObject.GetComponent<Renderer>().material = newMaterial; //assign new material to object 
        isChanged = true;     
    }

}
