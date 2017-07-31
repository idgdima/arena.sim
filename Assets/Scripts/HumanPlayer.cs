using System.Collections;
using UnityEngine;


public class HumanPlayer : BasePlayer {
    public static HumanPlayer Instance;

    public AudioClip PickupAudio;

    protected override void Awake() {
        base.Awake();

        Instance = this;

        ShootLayersMask = LayerMask.GetMask("Obstacles", "Enemy");
    }

    private void Update() {
		if (Input.GetMouseButton(0) || Input.GetKeyDown (KeyCode.JoystickButton5)||Input.GetKeyDown (KeyCode.JoystickButton0)) {
            TryShoot(GetWorldMousePosition());
        }
    }

    private void FixedUpdate() {
        UpdatePositionAndRotation();
    }

    private void UpdatePositionAndRotation() {
        if (!HitPoints.IsAlive) {
            return;
        }

        var inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (inputVector.sqrMagnitude > 1) {
            inputVector = inputVector.normalized;
        }
        var force = inputVector * Time.fixedDeltaTime * Acceleration;
        ApplyForce(force);

		LookTo (GetWorldMousePosition ());
    }

    private Vector3 GetWorldMousePosition() {
		if (Options.Global.UseGamePad == true) {
			Vector3 MoveVector = new Vector3 (Input.GetAxis ("Axis 3"), -Input.GetAxis ("Axis 4"), 0) * 10;
			if (MoveVector != Vector3.zero) {
				return transform.position + MoveVector;
			}else {
				return transform.position + Renderer.transform.up;
			}
		} else {
			return Camera.main.ScreenToWorldPoint (Input.mousePosition);
		}
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickups")) {
            PlayAudioClip(PickupAudio);
            other.GetComponent<Pickup>().PickUp(this);
        }
    }

    public void ChangePowerRate(float newShootCooldownModifier, float duration) {
        ShootCooldownModifier = newShootCooldownModifier;
        StartCoroutine(RestoreCooldownCoroutine(duration));
    }

    private IEnumerator RestoreCooldownCoroutine(float duration) {
        yield return new WaitForSeconds(duration);
        ShootCooldownModifier = 1;
    }

    public void ShootThroughWalls(float duration) {
        ShootLayersMask = LayerMask.GetMask("Enemy");
        StartCoroutine(DisableShootThroughWallsCoroutine(duration));
    }

    private IEnumerator DisableShootThroughWallsCoroutine(float duration) {
        yield return new WaitForSeconds(duration);
        ShootLayersMask = LayerMask.GetMask("Obstacles", "Enemy");
    }
}