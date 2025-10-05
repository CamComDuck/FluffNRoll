using System.Collections.Generic;
using UnityEngine;

public class PlacedObjOnGrid : MonoBehaviour {
    
    private Vector2Int origin;
    private Transform placedObject;

    public static PlacedObjOnGrid Create(Vector3 worldPosition, Vector2Int origin, Transform spawnPrefab) {
        Transform placedObjectTransform = Instantiate(spawnPrefab, worldPosition, Quaternion.identity);

        PlacedObjOnGrid placedObject = placedObjectTransform.GetComponent<PlacedObjOnGrid>();

        placedObject.origin = origin;
        return placedObject;
    }

    public Transform GetObject() {
        return placedObject;
    }
}