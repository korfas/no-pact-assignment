using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBob : Player {

    [Header("Ground Breaker")]
    [Space]
    [SerializeField] private float _groundBreakSpeed;

    protected override void ApplySpecialJump() {
        base.ApplySpecialJump();

        isApplyingSpecialJump = true;
    }

    protected override void Atttack() {
        base.Atttack();
    }

    protected override void Update() {
        base.Update();

        if (isApplyingSpecialJump) {
            SetVelocity(0, _groundBreakSpeed * -1.0f);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        isApplyingSpecialJump = false;
    }
}
