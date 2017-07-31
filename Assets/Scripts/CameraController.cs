using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController Instance;

    public float ShakeDecreaseFactor;

    private float _shakeAmount;

    private void Awake() {
        Instance = this;
    }

    public void Shake(float amount) {
        _shakeAmount = amount;
    }

    private void Update() {
        if (_shakeAmount > 0) {
            var newPosition = Random.insideUnitCircle * _shakeAmount;
            transform.localPosition = new Vector3(newPosition.x, newPosition.y, transform.localPosition.z);
            _shakeAmount -= Time.deltaTime * ShakeDecreaseFactor;
        } else {
            transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
        }
    }
}