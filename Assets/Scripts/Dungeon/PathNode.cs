using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid<PathNode> Grid;
    public int x;
    public int y;

    public int gCost; //current cumulated cost
    public int hCost; //cost from current node to last node
    public int fCost; //G+H

    public PathNode PreviousNode;

    public PathNode(Grid<PathNode> Grid, int x, int y)
    {
        this.Grid = Grid;
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
