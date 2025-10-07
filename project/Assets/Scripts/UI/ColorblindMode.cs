using System.Collections.Generic;
using UnityEngine;

public class ColorblindMode : MonoBehaviour {

    public static ColorblindMode Instance { get; private set; }

    private bool isColorblindMode = false;
    private float mouseSensitivity = 1f;

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

    public void SetMouseSensitivity(float newValue) {
        mouseSensitivity = newValue;
    }

    public float GetMouseSensitivity() {
        return mouseSensitivity;
    }
}
