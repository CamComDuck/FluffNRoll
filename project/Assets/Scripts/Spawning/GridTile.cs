using UnityEngine;

public class GridTile {
        
    private GridXZ<GridTile> grid;
    private Vector2Int gridPosition;
    private PlacedObjOnGrid placedObject = null;

    public GridTile(GridXZ<GridTile> grid, Vector2Int gridPosition) {
        this.grid = grid;
        this.gridPosition = gridPosition;
    }

    public void SetPlacedObject(PlacedObjOnGrid placedObject) {
        this.placedObject = placedObject;
        grid.SetGridObject(gridPosition, this);
    }

    public PlacedObjOnGrid GetPlacedObject() {
        return this.placedObject;
    }

    public void ClearPlacedObject() {
        SetPlacedObject(null);
    }

    public bool HasObject() {
        return placedObject != null;
    }

    public override string ToString() {
        return "(" + gridPosition.x + ", " + gridPosition.y + "): " + this.placedObject?.GetObject().name;
    }

}
