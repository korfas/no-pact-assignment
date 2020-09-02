using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public Animator _anim { get; private set; }
    public SpriteRenderer _playerSprite { get; private set; }
    public BoxCollider2D _collider { get; private set; }

    [SerializeField] private float _runSpeedAnimMultiplier = 0.25f;

    void Awake() {
        _anim = GetComponentInChildren<Animator>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }

    public void Move(float move) {
        _anim.SetFloat("Move", Mathf.Abs(move));
    }

    public void Idle() {
        _anim.SetFloat("Move", 0);
    }

    public void Run(float speed) {
        _anim.SetFloat("Move", 1);
        _anim.SetFloat("RunSpeed", speed * _runSpeedAnimMultiplier);
    }

    public void Jump(bool jumping) {
        _anim.SetBool("Jumping", jumping);
    }

}