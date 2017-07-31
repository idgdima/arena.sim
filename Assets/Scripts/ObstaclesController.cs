using System.Collections.Generic;
using UnityEngine;

public class ObstaclesController : MonoBehaviour {
    public static ObstaclesController Instance;

    public GameObject ObstaclePrefab;
    public Sprite[] Sprites;

    private Queue<GameObject> _freeObstacles;
    private List<GameObject> _obstacles;

    private void Awake() {
        Instance = this;

        _freeObstacles = new Queue<GameObject>();
        _obstacles = new List<GameObject>();
    }

    public void RegenerateObstacles() {
        RemoveAllObstacles();

        const int RESERVED = 999;
        var mapSize = 7;
        var map = new int?[mapSize, mapSize];

        //Player and enemy cells
        map[0, 3] = RESERVED;
        map[6, 3] = RESERVED;

        //Unreachable cells
        map[0, 0] = RESERVED;
        map[mapSize - 1, 0] = RESERVED;
        map[0, mapSize - 1] = RESERVED;
        map[mapSize - 1, mapSize - 1] = RESERVED;

        map[3, 3] = (int) (Random.value * Sprites.Length);

        var tries = 15;
        var obstaclesToPlace = 3;
        while (obstaclesToPlace != 0 && tries > 0) {
            tries--;
            var x = (int) (Random.value * 5);
            var y = (int) (Random.value * 5);
            if (map[x, y] == null) {
                obstaclesToPlace--;
                var spriteIndex = (int) (Random.value * Sprites.Length);
                map[x, y] = spriteIndex;
                map[mapSize - x - 1, y] = spriteIndex;
                map[x, mapSize - y - 1] = spriteIndex;
                map[mapSize - x - 1, mapSize - y - 1] = spriteIndex;
            }
        }

        for (var x = 0; x < mapSize; x++) {
            for (var y = 0; y < mapSize; y++) {
                if (map[x, y] != null && map[x, y] != RESERVED) {
                    var position = new Vector3(GetXForMapIndex(mapSize, x), GetYForMapIndex(mapSize, y));
                    PlaceObstacleAt(position, (int) map[x, y]);
                }
            }
        }
    }

    private float GetYForMapIndex(int mapSize, int index) {
        var position = -mapSize / 2f + index + 0.5f;

        if (index < mapSize / 2f) {
            position -= 0.7f;
        }

        if (index > mapSize / 2f - 1) {
            position += 0.7f;
        }

        return position;
    }

    private float GetXForMapIndex(int mapSize, int index) {
        var position = -mapSize / 2f + index + 0.5f;


        if (index < mapSize / 2f - 2) {
            position -= 0.7f;
        }

        if (index > mapSize / 2f + 1) {
            position += 0.7f;
        }

        return position;
    }

    private void PlaceObstacleAt(Vector3 position, int spriteIndex) {
        var obstacle = GetOrCreateObstacle();
        obstacle.transform.localPosition = position;
        var renderer = obstacle.GetComponent<SpriteRenderer>();
        renderer.sprite = Sprites[spriteIndex];
    }

    private void RemoveAllObstacles() {
        foreach (var obstacle in _obstacles) {
            obstacle.SetActive(false);
            _freeObstacles.Enqueue(obstacle);
        }
        _obstacles.Clear();
    }

    private GameObject GetOrCreateObstacle() {
        GameObject obstacle;
        if (_freeObstacles.Count != 0) {
            obstacle = _freeObstacles.Dequeue();
            obstacle.SetActive(true);
        } else {
            obstacle = Instantiate(ObstaclePrefab);
            obstacle.transform.parent = transform;
        }
        _obstacles.Add(obstacle);
        return obstacle;
    }
}