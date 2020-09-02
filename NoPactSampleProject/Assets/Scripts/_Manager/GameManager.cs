using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager> {

    public enum GameState { PREGAME, MAIN_MENU, PLAYING }

    public GameObject[] SystemPrefabs;

    public event Action<GameState, GameState> OnGameStateChanged;

    private string _currentSceneName = string.Empty;

    public GameState currentGameState { get; private set; } = GameState.PREGAME;

    private List<GameObject> _instancedSystemPrefabs;
    private List<AsyncOperation> _sceneLoadOperations;

    private string _gameSceneName = "GameScene";

    private string _lastSceneNameLoading = string.Empty;
    private string _lastSceneNameUnloading = string.Empty;

    public string selectedCharacter = "bob";

    #region Start & Initialize

    private void Start() {

        DontDestroyOnLoad(gameObject);
        _instancedSystemPrefabs = new List<GameObject>();
        _sceneLoadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();

        SetMainMenuState();
    }


    private void InstantiateSystemPrefabs() {

        GameObject prefabInstance;

        for (int i = 0; i < SystemPrefabs.Length; i++) {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        for (int i = 0; i < _instancedSystemPrefabs.Count; i++) {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }

    #endregion

    #region State Management

    private void UpdateState(GameState state) {

        GameState previousGameState = currentGameState;
        currentGameState = state;

        OnGameStateChanged?.Invoke(previousGameState, currentGameState);
    }

    public void SetMainMenuState() {
        UpdateState(GameState.MAIN_MENU);
    }

    public void SetPlayingState() {
        LoadSceneAsync(_gameSceneName);
    }

    public void OpenMainMenu() {
        SetMainMenuState();
        _lastSceneNameUnloading = _currentSceneName;
        _currentSceneName = "";
        UnloadSceneAsync(_lastSceneNameUnloading);
    }

    #endregion

    #region Scene Management

    public void LoadSceneAsync(string sceneName) {

        if (_lastSceneNameLoading != sceneName || _currentSceneName != sceneName) {
            _lastSceneNameLoading = sceneName;

            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            if (ao == null) {
                Debug.LogError("[GameManager] unable to load scene" + sceneName);
                return;
            }

            if (_sceneLoadOperations.Count <= 0) {
                _currentSceneName = sceneName;
                ao.completed += OnLoadSceneAsyncComplete;
                _sceneLoadOperations.Add(ao);
            }
        }
    }

    public void UnloadSceneAsync(string sceneName, string sceneAfter = "") {

        _lastSceneNameUnloading = sceneName;

        AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneName);

        if (ao == null) {
            Debug.LogError("[GameManager] unable to unload scene" + sceneName);
            return;
        }

        ao.completed += OnUnloadSceneAsyncComplete;
    }


    private void OnLoadSceneAsyncComplete(AsyncOperation ao) {

        _lastSceneNameLoading = "";

        if (_sceneLoadOperations.Contains(ao)) {
            _sceneLoadOperations.Remove(ao);

            if (_sceneLoadOperations.Count == 0) {
                if (IsLevelScene(_currentSceneName)) {
                    UpdateState(GameState.PLAYING);
                }
            }
        }

    }

    private void OnUnloadSceneAsyncComplete(AsyncOperation ao) {

        _lastSceneNameUnloading = "";

        if (IsLevelScene(_lastSceneNameUnloading)) {
            UpdateState(GameState.MAIN_MENU);
        }
    }

    private bool IsLevelScene(string sceneName) {
        return sceneName == _gameSceneName;
    }

    #endregion
}
