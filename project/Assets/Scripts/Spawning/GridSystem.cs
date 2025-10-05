using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class GridSystem : MonoBehaviour {

    public static GridSystem Instance { get; private set; }

    [SerializeField] private List<SheepSO> sheepSOs;
    [SerializeField] private Transform sheepPrefab;
    [SerializeField] private Transform barnPrefab;
    [SerializeField] private Transform grassPrefab;

    private const int TILE_SIZE = 5;
    private const int BORDER_THRESHOLD = 5;

    private GridXZ<GridTile> placingGrid;

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

    private void Start() {
        placingGrid = new GridXZ<GridTile>(new Vector2Int(70, 70), TILE_SIZE, new Vector3(-175, 0, -175), (g, v) => new GridTile(g, v));
        List<Barn> barns = new();

        List<int> randomSheepCounts = new();
        for (int i = 0; i < sheepSOs.Count; i++) {
            int randomSheepCount = Random.Range(2, 5);
            // int randomSheepCount = 1; // for testing
            randomSheepCounts.Add(randomSheepCount);
        }

        for (int i = 0; i < sheepSOs.Count; i++) {
            bool canBuild = true;
            int safetyCounter = 0;
            Vector2Int placingGridPosition;
            do {
                int randomGridX = Random.Range(BORDER_THRESHOLD, placingGrid.GetGridSize().x - BORDER_THRESHOLD);
                int randomGridZ = Random.Range(BORDER_THRESHOLD, placingGrid.GetGridSize().y - BORDER_THRESHOLD);
                placingGridPosition = new(randomGridX, randomGridZ);
                canBuild = IsValidPositionToPlaceOnGrid(placingGrid, placingGridPosition, barnPrefab);
                safetyCounter++;
            } while (!canBuild && safetyCounter < 20);

            if (canBuild) {
                PlacedObjOnGrid newObject = BuildOnGrid(barnPrefab, placingGrid, placingGridPosition);
                Barn newBarn = newObject.gameObject.GetComponent<Barn>();
                newBarn.SetTargetSheepSO(sheepSOs[i]);
                barns.Add(newBarn);
            }
        }

        int grassCount = Random.Range(150, 225);
        for (int i = 0; i < grassCount; i++) {
            bool canBuild = true;
            int safetyCounter = 0;
            Vector2Int placingGridPosition;
            do {
                int randomGridX = Random.Range(BORDER_THRESHOLD, placingGrid.GetGridSize().x - BORDER_THRESHOLD);
                int randomGridZ = Random.Range(BORDER_THRESHOLD, placingGrid.GetGridSize().y - BORDER_THRESHOLD);
                placingGridPosition = new(randomGridX, randomGridZ);
                canBuild = IsValidPositionToPlaceOnGrid(placingGrid, placingGridPosition, grassPrefab);
                safetyCounter++;
            } while (!canBuild && safetyCounter < 20);

            if (canBuild) {
                PlacedObjOnGrid newObject = BuildOnGrid(grassPrefab, placingGrid, placingGridPosition);
            }
        }

        for (int i = 0; i < sheepSOs.Count; i++) {
            for (int j = 0; j < randomSheepCounts[i]; j++) {
                bool canBuild = true;
                int safetyCounter = 0;
                Vector2Int placingGridPosition;
                do {
                    int randomGridX = Random.Range(BORDER_THRESHOLD, placingGrid.GetGridSize().x - BORDER_THRESHOLD);
                    int randomGridZ = Random.Range(BORDER_THRESHOLD, placingGrid.GetGridSize().y - BORDER_THRESHOLD);
                    placingGridPosition = new(randomGridX, randomGridZ);
                    canBuild = IsValidPositionToPlaceOnGrid(placingGrid, placingGridPosition, sheepPrefab);
                    safetyCounter++;
                } while (!canBuild && safetyCounter < 20);

                if (canBuild) {
                    PlacedObjOnGrid newObject = BuildOnGrid(sheepPrefab, placingGrid, placingGridPosition);
                    Sheep newSheep = newObject.gameObject.GetComponent<Sheep>();
                    newSheep.SetSheepSO(sheepSOs[i]);
                }
            }
        }

        foreach (Barn b in barns) {
            b.SetupSheepInScene();
        }

        SheepBarnCounter.Instance.SetupBarnsInScene();
    }

    public PlacedObjOnGrid BuildOnGrid(Transform placingPrefab, GridXZ<GridTile> gridPlacingOn, Vector2Int placingGridPosition) {
        Vector2Int objectGridSize;
        if (placingPrefab.GetComponent<Barn>() != null) {
            objectGridSize = new(6, 6);
        } else if (placingPrefab.GetComponent<Sheep>() != null) {
                objectGridSize = new(1, 1);
        } else {
            objectGridSize = new(2, 2);
        }
        Vector2Int rotationOffset = GetRotationOffset(direction, objectGridSize);
        Vector3 rotationWorldPosition = new Vector3(rotationOffset.x, 0, rotationOffset.y) * gridPlacingOn.GetCellSize();
        Vector3 tileWorldPosition = gridPlacingOn.GetWorldPosition(placingGridPosition);
        Vector3 placedFurnitureWorldPosition = tileWorldPosition + rotationWorldPosition;

        PlacedObjOnGrid placedObject = PlacedObjOnGrid.Create(placedFurnitureWorldPosition, direction, placingPrefab);
        List<Vector2Int> gridPositionList = GetObjectGridPositions(placingGridPosition, direction, objectGridSize);

        foreach (Vector2Int gridPosition in gridPositionList) {
            gridPlacingOn.GetGridObject(gridPosition).SetPlacedObject(placedObject);
        }

        return placedObject;
    }

    // Returns if object would be placed sucessfully
    public bool IsValidPositionToPlaceOnGrid(GridXZ<GridTile> gridPlacingOn, Vector2Int placingGridPosition, Transform placingPrefab) {
        if (placingGridPosition.x >= BORDER_THRESHOLD && placingGridPosition.y >= BORDER_THRESHOLD && placingGridPosition.x < gridPlacingOn.GetGridSize().x - BORDER_THRESHOLD && placingGridPosition.y < gridPlacingOn.GetGridSize().y - BORDER_THRESHOLD) {
            Vector2Int objectGridSize;
            if (placingPrefab.GetComponent<Barn>() != null) {
                objectGridSize = new(6, 6);
            } else if (placingPrefab.GetComponent<Sheep>() != null) {
                objectGridSize = new(1, 1);
            } else {
                objectGridSize = new(2, 2);
            }
            List<Vector2Int> gridPositionList = GetObjectGridPositions(placingGridPosition, direction, objectGridSize);

            foreach (Vector2Int gridPosition in gridPositionList) {
                if (gridPlacingOn.GetGridObject(gridPosition) == null) {
                    // Debug.LogWarning("Object would be partly outside grid!");
                    return false;

                } else if (gridPlacingOn.GetGridObject(gridPosition).HasObject() && objectGridSize != new Vector2Int(1, 1)) {
                    // Debug.LogWarning("Object would be partly inside another object!");
                    return false;
                } else if (gridPosition == gridPlacingOn.GetCenterGridPosition()) {
                    // Don't place on top of the player's starting position
                    return false;
                } else if (objectGridSize == new Vector2Int(1, 1) && gridPlacingOn.GetGridObject(gridPosition).HasObject()) {
                    if (gridPlacingOn.GetGridObject(gridPosition).GetPlacedObject().GetComponent<Barn>() != null) {
                        return false;
                    }
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