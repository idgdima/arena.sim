using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GameOverText : MonoBehaviour {
    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void ShowAnimation() {
        _animator.SetTrigger("start");
    }
}