using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour {
    public const float Size = 0.06f;

    public float Speed;
    public Vector3 Direction;
    public float MaxLifetime;
    public int AffectedLayersMask;
    public int Damage;

    private float _lifeTime;
    private bool _isMoving;
    private SpriteRenderer _spriteRenderer;
    private GameObject _particleSystem;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _particleSystem = GetComponentInChildren<ParticleSystem>().gameObject;

        ResetState();
    }

    private void FixedUpdate() {
        if (!_isMoving) {
            return;
        }

        transform.position += Direction * Speed * Time.fixedDeltaTime;
        
        _lifeTime += Time.fixedDeltaTime;
        if (_lifeTime >= MaxLifetime) {
            StartCoroutine(DestroyProjectileCoroutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (_isMoving && (LayerToLayerMask(other.gameObject.layer) & AffectedLayersMask) != 0) {
            var shootableTarget = other.GetComponent<IShootable>();
            if (shootableTarget != null) {
                shootableTarget.OnShoot(this);
            }
            var splashDirection = transform.position - other.transform.position;
            var splashAngle = VectorUtils.GetDirectionAngle(splashDirection);
            StartCoroutine(DestroyProjectileCoroutine(splashAngle));
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (_isMoving && other == Arena.Instance.Collider) {
            StartCoroutine(DestroyProjectileCoroutine());
        }
    }

    private IEnumerator DestroyProjectileCoroutine(float? particlesAngle = null) {
        _isMoving = false;
        _spriteRenderer.enabled = false;
        if (particlesAngle != null) {
            _particleSystem.transform.localRotation = Quaternion.Euler(0, particlesAngle.Value + 90, 0);
            _particleSystem.SetActive(true);
        }
        yield return new WaitForSeconds(1);
        ProjectilesController.Instance.FreeProjectile(this);
        _particleSystem.SetActive(false);
    }

    private int LayerToLayerMask(int layer) {
        return 1 << layer;
    }

    public void ResetState() {
        _lifeTime = 0;
        _isMoving = true;
        _spriteRenderer.enabled = true;
        _particleSystem.SetActive(false);
        StopAllCoroutines();
    }
}