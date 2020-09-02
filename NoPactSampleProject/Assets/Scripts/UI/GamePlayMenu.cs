using UnityEngine;
using UnityEngine.UI;

public class GamePlayMenu : Singleton<GamePlayMenu> {

    [SerializeField] private GameObject _virtualControlsPanel = null;
    [SerializeField] private Button _toMainMenuBtn = null;

    private void Start() {

        _toMainMenuBtn.onClick.AddListener(HandleToMainMenuClicked);
        SetVirtualConsoleAvailability();
    }

    private void SetVirtualConsoleAvailability() {

#if UNITY_EDITOR
        _virtualControlsPanel.SetActive(false);

#elif UNITY_ANDROID || UNITY_IOS
        _virtualControlsPanel.SetActive(true);
#else
        _virtualControlsPanel.SetActive(false);
#endif

    }

    private void HandleToMainMenuClicked() {

        GameManager.Instance.OpenMainMenu();
    }

    public void SetMove(float move) {
        InputManager.Instance.moveTouch = move;
    }

    public void SetJump(bool jump) {
        InputManager.Instance.jumpTouch = jump;
    }

}
