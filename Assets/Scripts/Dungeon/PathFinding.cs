using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private const int COST_TO_MOVE_STRAIGHT = 10;
    private const int COST_TO_MOVE_DIAGONALLY = 14;
    //cost to move the object (using pythagoras theorem)

    public static PathFinding Instance { get; private set; }
    private Grid<PathNode> Grid;
    private List<PathNode> OpenList;
    private List<PathNode> ClosedList;
    private Vector3 OriginLocation;

    public PathFinding(int width, int height, int OriginPointX, int OriginPointY, Vector3 Where)
    {
        OriginLocation = Where;
        Instance = this;
        Grid = new Grid<PathNode>(width, height, 1f, OriginPointX, OriginPointY, Where, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y)); //create the grid as well as nodes
    }
    public List<Vector3> FindPath(Vector3 StartPos, Vector3 EndPos) //translate node to list of positions
    {
        Grid.GetXY(StartPos, out int StartX, out int StartY); //find start location
        Grid.GetXY(EndPos, out int EndX, out int EndY); //find end location

        //Debug.Log("Moving from: X: " + StartX + ", Y: " + StartY + "\tTo X: " + EndX + ", Y: " + EndY);
        
        List<PathNode> path = FindPath(StartX + 15, StartY + 15, EndX, EndY);
        //Debug.Log(path);
        if (path == null) //has an error
        {
            Debug.Log("PATH IS EMPTY");
            return null;
        }
        else //convert everything
        {
            //Debug.Log("CONVERTING");
            List<Vector3> PathOfVectors = new List<Vector3>();
            foreach (PathNode PNode in path)
            {
                PathOfVectors.Add((new Vector3(PNode.x - 15, PNode.y - 15) * Grid.GetCellSize()) + OriginLocation);
            }
            return PathOfVectors;
        }
    }
    public List<PathNode> FindPath(int StartX, int StartY, int EndX, int EndY)
    {
        PathNode StartNode = Grid.GetGridObject(StartX, StartY); //find start location
        PathNode EndNode = Grid.GetGridObject(EndX + 15, EndY + 15); //find end location

        //Debug.Log("Moving from: X: " + StartX + ", Y: " + StartY + "\tTo X: " + (EndX + 15) + ", Y: " + (EndY + 15));
        //Debug.Log("Moving from: X: " + StartNode.x + ", Y: " + StartNode.y + "\tTo X: " + EndNode.x + ", Y: " + EndNode.y);

        if (StartNode == null || EndNode == null)
        {
            //Debug.Log("Start: " +StartNode);
            //Debug.Log("End: " + EndNode);
            // Invalid Path
            return null;
        }

        OpenList = new List<PathNode> { StartNode };
        ClosedList = new List<PathNode>();

        for (int x = 0; x < Grid.GetWidth(); ++x)
        {
            for (int y = 0; y < Grid.GetHeight(); ++y)
            {
                PathNode ActualPathNode = Grid.GetGridObject(x, y);
                ActualPathNode.gCost = int.MaxValue;
                ActualPathNode.CalculateFCost();
                ActualPathNode.PreviousNode = null;
            }
        }
        //reset the entire thing

        //start node calculation
        StartNode.gCost = 0;
        StartNode.hCost = CalculateDistanceCost(StartNode, EndNode);
        StartNode.CalculateFCost();

        while(OpenList.Count > 0)
        {
            PathNode CurrentNode = GetLowestFCostNode(OpenList);
            if (CurrentNode == EndNode)
                return CalculatePath(EndNode); //reached end, can go on


            //remove current from openlist as we have already searched it
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            //search neighbournode
            foreach(PathNode NeighbourNode in GetNeighbourList(CurrentNode))
            {
                //use list function to see if closedlist has neighbournode
                if (ClosedList.Contains(NeighbourNode))
                {
                    continue;
                }
                if (NeighbourNode != null)
                {
                    if (!NeighbourNode.IsWalkable)
                    {
                        Debug.Log("Not walkable");
                        ClosedList.Add(NeighbourNode);
                        continue;
                    }
                }

                int TempGCost = CurrentNode.gCost + CalculateDistanceCost(CurrentNode, NeighbourNode);//find out if the new gcost is cheaper than neighbour
                if (TempGCost < NeighbourNode.gCost)
                {
                    NeighbourNode.PreviousNode = CurrentNode;

                    //find new neighbour and see if its shorter
                    NeighbourNode.gCost = TempGCost;
                    NeighbourNode.hCost = CalculateDistanceCost(NeighbourNode, EndNode);
                    NeighbourNode.CalculateFCost();

                    if (!OpenList.Contains(NeighbourNode))
                    {
                        OpenList.Add(NeighbourNode);
                    }
                }
            }
        }

        //out of the nodes on the openlist, searched through whole map
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode CurrentNode) //check everything around it
    {
        List<PathNode> neightbourlist = new List<PathNode>();

        if (CurrentNode.x - 1 >= 0)
        {
            //Search left
            neightbourlist.Add(GetNode(CurrentNode.x - 1, CurrentNode.y));
            //down and then up
            if (CurrentNode.y - 1 >= 0)
                neightbourlist.Add(GetNode(CurrentNode.x - 1, CurrentNode.y - 1));
            if (CurrentNode.y + 1 >= 0)
                neightbourlist.Add(GetNode(CurrentNode.x - 1, CurrentNode.y + 1));
        }
        if (CurrentNode.x + 1 < Grid.GetWidth())
        {
            //Search right
            neightbourlist.Add(GetNode(CurrentNode.x + 1, CurrentNode.y));
            //down and then up
            if (CurrentNode.y - 1 >= 0)
                neightbourlist.Add(GetNode(CurrentNode.x + 1, CurrentNode.y - 1));
            if (CurrentNode.y + 1 >= 0)
                neightbourlist.Add(GetNode(CurrentNode.x + 1, CurrentNode.y + 1));
        }

        //down
        if (CurrentNode.y - 1 >= 0)
        {
            neightbourlist.Add(GetNode(CurrentNode.x, CurrentNode.y - 1));
        }
        //up
        if (CurrentNode.y + 1 >= 0)
        {
            neightbourlist.Add(GetNode(CurrentNode.x, CurrentNode.y + 1));
        }

        return neightbourlist;
    }

    public PathNode GetNode(int x, int y)
    {
        return Grid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatePath (PathNode EndNode)
    {
        List<PathNode> PastPath = new List<PathNode>();
        EndNode.SetIsWalkable(false);
        PastPath.Add(EndNode); //start from the end and work our way back
        PathNode CurrenNode = EndNode;
        while (CurrenNode.PreviousNode != null) //so while it still has a parent
        {
            PastPath.Add(CurrenNode.PreviousNode); //add the where it was in the past to the list
            CurrenNode = CurrenNode.PreviousNode; //keep moving back up the list until we find what we want
        }
        PastPath.Reverse();
        return PastPath;
    }


    private int CalculateDistanceCost(PathNode PointA, PathNode PointB)
    {
        int xDistance = Mathf.Abs(PointA.x - PointB.x);
        int yDistance = Mathf.Abs(PointA.y - PointB.y);
        int Remaining = Mathf.Abs(xDistance - yDistance);
        return COST_TO_MOVE_DIAGONALLY * Mathf.Min(xDistance, yDistance) + COST_TO_MOVE_STRAIGHT * Remaining;
        //finding out the straight line distance to something
    }

    private PathNode GetLowestFCostNode(List<PathNode> PathNodeList)
    {
        PathNode LowestFCostNode = PathNodeList[0];
        for (int i = 1; i < PathNodeList.Count; ++i)
        {
            if (PathNodeList[i].fCost < LowestFCostNode.fCost)
            {
                LowestFCostNode = PathNodeList[i];
            }
        }
        return LowestFCostNode;

        //goes through array to find out who's the lowest
    }



    public Grid<PathNode> GetGrid()
    {
        return Grid;
    }

    public Vector3 ConvertWorldPos(Vector3 Position)
    {
        Grid.GetXY(Position, out int StartX, out int StartY);

        Vector3 ConvertedPosition = new Vector3(StartX + 15, StartY + 15);

        return ConvertedPosition;
    }
}
