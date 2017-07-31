using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour {
    public static Options Global;

	public bool UseGamePad;
    public Slider VolumeSlider;
    public Transform OptionsGroup;

	public Texture2D CursorSrpite;
	public Texture2D AimSprite;

	[Header("Menu UI")]
	public float Timer;
	public Text StartText;
	public Text OptionText;
	public int Current = 0;
	public int SecondCurrent = 6;
	public int OptionCurrent = 0;


	public RectTransform[] OptionsUI;

	[Header ("Factors")]
	public float RotationSpeed;
	public float MoveSpeed;


    void Awake() {
        Global = this;
    }
		

	void Update () {
		
		float MoveFactort  = Mathf.Lerp (-15,15, Mathf.PingPong (Time.time, 1f));
		float RotateFactor = 1 * Mathf.Sin(Time.time * 3);

		foreach (RectTransform each in OptionsUI) {
//			each.position += new Vector3 (MoveFactort, MoveFactort, 0) * Time.deltaTime * MoveSpeed;
			each.localRotation = Quaternion.Euler(0, 0, RotateFactor);
		}

		Timer += Time.deltaTime;

		if (Timer >= 1) {
			Timer = 0;
			MoveUpper (Current      , StartText , 0);
			MoveUpper (SecondCurrent, StartText , 1);
			MoveUpper (OptionCurrent, OptionText, 2);
		}
	}

	void MoveUpper (int index, Text UIText, int type) {
		
		char[] NewText = UIText.text.ToCharArray(); 
		NewText[index] = char.ToLower (NewText [index]);

		if (index + 1 != NewText.Length) {
			NewText[index + 1] = char.ToUpper (NewText [index + 1]);
		} else {
			NewText[0] = char.ToUpper (NewText [0]);

			if (type == 0) {
				Current = 0;
			} 
			if (type == 1) {
				SecondCurrent = 0;
			}
			if (type == 2) {
				OptionCurrent = 0;
			}
		}
			
		UIText.text = new string (NewText);


		if (type == 0) {
			Current++;
		} 
		if (type == 1) {
			SecondCurrent++;
		}
		if (type == 2) {
			OptionCurrent++;
		}

	}

    public void ChangeVolume() {
        AudioListener.volume = VolumeSlider.value;
    }

	public void ToggleGamePad () {
		UseGamePad = !UseGamePad;
	}
		

    public void Exit() {
        Application.Quit();
    }

    public void ShowOptions() {

        OptionsGroup.gameObject.SetActive(!OptionsGroup.gameObject.activeSelf);
    }
		
}
