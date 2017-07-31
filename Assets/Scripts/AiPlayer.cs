using UnityEngine;

public class AiPlayer : BasePlayer {
    private const float ScanStepDegrees = 15;
    private const float ScanSamplesEachSide = 6;
    
    public static AiPlayer Instance;

    public int StartingHp;
    public int HpPerKill;
    public int MaxHp;

    private int _obstacklesLayerMask;

    protected override void Awake() {
        base.Awake();

        Instance = this;

        ShootLayersMask = LayerMask.GetMask("Obstacles", "Player");
        _obstacklesLayerMask = LayerMask.GetMask("Obstacles");
    }

    private void FixedUpdate() {
        var playerVisible = IsPlayerVisible();
        
        UpdatePositionAndRotation(playerVisible);

        if (playerVisible) {
            TryShoot(HumanPlayer.Instance.transform.position);
        }
    }

	public void DifficultyLevel (int level) {
		HitPoints.MaxHp = Mathf.Min(StartingHp + level * HpPerKill, MaxHp);
	}

    private void UpdatePositionAndRotation(bool playerVisible) {
        if (!HitPoints.IsAlive) {
            return;
        }

        var direction = FindDirectionToMove(playerVisible);
        if (direction.sqrMagnitude > 1) {
            direction = direction.normalized;
        }
        var force = direction * Time.fixedDeltaTime * Acceleration;
        ApplyForce(force);

        LookTo(HumanPlayer.Instance.transform.position);
    }

    private Vector3 FindDirectionToMove(bool playerVisible) {
        if (playerVisible) {
            return Vector3.zero;
        }

        var directionToPlayer = HumanPlayer.Instance.transform.position - transform.position;
        var polar = Polar.FromCartesian(directionToPlayer.x, directionToPlayer.y);

        var angleStep = ScanStepDegrees;

        var biggestDistance = GetDistanceToObstacle(new Vector3(polar.X, polar.Y, 0));
        var bestDirectionPolar = polar;
        for (var i = 0; i < ScanSamplesEachSide; i++) {
            polar.Angle += angleStep;
            var distance = GetDistanceToObstacle(new Vector3(polar.X, polar.Y, 0));

            if (biggestDistance != null && (distance == null || distance > biggestDistance)) {
                biggestDistance = distance;
                bestDirectionPolar = polar;
            }
        }

        polar.Angle -= angleStep * ScanSamplesEachSide;

        for (var i = 0; i < ScanSamplesEachSide; i++) {
            polar.Angle -= angleStep;
            var distance = GetDistanceToObstacle(new Vector3(polar.X, polar.Y, 0));

            if (biggestDistance != null && (distance == null || distance > biggestDistance)) {
                biggestDistance = distance;
                bestDirectionPolar = polar;
            }
        }

        return new Vector3(bestDirectionPolar.X, bestDirectionPolar.Y, 0);
    }

    private bool IsPlayerVisible() {
        return !CheckForObstacles(HumanPlayer.Instance.transform.position);
    }

    private bool CheckForObstacles(Vector3 targetPoint) {
        var direction = targetPoint - transform.position;
        var raycastHit = Physics2D.Raycast(transform.position, direction, direction.magnitude, _obstacklesLayerMask);
        return raycastHit.transform != null;
    }

    private float? GetDistanceToObstacle(Vector3 direction) {
        var raycastHit = Physics2D.Raycast(transform.position, direction, direction.magnitude, _obstacklesLayerMask);
        if (raycastHit.transform == null) {
            return null;
        }
        return raycastHit.distance;
    }
}