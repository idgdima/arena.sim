using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IShootable {

	public AudioSource audioSource;
	
	public void OnShoot(Projectile projectile) {
		audioSource.Play ();
	}
}
