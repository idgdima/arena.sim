using UnityEngine;

public class BgOverlay : MonoBehaviour {
    private void Update() {
        var newY = (transform.localPosition.y + 0.5f * Time.deltaTime) % 2;
        transform.localPosition = new Vector3(0, newY, 0);
    }
}