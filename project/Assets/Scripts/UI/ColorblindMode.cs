using UnityEngine;

public class ColorblindMode : MonoBehaviour {

    public static ColorblindMode Instance { get; private set; }

    private bool isColorblindMode = false;

    private void Awake() {
        if (Instance != null) Debug.LogError("No more than 1 ColorblindMode");
        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public void ToggleColorblindMode() {
        isColorblindMode = !isColorblindMode;
    }

    public bool IsColorblindMode() {
        return isColorblindMode;
    }
}
