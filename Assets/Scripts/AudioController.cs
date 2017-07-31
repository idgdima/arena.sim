using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public static AudioController Global;

	public AudioSource audioSource;
	public AudioClip[] AudioClips;

	void Awake () {
		Global = this;
	}

	void Start () {
		
		MenuMusic ();
	}

	void MenuMusic () {
		audioSource.clip = AudioClips [0];
		audioSource.Play ();
	}

	public void StartGame () {
		
		audioSource.Stop ();
		audioSource.clip = AudioClips [Random.Range (1, 3)];
		audioSource.Play ();
	}

}
