using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pickup : MonoBehaviour {
    public const float Radius = 0.1666667f;

    public PickupType Type;

    private SpriteRenderer _renderer;

    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public Sprite Sprite {
        set { _renderer.sprite = value; }
    }

    public void PickUp(HumanPlayer player) {
        PickupText.Instance.ShowPickup(Type);
        
        gameObject.SetActive(false);

        switch (Type) {
            case PickupType.Health:
                player.Heal(50);
                break;
            case PickupType.Expand:
                Arena.Instance.Expand(0.2f);
                break;
            case PickupType.FireRate:
                player.ChangePowerRate(0.5f, 5f);
                break;
            case PickupType.ShootThroughWalls:
                player.ShootThroughWalls(5f);
                break;
        }
    }
}