using System.Collections.Generic;
using UnityEngine;

public class PlacedObjOnGrid : MonoBehaviour {
    
    private Transform placedObject;

    public static PlacedObjOnGrid Create(Vector3 worldPosition, GridSystem.Direction dir, Transform placingPrefab) {
        Transform placedObjectTransform = Instantiate(placingPrefab, worldPosition, Quaternion.Euler(0, GridSystem.Instance.GetRotationAngle(dir), 0));

        PlacedObjOnGrid newObject = placedObjectTransform.GetComponent<PlacedObjOnGrid>();

        return newObject;
    }

    public Transform GetObject() {
        return placedObject;
    }
}