using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlice : Player {

    [Header("Forward Dash")]
    [Space]
    [SerializeField] private float _forwardDashSeconds;
    [SerializeField] private float _forwardDashSpeed;

    protected override void ApplySpecialJump() {
        base.ApplySpecialJump();

        StartCoroutine(ForwardDashRoutine());
    }

    protected override void Atttack() {
        base.Atttack();
    }

    protected override void Update() {
        base.Update();

        if (isApplyingSpecialJump) {
            float multiplier = isDirectedRight ? 1.0f : -1.0f;
            SetVelocity(_forwardDashSpeed * multiplier, 0);
        }
    }


    IEnumerator ForwardDashRoutine() {
        isApplyingSpecialJump = true;
        yield return new WaitForSeconds(_forwardDashSeconds);
        isApplyingSpecialJump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        isApplyingSpecialJump = false;
    }
}
