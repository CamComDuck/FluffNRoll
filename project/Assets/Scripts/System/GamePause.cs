using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePause : MonoBehaviour {

    [SerializeField] private Transform gamePausePrefab;

    public static event EventHandler OnUnpauseClicked;

    private Transform spawnedPanel;

    private void Start() {
        GameSM.Instance.OnGameStateStarted += GameSM_OnGameStateStarted;
    }

    public static void ResetStaticData() {
        OnUnpauseClicked = null;
    }

    private void GameSM_OnGameStateStarted(object sender, GameSM.GameEventArgs e) {
        if (e.gameState == GameSM.GameState.Paused) {
            if (!spawnedPanel) {
                SpawnPanel();
            }
        } else {
            if (spawnedPanel) {
                Destroy(spawnedPanel.gameObject);
            }
        }
    }

    private void SpawnPanel() {
        spawnedPanel = Instantiate(gamePausePrefab);
        spawnedPanel.transform.SetParent(gameObject.transform, false);
        spawnedPanel.gameObject.SetActive(true);

        Button restartButton = GetComponentInChildren<Button>();
        restartButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked() {
        Destroy(spawnedPanel.gameObject);
        OnUnpauseClicked?.Invoke(this, EventArgs.Empty);
    }
    
}
