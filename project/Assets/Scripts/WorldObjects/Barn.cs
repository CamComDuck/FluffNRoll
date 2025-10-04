using System;
using System.Collections.Generic;
using UnityEngine;

public class Barn : MonoBehaviour {
    [SerializeField] private List<MeshRenderer> sheepColorMeshRenderers;
    [SerializeField] private SheepSO targetSheepSO; // Will be set programmatically

    private List<Sheep> sheepInBarn = new();
    private int totalSheepNeeded;

    private void Start() {
        foreach (MeshRenderer meshRenderer in sheepColorMeshRenderers) {
            List<Material> settingMaterials = new() {
                targetSheepSO.GetMaterial()
            };
            meshRenderer.SetMaterials(settingMaterials);
        }

        Sheep[] allSheepInScene = FindObjectsByType<Sheep>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (Sheep sheep in allSheepInScene) {
            if (sheep.GetSheepSO() == targetSheepSO) {
                totalSheepNeeded += 1;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        Sheep collidedSheep = other.GetComponentInParent<Sheep>();
        if (collidedSheep != null) { // collided with sheep
            if (!sheepInBarn.Contains(collidedSheep) && collidedSheep.IsStanding() && targetSheepSO == collidedSheep.GetSheepSO()) {
                sheepInBarn.Add(collidedSheep);
                collidedSheep.ArrivedAtBarn();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        Sheep collidedSheep = other.GetComponentInParent<Sheep>();
        if (collidedSheep != null) { // collided with sheep
            if (sheepInBarn.Contains(collidedSheep)) {
                sheepInBarn.Remove(collidedSheep);
            }
        }
    }

    public bool HasAllSheepRequired() {
        return sheepInBarn.Count >= totalSheepNeeded;
    }
}
