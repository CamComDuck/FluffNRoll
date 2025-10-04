using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SheepSO", menuName = "Scriptable Objects/SheepSO")]
public class SheepSO : ScriptableObject {

    [SerializeField] private String sheepName;
    [SerializeField] private Material material;
    [SerializeField] private Color sheepColor;

    public String GetName() {
        return sheepName;
    }

    public Material GetMaterial() {
        return material;
    }

    public Color GetColor() {
        return sheepColor;
    }
}
