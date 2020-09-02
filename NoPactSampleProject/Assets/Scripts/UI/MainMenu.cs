using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu> {

    [SerializeField] private Button _bobBtn = null;
    [SerializeField] private Button _aliceBtn = null;


    private void Start() {

        _bobBtn.onClick.AddListener(HandleBobClicked);
        _aliceBtn.onClick.AddListener(HandleAliceClicked);
    }

    private void HandleBobClicked() {

        GameManager.Instance.selectedCharacter = "bob";
        StartGame();
    }

    private void HandleAliceClicked() {

        GameManager.Instance.selectedCharacter = "alice";
        StartGame();
    }

    private void StartGame() {

        GameManager.Instance.SetPlayingState();
    }

}
