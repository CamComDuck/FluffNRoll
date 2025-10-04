using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour {

    [SerializeField] private GameWinPanel gameEndPrefab;

    private void Start() {
        GameSM.Instance.OnGameStateStarted += GameSM_OnGameStateStarted;
    }

    private void GameSM_OnGameStateStarted(object sender, GameSM.GameEventArgs e) {
        if (e.gameState == GameSM.GameState.GameOverWin) {
            SpawnPanel(true);
        } else if (e.gameState == GameSM.GameState.GameOverLose) {
            SpawnPanel(false);
        }
    }

    private void SpawnPanel(bool isWin) {
        GameInput.DisablePlayerInputActions();
        Time.timeScale = 0f;

        GameWinPanel newPanel = Instantiate(gameEndPrefab);
        newPanel.transform.SetParent(gameObject.transform, false);
        newPanel.GetMenuButton().onClick.AddListener(OnMenuButtonClicked);
    }

    private void OnMenuButtonClicked() {
        SceneManager.LoadScene("TitleScene");
    }
    
}
