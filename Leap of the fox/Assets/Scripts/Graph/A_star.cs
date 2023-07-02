using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class A_star
{
    private Graph graph;


    public A_star(Graph graph)
    {
        this.graph = graph;
    }


    public List<Node> Run(Node from, Node to)
    {
        graph.nodes.ForEach(node => node.AddNeighbours());

        graph.nodes.ForEach(node => node.ResetNode());
        from.SetDistanceFromTarget(to);
        from.Cost = 0;
        List<Node> activeNodes = new List<Node>();
        activeNodes.Add(from);
        List<Node> visitedNodes = new List<Node>();

        while (activeNodes.Any())
        {
            Node checkNode = activeNodes.OrderBy(node => node.totalAproximatedCost).First();

            if(checkNode.transform.position == to.transform.position)
            {
                return ReconstructPath(from, to);
            }

            visitedNodes.Add(checkNode);
            activeNodes.Remove(checkNode);


            foreach(Node neighbour in checkNode.neighbors)
            {
                if (visitedNodes.Any(node => node.transform.position == neighbour.transform.position)) //node is in closed list
                    continue;

                if(checkNode.Cost + Vector2.Distance(checkNode.transform.position, neighbour.transform.position) < neighbour.Cost
                    || !activeNodes.Any(node => node.transform.position == neighbour.transform.position))
                {
                    neighbour.Cost = checkNode.Cost + Vector2.Distance(checkNode.transform.position, neighbour.transform.position);
                    neighbour.SetDistanceFromTarget(to);
                    neighbour.parent = checkNode;
                    if(!activeNodes.Any(node => node.transform.position == neighbour.transform.position)) //neighbour not in open
                    {
                        activeNodes.Add(neighbour);
                    }
                }
            }
        }
        return new List<Node>();
    }


/*    public List<Node> Run2(Node from, Node to)
    {
        graph.nodes.ForEach(node => node.AddNeighbours());
        Queue<Node> queue = new Queue<Node>();

        queue.Enqueue(from);
        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();
            List<Node> neighbours = current.neighbors;
            foreach(Node neighbor in neighbours)
            {
                if(neighbor.parent == null)
                    neighbor.parent = current;
                if(neighbor.transform.position == to.transform.position)
                {
                    return ReconstructPath(from, to);
                }
                queue.Enqueue(neighbor);
            }
        }
        Debug.Log("Path not found");
        return null;
    }*/

    private List<Node> ReconstructPath(Node from, Node to)
    {
        Node currentNode = to;
        List<Node> reconstructedPath = new List<Node>();
        if(currentNode.parent == null)
        {
            Debug.Log("PARENT NULL");
        }
        while(currentNode.transform.position != from.transform.position)
        {
            reconstructedPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        reconstructedPath.Reverse();
        return reconstructedPath;
    }

}
