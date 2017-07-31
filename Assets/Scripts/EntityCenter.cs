using System;
using UnityEngine;

public class EntityCenter : MonoBehaviour {
    public Collider2D ArenaCollider;

    public Action LeftArenaListener;

    private void OnTriggerExit2D(Collider2D other) {
        if (other == ArenaCollider && LeftArenaListener != null) {
            LeftArenaListener.Invoke();
        }
    }
}