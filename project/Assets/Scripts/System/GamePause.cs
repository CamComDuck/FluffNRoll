using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePause : MonoBehaviour {

    [SerializeField] private PausePanel gamePausePrefab;

    public static event EventHandler OnUnpauseClicked;

    private PausePanel spawnedPanel;

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

        spawnedPanel.GetUnpauseButton().onClick.AddListener(OnUnpauseButtonClicked);
        spawnedPanel.GetMenuButton().onClick.AddListener(OnMenuButtonClicked);
    }

    private void OnUnpauseButtonClicked() {
        Destroy(spawnedPanel.gameObject);
        OnUnpauseClicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnMenuButtonClicked() {
        Debug.Log("TODO: Transition to main menu");
    }
    
}
