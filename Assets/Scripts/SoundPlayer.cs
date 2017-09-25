﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

public class SoundPlayer : MonoBehaviour {
    private AudioSource audioSource;

	void Start () {
        this.audioSource = GetComponent<AudioSource>();
        EventManager.AttachDelegate<PlaySoundEvent>(this.OnPlaySoundEvent);
	}

    void OnDestroy() {
        EventManager.RemoveDelegate<PlaySoundEvent>(this.OnPlaySoundEvent);
    }

    void OnPlaySoundEvent(PlaySoundEvent evt) {
        Debug.Log("Got PlaySoundEvent");
        var audioClip = Resources.Load<AudioClip>("Sounds/" + evt.soundName);
        this.audioSource.PlayOneShot(audioClip);
    }
}
