using UnityEngine;

public class UIManager : Singleton<UIManager> {

    [SerializeField] private GameObject UICamera = null;

    [Header("Menus")]
    [Space]
    [SerializeField] private GameObject mainMenu = null;
    [SerializeField] private GameObject gamePlayMenu = null;

    private void Start() {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManager.GameState previousState, GameManager.GameState currentState) {

        switch (currentState) {

            case GameManager.GameState.PREGAME:
                SetUICameraActive(true);
                break;

            case GameManager.GameState.MAIN_MENU:
                SetUICameraActive(true);
                SetMainMenuUI();
                break;

            case GameManager.GameState.PLAYING:
                SetUICameraActive(false);
                SetPlayScreenUI();
                break;

            default:
                break;
        }
    }

    private void SetUICameraActive(bool active) {
        UICamera.SetActive(active);
    }

    private void SetMainMenuUI() {
        mainMenu.SetActive(true);
        gamePlayMenu.SetActive(false);
    }

    private void SetPlayScreenUI() {
        mainMenu.SetActive(false);
        gamePlayMenu.SetActive(true);
    }

}
