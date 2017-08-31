using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VFXViewer : MonoBehaviour {

    public GameObject[] vfxObjects;
    public List<GameObject> vfx;

    public int currentEffect;

    void Awake()
    {
        vfx = new List<GameObject>();
        for(int i = 0; i < vfxObjects.Length; i++)
        {
            GameObject temp = Instantiate(vfxObjects[i]) as GameObject;
            temp.SetActive(false);
            vfx.Add(temp);
        }

        currentEffect = 0;
        vfx[currentEffect].SetActive(true);
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
            ChangeEffect();
    }

    void ChangeEffect()
    {
        vfx[currentEffect].SetActive(false);
        currentEffect++;

        if (currentEffect >= vfx.Count)
            currentEffect = 0;

        vfx[currentEffect].SetActive(true);
    }
}
