using System.Collections.Generic;
using UnityEngine;

public class ProjectilesController : MonoBehaviour {
    public static ProjectilesController Instance;

    public GameObject ProjectilePrefab;

    private List<Projectile> _projectiles;
    private Queue<Projectile> _freeProjectiles;

    private void Awake() {
        Instance = this;

        _projectiles = new List<Projectile>();
        _freeProjectiles = new Queue<Projectile>();
    }

    public void Restart() {
        RemoveAllProjectiles();
    }

    private void RemoveAllProjectiles() {
        foreach (var projectile in _projectiles) {
            FreeProjectile(projectile);
        }
        _projectiles.Clear();
    }

    public void SpawnProjectile(Vector3 position, Vector3 direction, int targetLayersMask) {
        var projectile = GetFreeProjectile();
        var normalizedDirection = direction.normalized;
        projectile.transform.position = position + normalizedDirection * (BasePlayer.Size + Projectile.Size) / 2f;
        projectile.Direction = normalizedDirection;
        projectile.AffectedLayersMask = targetLayersMask;
        _projectiles.Add(projectile);
    }

    private Projectile GetFreeProjectile() {
        if (_freeProjectiles.Count != 0) {
            var projectile = _freeProjectiles.Dequeue();
            projectile.gameObject.SetActive(true);
            projectile.ResetState();
            return projectile;
        }

        var newProjectile = Instantiate(ProjectilePrefab).GetComponent<Projectile>();
        newProjectile.transform.parent = transform;
        return newProjectile;
    }

    public void FreeProjectile(Projectile projectile) {
        projectile.gameObject.SetActive(false);
        _freeProjectiles.Enqueue(projectile);
    }
}