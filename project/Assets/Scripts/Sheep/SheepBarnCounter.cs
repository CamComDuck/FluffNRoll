using System;
using System.Collections.Generic;
using UnityEngine;

public class SheepBarnCounter : MonoBehaviour {

    public static SheepBarnCounter Instance { get; private set; }

    public event EventHandler OnGameWon;

    private Barn[] barnsInScene;
    private List<Barn> completedBarns = new();

    private void Awake() {
        if (Instance != null) Debug.LogError("There is more than 1 SheepBarnCounter!");
        Instance = this;
    }

    public void SetupBarnsInScene() {
        barnsInScene = FindObjectsByType<Barn>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
        foreach (Barn barn in barnsInScene) {
            barn.OnAllSheepArrived += Barn_OnAllSheepArrived;
            barn.OnLastSheepLeft += Barn_OnLastSheepLeft;
        }
    }

    private void Barn_OnAllSheepArrived(object sender, System.EventArgs e) {
        Barn barn = (Barn)sender;
        if (completedBarns.Contains(barn)) {
            Debug.LogError("Barn should have already been completed");
        } else {
            completedBarns.Add(barn);

            if (completedBarns.Count == barnsInScene.Length) {
                OnGameWon?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    
    private void Barn_OnLastSheepLeft(object sender, System.EventArgs e) {
        Barn barn = (Barn)sender;
        if (completedBarns.Contains(barn)) {
            completedBarns.Remove(barn);
        } else {
            Debug.LogError("Barn should have already been removed");
        }
    }
}
