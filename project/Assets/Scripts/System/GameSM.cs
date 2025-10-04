using System;
using UnityEditor;
using UnityEngine;

public class GameSM : MonoBehaviour {

    public static GameSM Instance { get; private set; }

    public event EventHandler<GameEventArgs> OnGameStateStarted;
    public class GameEventArgs : EventArgs {
        public GameState gameState;
    }

    public enum GameState {
        WaitingToStart,
        GamePlaying,
        Paused,
        GameOverWin,
        GameOverLose
    }

    private GameState gameState;
    private float gameTime = 0f;

    private void Awake() {
        if (Instance != null) Debug.LogError("There is more than 1 GameSM!");
        Instance = this;

        TransitionState(GameState.GamePlaying);
    }

    private void Update() {
        if (IsGamePlaying()) {
            gameTime += Time.deltaTime;
        }
    }

    private void Start() {
        SheepBarnCounter.Instance.OnGameWon += SheepBarnCounter_OnGameWon;

        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GamePause.OnUnpauseClicked += GamePause_OnUnpauseClicked;
    }

    private void TransitionState(GameState newState) {
        if (newState == GameState.GamePlaying) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

        gameState = newState;

        if (!IsGamePlaying()) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        OnGameStateStarted?.Invoke(this, new GameEventArgs {
            gameState = gameState
        });
    }

    private void SheepBarnCounter_OnGameWon(object sender, System.EventArgs e) {
        TransitionState(GameState.GameOverWin);
    }

    private void GamePause_OnUnpauseClicked(object sender, System.EventArgs e) {
        Time.timeScale = 1f;
        TransitionState(GameState.GamePlaying);
    }

    private void GameInput_OnPauseAction(object sender, System.EventArgs e) {
        if (IsGamePlaying()) { // Pause
            Time.timeScale = 0f;
            TransitionState(GameState.Paused);

        } else if (gameState == GameState.Paused) { // Unpause
            Time.timeScale = 1f;
            TransitionState(GameState.GamePlaying);
        }
    }

    public bool IsGamePlaying() {
        return gameState == GameState.GamePlaying;
    }

    public bool IsGameOver() {
        return gameState == GameState.GameOverLose || gameState == GameState.GameOverWin;
    }

    public float GetGameTime() {
        return gameTime;
    }
    
}
