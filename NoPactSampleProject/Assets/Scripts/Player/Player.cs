using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {

    private enum PlayerState { Idle, Running, Jumping }
    protected enum WalkingSpeed { Slow, Normal, Fast }

    private PlayerState _currentState = PlayerState.Idle;

    [Header("Player Common")]
    [Space]
    [SerializeField] private WalkingSpeed _walkingSpeed = WalkingSpeed.Normal;
    //[SerializeField] private LayerMask _groundLayer;

    [SerializeField] private float _normalWalkingSpeed = 5.0f;
    [SerializeField] private float _jumpForce = 6.0f;

    private Rigidbody2D rigid;
    private PlayerAnimation playerAnim;
    private SpriteRenderer playerSprite;

    private bool _isGrounded = true;
    private bool _checkGrounded = false;
    private bool _canApplySpecialJump = true;

    protected bool isApplyingSpecialJump = false;
    protected bool isDirectedRight = true;

    #region Init

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<PlayerAnimation>();
        playerSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    protected virtual void Start() {
        InitRunSpeed();
    }

    private void InitRunSpeed() {

        switch (_walkingSpeed) {
            case WalkingSpeed.Slow:
                _normalWalkingSpeed -= 2.0f;
                break;
            case WalkingSpeed.Fast:
                _normalWalkingSpeed += 2.0f;
                break;
            default:
                break;
        }
    }

    #endregion

    #region State Management

    private void UpdateState(PlayerState state) {

        _currentState = state;

        switch (_currentState) {

            case PlayerState.Idle:
                playerAnim.Jump(false);
                playerAnim.Idle();
                SetVelocity(0, rigid.velocity.y);
                break;

            case PlayerState.Running:
                playerAnim.Run(_normalWalkingSpeed);
                break;

            case PlayerState.Jumping:
                playerAnim.Jump(true);
                SetVelocity(rigid.velocity.x, _jumpForce);
                _isGrounded = false;
                StartCoroutine(ResetJumpRoutine());
                break;

            default:
                break;
        }
    }
    #endregion

    #region Update

    protected virtual void Update() {

        if (!isApplyingSpecialJump) {
            Movement();
        }

        if (_checkGrounded) {
            CheckGrounded();
        }

    }

    private void Movement() {

        if (InputManager.Instance.jump) {

            if (_currentState != PlayerState.Jumping && _isGrounded && IsHitGround()) {
                UpdateState(PlayerState.Jumping);

            } else if (_canApplySpecialJump) {
                ApplySpecialJump();
            }
        }

        float move = InputManager.Instance.move;

        SetVelocity(move * _normalWalkingSpeed, rigid.velocity.y);

        if (Mathf.Abs(move) > Mathf.Epsilon) {

            isDirectedRight = move > 0;
            Flip(!isDirectedRight);

            if (_currentState == PlayerState.Idle) {
                UpdateState(PlayerState.Running);
            }

        } else {

            if (_currentState == PlayerState.Running) {
                UpdateState(PlayerState.Idle);
            }
        }

    }

    private void Flip(bool isLeft) {
        playerSprite.flipX = isLeft;
    }

    public void SetVelocity(float x, float y) {
        rigid.velocity = new Vector2(x, y);
    }

    IEnumerator ResetJumpRoutine() {
        _checkGrounded = false;
        yield return new WaitForSeconds(0.1f);
        _checkGrounded = true;
    }

    #endregion

    #region Virtual Methods

    protected virtual void ApplySpecialJump() {
        _canApplySpecialJump = false;
    }

    protected virtual void Atttack() {

    }

    #endregion

    #region Ground Check

    private void CheckGrounded() {

        if (IsHitGround()) {
            ResetFlags();
            UpdateState(PlayerState.Idle);
        } else {
            _isGrounded = false;
        }
    }

    private void ResetFlags() {

        _isGrounded = true;
        _checkGrounded = false;
        _canApplySpecialJump = true;
    }

    private bool IsHitGround() {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, 1 << 8);
        Debug.DrawRay(transform.position, Vector2.down * 1.0f, Color.green);

        return hitInfo.collider != null;
    }

    #endregion
}