using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour {
    public AudioClip Level2Music;
    public AudioSource Source;

	// Use this for initialization
	void Awake () {
        Source = GetComponent<AudioSource>();
	}

    private void OnLevelWasLoaded(int level)
    {
        if (level == 2)
        {
            Source.clip = Level2Music;
            Source.Play();
        }
    }
}
