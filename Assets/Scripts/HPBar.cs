using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour {
	float Factor;
	//	float PrevFactor;


	//UI Links
	public Image     MainBarImage;
	public Transform MainBar;
	public Transform BackBar;

	//Colors
	public Color ColorMin;
	public Color ColorMax;

	//Smooth back bar
	float BackBarScale;
	public float SmoothTime = 1.5f;


	void Start () {
		Update ();	
	}


	public void UpdateHP (float current, float max) {

		Factor = current / max;

		//It should call here, because during double invoke from HitPoints class the Update function have no time to do it
		if (BackBarScale < Factor) {
			BackBarScale = Factor;
		}



		//Rescale bars
		MainBar.localScale = new Vector3 (Factor, 1, 1);

		//Change bar color
		MainBarImage.color = Color.Lerp (ColorMin, ColorMax, Factor);

		//Set new delta
		//		PrevFactor = Factor;
	}


	void Update () {

		//Prevent rising backplane in opposite way
		if (BackBarScale > Factor) {

			float ChangeVelocity = (Factor - BackBarScale) * SmoothTime;

			//Clamp minimum velocity
			if (ChangeVelocity > -0.2f) ChangeVelocity = -0.2f;

			BackBarScale += ChangeVelocity * Time.deltaTime;
		}

		//Apply
		BackBar.localScale = new Vector3 (BackBarScale, 1, 1);

	}
}
