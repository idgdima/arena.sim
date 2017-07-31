using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickupsController : MonoBehaviour {
    public static PickupsController Instance;

    public GameObject PickupPrefab;
    public Sprite[] Sprites;
    public float FirstPickupSpawnTime;
    public float NextPickupsSpawnTime;
    public float SpawnTries;

    private List<Pickup> _pickups;
    private Queue<Pickup> _freePickups;
    private int _spawningLayersMask;

    private void Awake() {
        Instance = this;

        _pickups = new List<Pickup>();
        _freePickups = new Queue<Pickup>();
        _spawningLayersMask = LayerMask.GetMask("Player", "Obstacles", "Pickups");
    }

    public void Restart() {
        RemoveAllPickups();
        StopAllCoroutines();
        StartCoroutine(SpawnPowerupsCoroutine());
    }

    private IEnumerator SpawnPowerupsCoroutine() {
        yield return new WaitForSeconds(FirstPickupSpawnTime);
        SpawnRandomPickup();
        while (true) {
            yield return new WaitForSeconds(NextPickupsSpawnTime);
            SpawnRandomPickup();
        }
    }

    private void SpawnRandomPickup() {
        var position = FindPosition();
        if (position != null) {
            var possibleTypes = (PickupType[]) Enum.GetValues(typeof(PickupType));
            var type = possibleTypes[(int) (Random.value * possibleTypes.Length)];
            SpawnPickupAt(position.Value, type);
        }
    }

    private void SpawnPickupAt(Vector3 position, PickupType type) {
        var pickup = GetOrCreatePickup();
        pickup.transform.position = position;
        pickup.Type = type;
        pickup.Sprite = Sprites[(int) type];
        _pickups.Add(pickup);
    }

    private Vector3? FindPosition() {
        for (var i = 0; i < SpawnTries; i++) {
            var distance = Random.value * Arena.Instance.ArenaScale * 4.5f;
            var angle = Random.value * 360;
            var polar = new Polar(distance, angle);
            var position = new Vector3(polar.X, polar.Y);

            if (Physics2D.OverlapCircle(position, Pickup.Radius, _spawningLayersMask) == null) {
                return position;
            }
        }
        return null;
    }

    private void RemoveAllPickups() {
        foreach (var pickup in _pickups) {
            DestroyPickup(pickup);
        }
        _pickups.Clear();
    }

    public void DestroyPickup(Pickup pickup) {
        pickup.gameObject.SetActive(false);
        _freePickups.Enqueue(pickup);
    }

    private Pickup GetOrCreatePickup() {
        if (_freePickups.Count != 0) {
            var freePickup = _freePickups.Dequeue();
            freePickup.gameObject.SetActive(true);
            return freePickup;
        }

        var newPickup = Instantiate(PickupPrefab).GetComponent<Pickup>();
        newPickup.transform.parent = transform;
        return newPickup;
    }
}