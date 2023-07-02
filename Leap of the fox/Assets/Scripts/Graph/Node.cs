using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node : MonoBehaviour
{
    public List<Node> neighbors = new List<Node>();
    [SerializeField] public float DistanceFromTarget { set; get; }
    [SerializeField] public float Cost { set; get; }
    [SerializeField] public float totalAproximatedCost => Cost + DistanceFromTarget;
    public Node parent { get; set; }
    public bool doneAddingNeighbors = false;
    public void AddNeighbours()
    {
        parent = null;
        neighbors = GameObject.FindGameObjectsWithTag("Web").Select(obj => obj.GetComponent<Node>()).Where(node => node != null && Vector2.Distance(node.transform.position,
            transform.position) < 1.45 && Vector2.Distance(node.transform.position, transform.position) > 0.1).ToList();
        doneAddingNeighbors = true;

    }

    public void ResetNode()
    {
        parent = null;
        Cost = 1000000;
    }

    public void SetDistanceFromTarget(Node target)
    {
        DistanceFromTarget = Vector2.Distance(transform.position, target.transform.position);
    }
}
