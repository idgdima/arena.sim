using UnityEngine;
using System.Collections;

public class HitPointsCore : MonoBehaviour {


	public static HitPointsCore Global;
	public HPBar HPBarPrefab;
	public Transform Holder;
	public Transform LeftPortrait;
	public Transform RightPortrait;



	void Awake () {
		Global = this;
	}
}

