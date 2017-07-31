using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HitPoints))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class BasePlayer : MonoBehaviour, IShootable {
    public const float Size = 0.5f;
    private const float RotationOffset = -90;

    public EntityCenter EntityCenter;
    public SpriteRenderer Renderer;
    public SpriteRenderer ShadowRenderer;
    public Color DamageColor;
    public float ShootCoooldown;
    public float Acceleration;
    public float MaxVelocity;
    public AudioClip DamageAudio;
    public AudioClip DeathAudio;

    public Action EntityDiedListener;

    protected int ShootLayersMask;
    protected HitPoints HitPoints;
    protected float ShootCooldownModifier = 1;

    private Rigidbody2D _rigidbody2D;
    private bool _canShoot = true;
    private float _maxVelocitySquared;
    private AudioSource _audioSource;

    protected virtual void Awake() {
        ShootLayersMask = LayerMask.GetMask("Obstacles");

        _audioSource = GetComponent<AudioSource>();

        EntityCenter.LeftArenaListener = KillPlayer;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        HitPoints = GetComponent<HitPoints>();

        HitPoints.DeathListener = OnPlayerDied;
        _maxVelocitySquared = MaxVelocity * MaxVelocity;
        
        Restart();
    }

    private void KillPlayer() {
        HitPoints.TakingDamage(HitPoints.CurHp);
    }

    private void OnPlayerDied() {
        StartCoroutine(PlayerDeathAnimationCoroutine());
        EntityDiedListener();
        PlayAudioClip(DeathAudio);
    }

    private IEnumerator PlayerDeathAnimationCoroutine() {
        for (var i = 0; i < 9; i++) {
            Renderer.color = i % 2 == 0 ? DamageColor : Color.white; 
            yield return new WaitForSeconds(0.1f);
        }
    }

    public virtual void Restart() {
        ShootCooldownModifier = 1;
        HitPoints.Reset();
        Renderer.color = Color.white;
        _canShoot = true;
        StopAllCoroutines();
    }

    protected void ApplyForce(Vector2 force) {
        _rigidbody2D.AddForce(force);
        if (_rigidbody2D.velocity.sqrMagnitude > _maxVelocitySquared) {
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * MaxVelocity;
        }
    }

    protected void TryShoot(Vector3 target) {
        if (_canShoot && HitPoints.IsAlive) {
            var direction = GetDirectionVector(target);
            ProjectilesController.Instance.SpawnProjectile(transform.position, direction, ShootLayersMask);
            StartCoroutine(ShootCooldownCoroutine());
        }
    }

    private IEnumerator ShootCooldownCoroutine() {
        _canShoot = false;
        yield return new WaitForSeconds(ShootCoooldown * ShootCooldownModifier);
        _canShoot = true;
    }

    public void OnShoot(Projectile projectile) {
        var direction = transform.position - projectile.transform.position;
        ApplyForce(direction.normalized * 200);

        PlayAudioClip(DamageAudio);
        HitPoints.TakingDamage(projectile.Damage);
    }

    protected void PlayAudioClip(AudioClip clip) {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    protected void LookTo(Vector3 target) {
        var lookDirection = target - transform.position;
        var angle = VectorUtils.GetDirectionAngle(lookDirection) + RotationOffset;
        Renderer.transform.rotation = Quaternion.Euler(0, 0, angle);
        ShadowRenderer.transform.rotation = Renderer.transform.rotation;
    }

    private Vector3 GetDirectionVector(Vector3 target) {
        var direction = target - transform.position;
        direction.z = 0;
        return direction;
    }

    public void Heal(int amount) {
        HitPoints.TakingDamage(-amount);
    }
}