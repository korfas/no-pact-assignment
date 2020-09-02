using UnityEngine;

public class InputManager : Singleton<InputManager> {

    public bool jump { get; private set; } = false;
    public float move { get; private set; } = 0.0f;

    public bool jumpTouch { private get; set; } = false;
    public float moveTouch { private get; set; } = 0.0f;

    private void Update() {

            Movement();
            Jump();
    }

    private void Movement() {

#if UNITY_EDITOR
        float _moveKeyboard = Input.GetAxisRaw("Horizontal");
        move = Mathf.Clamp(_moveKeyboard, -1.0f, 1.0f);

#elif UNITY_ANDROID || UNITY_IOS
        move = Mathf.Clamp(moveTouch, -1.0f, 1.0f);
#else
        float _moveKeyboard = Input.GetAxisRaw("Horizontal");
        move = Mathf.Clamp(_moveKeyboard, -1.0f, 1.0f);
#endif

    }

    private void Jump() {

        bool jumpKeyboard = Input.GetKeyDown(KeyCode.Space);

        jump = jumpKeyboard || jumpTouch;

        jumpTouch = false;
    }

}
