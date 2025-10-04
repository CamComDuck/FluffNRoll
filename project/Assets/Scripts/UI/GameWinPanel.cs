using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameWinPanel : MonoBehaviour {

    [SerializeField] private Button menuButton;
    [SerializeField] private TMP_Text timeLabel;

    private void Awake() {
        SetTimeLabel();
    }

    public Button GetMenuButton() {
        return menuButton;
    }

    public void SetTimeLabel() {
        float gameTime = GameSM.Instance.GetGameTime();
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime - (minutes * 60));
        String minutesLabel;
        String secondsLabel;

        if (minutes < 10) {
            minutesLabel = "0" + minutes;
        } else {
            minutesLabel = ""+minutes;
        }

        if (seconds < 10) {
            secondsLabel = "0" + seconds;
        } else {
            secondsLabel = ""+seconds;
        }

        timeLabel.text = "Game Completed In " + minutesLabel + ":" + secondsLabel;
    }
}
