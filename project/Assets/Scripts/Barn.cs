using System;
using System.Collections.Generic;
using UnityEngine;

public class Barn : MonoBehaviour {

    public static EventHandler<CorrectSheepArgs> OnCorrectSheepArrived;
    public class CorrectSheepArgs : EventArgs {
        public Sheep sheep;
    }

    [SerializeField] private List<MeshRenderer> sheepColorMeshRenderers;
    [SerializeField] private SheepSO targetSheep; // Will be set programmatically

    private List<Sheep> sheepInBarn = new();

    private void OnDestroy() {
        OnCorrectSheepArrived = null;
    }

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
                OnCorrectSheepArrived?.Invoke(this, new CorrectSheepArgs { sheep = collidedSheep });
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
