using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{

    //private string tooltype;
    private float hp;
    private Material newMaterial;
    

    // Use this for initialization
    void Start()
    {
       //tooltype = "stick";
        hp = 1.0f;
    }

    public void hit(float velocityMag)
    {
        hp -= velocityMag;

        if (hp <= 0)
        {
            hp = 0;
            changeObject();
        }
    }

    public void changeObject()
    {
        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Cube); //create a new object

        Vector3 pos = transform.position;   //get old object position
        Vector3 scale = transform.localScale; //get old object size
        Quaternion rotation = transform.rotation; //get old object rotation
        newMaterial = Resources.Load("Affected", typeof(Material)) as Material; //load up the new material
        newObject.GetComponent<Renderer>().material = newMaterial; //assign new material to object
        newObject.AddComponent<Rigidbody>();

        newObject.transform.position = pos; //assign old object's position to new object
        newObject.transform.localScale = scale; //assign old object's scale to new object
        newObject.transform.rotation = rotation; //assign old object's rotation to new object

        DestroyObject(gameObject); //destroy the old object
    }

}
