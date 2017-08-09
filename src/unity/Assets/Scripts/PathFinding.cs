using System.Collections.Generic;
using UnityEngine;

public class PathFinding : Singleton<PathFinding> {

    public class AStarNode
    {
        public AStarNode(Cell c = null)
        {
            this.c = c;
        }
        public Cell c;
        public int gCost;
        public int hCost;
        public int fCost;
        public AStarNode parent;

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            AStarNode other = obj as AStarNode;
            if ((System.Object)other == null)
            {
                return false;
            }

            return this.c.Equals(other.c);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode()*gCost+c.name.GetHashCode();
        }
    }

    public AStarNode startNode;
    public AStarNode endNode;
    List<AStarNode> open = new List<AStarNode>();
    List<AStarNode> closed = new List<AStarNode>();

    public List<AStarNode> AStar(Cell start, Cell end)
    {
        if (start == end)
        {
            return new List<AStarNode>();
        }
        open = new List<AStarNode>();
        closed = new List<AStarNode>();
        bool finished = false;
        startNode = new AStarNode(start);
        startNode.parent = startNode;
        endNode = new AStarNode(end);

        open.Add(startNode);

        while(!finished)
        {
            if (open.Count == 0)
            {
                //Debug.Log("No path found from " + start.name + " to " +end.name);
                return null;
            }
            AStarNode current = getNodeWithLowestF();
            open.Remove(current);
            closed.Add(current);

            if (current.Equals(endNode)) {
                endNode = current;
                finished = true;
                //Debug.Log("Path found from " + start.name + " to " + end.name);
                break;
            }

            List<AStarNode> neighbours = getPossibleNeighbours(current);

            foreach (AStarNode n in neighbours)
            {
                calculateCost(n);
                if (closed.Contains(n))
                {
                    continue;
                }
                if (!open.Contains(n))
                {
                    open.Add(n);
                }

                AStarNode n1 = open.Find(x => x.c.Equals(n.c));
                if (n1.fCost > n.fCost  ||
                    (n1.fCost == n.fCost && n1.hCost > n.hCost) ||
                    (n1.fCost == n.fCost && n1.hCost == n.hCost && n1.gCost > n.gCost))
                {
                    n1.fCost = n.fCost;
                    n1.gCost = n.gCost;
                    n1.hCost = n.hCost;
                    n1.parent = n.parent;
                    n1.c = n.c;
                }
            }
        }
        return gereatePath();
    }

    private List<AStarNode> gereatePath()
    {
        AStarNode currentNode = endNode;
        List<AStarNode> ASList = new List<AStarNode>();
        while (currentNode != startNode)
        {
            ASList.Insert(0, currentNode);
            currentNode = currentNode.parent;
        }
        return ASList;
    }

    private AStarNode getNodeWithLowestF()
    {
        int minF = int.MaxValue;
        AStarNode min = new AStarNode();
        foreach (AStarNode a in open)
        {
            calculateCost(a);

            if (a.fCost < minF)
            {
                minF = a.fCost;
                min = a;
            }
        }
        return min;
    }

    private void calculateCost(AStarNode node)
    {
        node.gCost = getManhattanDistance(node.c, startNode.c);
        node.hCost = getManhattanDistance(node.c, endNode.c);
        node.fCost = node.hCost + node.gCost;
    }

    public int getManhattanDistance(Cell c1, Cell c2)
    {
        int dX = c1.posX - c2.posX;
        dX = Mathf.Abs(dX);

        int dY = c1.posY - c2.posY;
        dY = Mathf.Abs(dY);

        return dX + dY;
    }

    public List<Cell> getPossibleNeighbours(Cell c)
    {
        GameObject[][] matrix = MazeGenerator.Instance.toMatrix();
        List<Cell> cells = new List<Cell>();

        if (c.properties.north.activeSelf == false)
        {
            cells.Add(matrix[c.posY + 1][c.posX].GetComponent<Cell>());
        }
        if (c.properties.west.activeSelf == false)
        {
            cells.Add(matrix[c.posY][c.posX -1 ].GetComponent<Cell>());
        }
        if (c.properties.south.activeSelf == false)
        {
            cells.Add(matrix[c.posY - 1][c.posX].GetComponent<Cell>());
        }
        if (c.properties.east.activeSelf == false)
        {
            cells.Add(matrix[c.posY][c.posX + 1].GetComponent<Cell>());
        }
        //Debug.Log("Possible neighbours for " + c.name + ": " + cells.Count);
        return cells;
    }

    public List<AStarNode> getPossibleNeighbours(AStarNode node)
    {
        List<AStarNode> nodes = new List<AStarNode>();
        List<Cell> cells = getPossibleNeighbours(node.c);
        foreach (Cell c in cells)
        {
            AStarNode n = new AStarNode(c);
            n.parent = node;
            nodes.Add(n);
            calculateCost(n);
        }
        return nodes;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
