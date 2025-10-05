using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour {

    [SerializeField] private Button startButton;
    [SerializeField] private Toggle colorblindToggle;
    [SerializeField] private Transform colorbindPrefab;

    private void Awake() {
        if (ColorblindMode.Instance != null) {
            colorblindToggle.isOn = ColorblindMode.Instance.IsColorblindMode();
        } else {
            colorblindToggle.isOn = false;
        }
        
        startButton.onClick.AddListener(OnStartClicked);
        colorblindToggle.onValueChanged.AddListener(OnColorblindToggled);
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
}
