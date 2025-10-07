using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour {

    [SerializeField] private Button startButton;
    [SerializeField] private Toggle colorblindToggle;
    [SerializeField] private Transform colorbindPrefab;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityLabel;

    private void Awake() {
        if (ColorblindMode.Instance != null) {
            colorblindToggle.isOn = ColorblindMode.Instance.IsColorblindMode();
            sensitivitySlider.value = ColorblindMode.Instance.GetMouseSensitivity();
            sensitivityLabel.text = ColorblindMode.Instance.GetMouseSensitivity().ToString("F1") + "x";
        } else {
            colorblindToggle.isOn = false;
            sensitivitySlider.value = 1f;
            sensitivityLabel.text = "1.0x";
        }

        startButton.onClick.AddListener(OnStartClicked);
        colorblindToggle.onValueChanged.AddListener(OnColorblindToggled);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        Time.timeScale = 1f;
    }

    private void Start() {
        if (ColorblindMode.Instance == null) {
            Instantiate(colorbindPrefab);
        }
    }

    private void OnStartClicked() {
        SceneManager.LoadScene("GameScene");
    }

    private void OnColorblindToggled(bool newValue) {
        ColorblindMode.Instance.ToggleColorblindMode();
    }

    private void OnSensitivityChanged(Single newValue) {
        ColorblindMode.Instance.SetMouseSensitivity(newValue);
        sensitivityLabel.text = newValue.ToString("F1") + "x";
    }
}
