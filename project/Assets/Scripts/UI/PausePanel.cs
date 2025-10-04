using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour {

    [SerializeField] private Button unpauseButton;
    [SerializeField] private Button menuButton;

    public Button GetUnpauseButton() {
        return unpauseButton;
    }

    public Button GetMenuButton() {
        return menuButton;
    }
}
