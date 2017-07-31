using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Arena : MonoBehaviour {
    public static Arena Instance;

    public Transform Mask;
    public float ShrinkSpeed;
    public float MinScale;
    public float MaxScale;
    public ParticleSystem ArenaParticles;

    public CircleCollider2D Collider { get; private set; }


    private void Awake() {
        Instance = this;

        Collider = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate() {
        var newScale = ArenaScale - ShrinkSpeed * Time.fixedDeltaTime;
        newScale = Mathf.Max(newScale, MinScale);
        ArenaScale = newScale;
    }

    public void Restart() {
        ArenaScale = 1;
    }

    public float ArenaScale {
        get {
            return Collider.radius;
        }
        private set {
            Mask.localScale = Vector3.one * value;
            var mainModule = ArenaParticles.main;
            mainModule.startSize = value * 4.5f;
            Collider.radius = value;
        }
    }

    public void Expand(float amount) {
        ArenaScale = Mathf.Clamp(ArenaScale + amount, MinScale, MaxScale);
    }
}