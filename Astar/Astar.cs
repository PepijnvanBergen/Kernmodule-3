using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path
    /// Note that you will probably need to add some helper functions
    /// from the startPos to the endPos
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    Node[,] nodeGrid;
    int gridLengthX;
    int gridLengthY;

    private bool nodeGridCreated = false;
    private void CreateNodeGrid(Vector2Int endPos, Cell[,] grid)
    {
        List<Node> Nodes = new List<Node>();

        gridLengthX = grid.Length;
        gridLengthY = grid.Length;

        nodeGrid = new Node[gridLengthX, gridLengthY];

        for (int x = 0; x < gridLengthY; x++)
        {
            for (int y = 0; y < gridLengthY; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Node node = new Node
                {
                    position = pos,
                    GScore = int.MaxValue,
                    HScore = GetDistance(pos, endPos),
                    parent = null
                };
                nodeGrid[x, y] = node;
            }
        }
        nodeGridCreated = true;
    }

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        if (!nodeGridCreated) CreateNodeGrid(endPos, grid);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node startNode = nodeGrid[startPos.x, startPos.y];
        Node targetNode = nodeGrid[endPos.x, endPos.y];

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FScore < currentNode.FScore || openList[i].FScore == currentNode.FScore && openList[i].HScore < currentNode.HScore)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                return ReturnPath(startNode.position, targetNode.position);
            }

            foreach (Node neighbour in GetNeighbours(currentNode.position, grid))
            {
                if (closedList.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.GScore + GetDistance(currentNode.position, neighbour.position);
                if (newMovementCostToNeighbour < neighbour.GScore || !openList.Contains(neighbour))
                {
                    neighbour.GScore = newMovementCostToNeighbour;
                    neighbour.parent = currentNode;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
        return null;
    }
    private List<Node> GetNeighbours(Vector2Int currentNode, Cell[,] grid)
    {
        List<Node> neighbours = new List<Node>();
        Cell cell = grid[currentNode.x, currentNode.y];

        if (!cell.HasWall(Wall.UP))
        {
            neighbours.Add(nodeGrid[currentNode.x, currentNode.y + 1]);
        }
        if (!cell.HasWall(Wall.DOWN))
        {
            neighbours.Add(nodeGrid[currentNode.x, currentNode.y - 1]);
        }
        if (!cell.HasWall(Wall.LEFT))
        {
            neighbours.Add(nodeGrid[currentNode.x - 1, currentNode.y]);
        }
        if (!cell.HasWall(Wall.RIGHT))
        {
            neighbours.Add(nodeGrid[currentNode.x + 1, currentNode.y]);
        }
        return neighbours;
    }

    List<Vector2Int> ReturnPath(Vector2Int startNode, Vector2Int endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = nodeGrid[endNode.x, endNode.y];

        while (currentNode != nodeGrid[startNode.x, startNode.y])
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }


    private int GetDistance(Vector2Int firstNode, Vector2Int secondNode)
    {
        int distanceX = Mathf.Abs(firstNode.x - secondNode.x);
        int distanceY = Mathf.Abs(firstNode.y - secondNode.y);
        if (distanceX > distanceY)
        {
            return (14 * distanceY) + (10 * (distanceX - distanceY));
        }
        else
        {
            return (14 * distanceX) + (10 * (distanceY - distanceX));
        }
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is.
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public int FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public int GScore; //Current Travelled Distance
        public int HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
