using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shiki.EventSystem;
using Shiki.EventSystem.InternalEvents;

public class MoviePlayer : MonoBehaviour {
    public RawImage image;
    private MovieTexture tex;
	// Use this for initialization
	void Start () {
        Debug.Log(image.material);
        this.tex = (MovieTexture)image.texture;
        EventManager.FireEvent(new PrologueStartEvent());
        tex.Play();
        StartCoroutine(FindEnd(() => {
            SteamVR_Fade.Start(Color.clear, 0);
            SteamVR_Fade.Start(Color.black, 1);
            EventManager.FireEvent(new PrologueDoneEvent());
        }));
    }

    // http://answers.unity3d.com/answers/1000687/view.html
    private IEnumerator FindEnd(Action callback) {
        while(tex.isPlaying) {
            yield return 0;
        }

        callback();
        yield break;
    }
}
