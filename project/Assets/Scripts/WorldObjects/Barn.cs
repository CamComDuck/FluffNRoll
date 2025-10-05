using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barn : MonoBehaviour {

    public event EventHandler OnAllSheepArrived;
    public event EventHandler OnLastSheepLeft;

    [SerializeField] private List<MeshRenderer> sheepColorMeshRenderers;
    [SerializeField] private SheepSO targetSheepSO; // Will be set programmatically

    [SerializeField] private Transform sheepPanel;
    [SerializeField] private Image sheepImage;

    private List<Sheep> sheepInBarn = new();
    private int totalSheepNeeded;

    private List<Image> sheepImages = new();

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
                Image newSheepImage = Instantiate(sheepImage, sheepPanel);
                sheepImages.Add(newSheepImage);
            }
        }

        sheepPanel.GetComponent<Image>().color = targetSheepSO.GetColor();
    }

    private void OnTriggerEnter(Collider other) {
        Sheep collidedSheep = other.GetComponentInParent<Sheep>();
        if (collidedSheep != null) { // collided with sheep
            if (!sheepInBarn.Contains(collidedSheep) && collidedSheep.IsStanding() && targetSheepSO == collidedSheep.GetSheepSO()) {
                sheepInBarn.Add(collidedSheep);
                collidedSheep.ArrivedAtBarn();

                int index = sheepInBarn.Count - 1;
                sheepImages[index].color = targetSheepSO.GetColor();

                if (HasAllSheepRequired()) {
                    OnAllSheepArrived?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        Sheep collidedSheep = other.GetComponentInParent<Sheep>();
        if (collidedSheep != null) { // collided with sheep
            if (sheepInBarn.Contains(collidedSheep)) {
                sheepInBarn.Remove(collidedSheep);

                int index = sheepInBarn.Count;
                sheepImages[index].color = Color.white;

                if (sheepInBarn.Count == totalSheepNeeded - 1) {
                    OnLastSheepLeft?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        Sheep collidedSheep = other.GetComponentInParent<Sheep>();
        if (collidedSheep != null) { // collided with sheep
            if (!sheepInBarn.Contains(collidedSheep) && collidedSheep.IsStanding() && targetSheepSO == collidedSheep.GetSheepSO()) {
                sheepInBarn.Add(collidedSheep);
                collidedSheep.ArrivedAtBarn();

                int index = sheepInBarn.Count - 1;
                sheepImages[index].color = targetSheepSO.GetColor();

                if (HasAllSheepRequired()) {
                    OnAllSheepArrived?.Invoke(this, EventArgs.Empty);
                }
            } else if (sheepInBarn.Contains(collidedSheep) && !collidedSheep.IsStanding() && targetSheepSO == collidedSheep.GetSheepSO()) {
                sheepInBarn.Remove(collidedSheep);

                int index = sheepInBarn.Count;
                sheepImages[index].color = Color.white;

                if (sheepInBarn.Count == totalSheepNeeded - 1) {
                    OnLastSheepLeft?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    public bool HasAllSheepRequired() {
        return sheepInBarn.Count >= totalSheepNeeded;
    }
}
