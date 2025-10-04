using System;
using System.Collections.Generic;
using UnityEngine;

public class Barn : MonoBehaviour {
    [SerializeField] private List<MeshRenderer> sheepColorMeshRenderers;
    [SerializeField] private SheepSO targetSheep; // Will be set programmatically

    private List<Sheep> sheepInBarn = new();

    private void Start() {
        foreach (MeshRenderer meshRenderer in sheepColorMeshRenderers) {
            List<Material> settingMaterials = new() {
                targetSheep.GetMaterial()
            };
            meshRenderer.SetMaterials(settingMaterials);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Sheep collidedSheep = other.GetComponentInParent<Sheep>();
        if (collidedSheep != null) { // collided with sheep
            if (!sheepInBarn.Contains(collidedSheep) && collidedSheep.IsStanding() && targetSheep == collidedSheep.GetSheepSO()) {
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
}
