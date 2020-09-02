using Cinemachine;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

    [SerializeField] private GameObject _bobPrefab;
    [SerializeField] private GameObject _alicePrefab;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera = null;

    private void Start() {

        bool isBob = GameManager.Instance.selectedCharacter == "bob";
        GameObject player = Instantiate(isBob ? _bobPrefab : _alicePrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        _virtualCamera.Follow = player.transform;
    }
}
