using UnityEngine;
using System.Collections.Generic;

public class GridSystem : MonoBehaviour {

    public static GridSystem Instance { get; private set; }

    private const int TILE_SIZE = 5;

    private Direction direction = Direction.DOWN;

    public enum Direction {
        DOWN,
        LEFT,
        UP,
        RIGHT,
        NULL
    }

    private void Awake() {
        if (Instance != null) Debug.LogError("There is more than one GridBuildingSystem instance!");
        Instance = this;
    }

    public PlacedObjOnGrid BuildOnGrid(Transform placingPrefab, GridXZ<GridTile> gridPlacingOn, Vector2Int placingGridPosition) {
        Vector2Int objectGridSize;
        if (placingPrefab.GetComponent<Barn>() != null) {
            objectGridSize = new(6, 6);
        } else {
            objectGridSize = new(1, 1);
        }
        Vector2Int rotationOffset = GetRotationOffset(direction, objectGridSize);
        Vector3 rotationWorldPosition = new Vector3(rotationOffset.x, 0, rotationOffset.y) * gridPlacingOn.GetCellSize();
        Vector3 tileWorldPosition = gridPlacingOn.GetWorldPosition(placingGridPosition);
        Vector3 placedFurnitureWorldPosition = tileWorldPosition + rotationWorldPosition;

        PlacedObjOnGrid placedObject = PlacedObjOnGrid.Create(placedFurnitureWorldPosition, placingGridPosition, direction, placingPrefab);
        List<Vector2Int> gridPositionList = GetObjectGridPositions(placingGridPosition, direction, objectGridSize);

        foreach (Vector2Int gridPosition in gridPositionList) {
            gridPlacingOn.GetGridObject(gridPosition).SetPlacedObject(placedObject);
        }
        return placedObject;
    }

    // Returns if object would be placed sucessfully
    public bool IsValidPositionToPlaceOnGrid(GridXZ<GridTile> gridPlacingOn, Vector2Int placingGridPosition, Transform placingPrefab) {
        if (placingGridPosition.x >= 0 && placingGridPosition.y >= 0 && placingGridPosition.x < gridPlacingOn.GetGridSize().x && placingGridPosition.y < gridPlacingOn.GetGridSize().y) {
            Vector2Int objectGridSize;
            if (placingPrefab.GetComponent<Barn>() != null) {
                objectGridSize = new(6, 6);
            } else {
                objectGridSize = new(1, 1);
            }
            List<Vector2Int> gridPositionList = GetObjectGridPositions(placingGridPosition, direction, objectGridSize);

            foreach (Vector2Int gridPosition in gridPositionList) {
                if (gridPlacingOn.GetGridObject(gridPosition) == null) {
                    // Debug.LogWarning("Object would be partly outside grid!");
                    return false;

                } else if (gridPlacingOn.GetGridObject(gridPosition).HasObject()) {
                    // Debug.LogWarning("Object would be partly inside another object!");
                    return false;
                }
            }
            return true;
        } else {
            // Debug.LogWarning("Outside of grid!");
            return false;
        }
    }

    public List<Vector2Int> GetObjectGridPositions(Vector2Int offset, Direction dir, Vector2Int objectGridSize) {
        List<Vector2Int> objectGridPositions = new();
        switch (dir) {
            default:
            case Direction.DOWN:
            case Direction.UP:
                for (int x = 0; x < objectGridSize.x; x++) {
                    for (int y = 0; y < objectGridSize.y; y++) {
                        objectGridPositions.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Direction.LEFT:
            case Direction.RIGHT:
                for (int x = 0; x < objectGridSize.y; x++) {
                    for (int y = 0; y < objectGridSize.x; y++) {
                        objectGridPositions.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }
        return objectGridPositions;
    }

    public Vector2Int GetRotationOffset(Direction dir, Vector2Int gridSize) {
        switch (dir) {
            default:
            case Direction.DOWN: return new Vector2Int(0, 0);
            case Direction.LEFT: return new Vector2Int(0, gridSize.x);
            case Direction.UP: return new Vector2Int(gridSize.x, gridSize.y);
            case Direction.RIGHT: return new Vector2Int(gridSize.y, 0);
            case Direction.NULL: return new Vector2Int(-1, -1);
        }
    }

    public int GetRotationAngle(Direction dir) {
        switch (dir) {
            default:
            case Direction.DOWN: return 0;
            case Direction.LEFT: return 90;
            case Direction.UP: return 180;
            case Direction.RIGHT: return 270;
            case Direction.NULL: return -1;
        }
    }
}