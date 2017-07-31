using System;
using UnityEngine;

public class HitPoints : MonoBehaviour {
    public int MaxHp = 100;
    public int CurHp = 100;

    public HPBar HpBar;

    public Action DeathListener;

    public bool IsAlive {
        get { return CurHp > 0; }
    }

    private void Awake() {
        Reset();
    }

    public void Reset() {
        CurHp = MaxHp;
        HpBar.UpdateHP(CurHp, MaxHp);
    }

    private void Start() {
        HpBar.UpdateHP(CurHp, MaxHp);
    }

    public void TakingDamage(int damage) {
        var oldIsAlive = IsAlive;

        CurHp = Mathf.Clamp(CurHp - damage, 0, MaxHp);
        HpBar.UpdateHP(CurHp, MaxHp);

        if (oldIsAlive && !IsAlive && DeathListener != null) {
            DeathListener();
        }
    }
}