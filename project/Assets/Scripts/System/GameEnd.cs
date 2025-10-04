using UnityEngine;

public class GameEnd : MonoBehaviour {

    [SerializeField] private GameObject gameEndPrefab;

    private void Start() {
        // GameSM.Instance.OnGameStateStarted += GameSM_OnGameStateStarted;
    }

    // private void GameSM_OnGameStateStarted(object sender, GameSM.GameEventArgs e) {
    //     if (e.gameState == GameSM.GameState.GameOverWin) {
    //         SpawnPanel(true);
    //     } else if (e.gameState == GameSM.GameState.GameOverLose) {
    //         SpawnPanel(false);
    //     }
    // }

    // private void SpawnPanel(bool isWin) {
    //     GameInput.DisablePlayerInputActions();
    //     Time.timeScale = 0f;

    //     GameObject newPanel = Instantiate(gameEndPrefab);
    //     newPanel.transform.SetParent(gameObject.transform, false);
    //     newPanel.SetActive(true);

    //     // TMP_Text endLabel = GetComponentInChildren<TMP_Text>();

    //     if (isWin) {
    //         endLabel.text = "You Won!";
    //     } else {
    //         endLabel.text = "You Lose!";
    //     }

    //     Button restartButton = GetComponentInChildren<Button>();
    //     restartButton.onClick.AddListener(OnRestartClicked);
    // }

    // private void OnRestartClicked() {
    //     SceneManager.LoadScene("TitleScene");
    // }
    
}
