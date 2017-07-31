using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PickupText : MonoBehaviour {
    public static PickupText Instance;

    private Text _text;

    private void Awake() {
        Instance = this;

        _text = GetComponent<Text>();
        _text.enabled = false;
    }

    public void ShowPickup(PickupType type) {
        _text.text = GetPickupName(type);
        _text.enabled = true;
        StartCoroutine(HideTextCoroutine());
    }

    private string GetPickupName(PickupType type) {
        switch (type) {
            case PickupType.Health:
                return "Health";
            case PickupType.Expand:
                return "Power Restored";
            case PickupType.FireRate:
                return "Fire Rate";
            case PickupType.ShootThroughWalls:
                return "Wall Hack";
            default:
                return "???";
        }
    }

    private IEnumerator HideTextCoroutine() {
        yield return new WaitForSeconds(1f);
        _text.enabled = false;
    }
}