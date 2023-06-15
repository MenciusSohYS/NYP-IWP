using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject> {

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private int Width; //width of place
    private int Height; //height of place
    private float CellSize; //size of a singular box
    private Vector3 OriginalPosition; //start pos
    private TGridObject[,] GridArray; //array to reference

    public Grid(int Width, int Height, float CellSize, int OriginPointX, int OriginPointY, Vector3 OriginalPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) 
    {
        this.Width = Width;
        this.Height = Height;
        this.CellSize = CellSize;
        this.OriginalPosition = OriginalPosition;

        GridArray = new TGridObject[Width, Height];

        for (int x = 0; x < GridArray.GetLength(0); ++x)
        {
            for (int y = 0; y < GridArray.GetLength(1); ++y) 
            {
                GridArray[x, y] = createGridObject(this, x, y); //draw array
            }
        }

        bool SeeDebug = true;
        if (SeeDebug)
        {
            for (int x = 0; x < GridArray.GetLength(0); ++x)
            {
                for (int y = 0; y < GridArray.GetLength(1); ++y)
                {
                    Debug.DrawLine(GetWorldPosition(x - 15, y - 15), GetWorldPosition(x - 15, y + 1 - 15), Color.red, 100f);
                    Debug.DrawLine(GetWorldPosition(x - 15, y - 15), GetWorldPosition(x + 1 - 15, y - 15), Color.red, 100f);
                }
            }
        }
    }

    public int GetWidth() {
        return Width;
    }

    public int GetHeight() {
        return Height;
    }

    public float GetCellSize() {
        return CellSize;
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * CellSize + OriginalPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y) {
        //Debug.Log(worldPosition + "  " + OriginalPosition);
        x = Mathf.FloorToInt((worldPosition - OriginalPosition).x / CellSize);
        y = Mathf.FloorToInt((worldPosition - OriginalPosition).y / CellSize);
    }

    public void SetGridObject(int x, int y, TGridObject value) {
        if (x >= 0 && y >= 0 && x < Width && y < Height) 
        {
            GridArray[x, y] = value;
            if (OnGridObjectChanged != null)
                OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void TriggerGridObjectChanged(int x, int y) {
        if (OnGridObjectChanged != null)
        {
            OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < Width && y < Height) 
        {
            return GridArray[x, y];
        } 
        else 
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public void DrawUnWalkable(int x, int y, bool IsNowWalkable) //will draw a line from the unwalkable to its right and above it
    {
        if (!IsNowWalkable)
        {
            Debug.DrawLine(GetWorldPosition(x - 15, y - 15), GetWorldPosition(x - 15, y + 1 - 15), Color.green, 100f);
            Debug.DrawLine(GetWorldPosition(x - 15, y - 15), GetWorldPosition(x + 1 - 15, y - 15), Color.green, 100f);
        }
        else
        {
            Debug.DrawLine(GetWorldPosition(x - 15, y - 15), GetWorldPosition(x - 15, y + 1 - 15), Color.red, 100f);
            Debug.DrawLine(GetWorldPosition(x - 15, y - 15), GetWorldPosition(x + 1 - 15, y - 15), Color.red, 100f);
        }
    }
}