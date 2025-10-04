using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour {

    [SerializeField] private Button startButton;

    private void Awake() {
        startButton.onClick.AddListener(OnStartClicked);
        Time.timeScale = 1f;
    }

    private void OnStartClicked() {
        SceneManager.LoadScene("GameScene");
    }
}
