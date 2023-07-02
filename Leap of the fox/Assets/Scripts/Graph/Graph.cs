using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Graph : MonoBehaviour
{
    public List<Node> nodes = new List<Node>();
    [SerializeField] float maxDistance = 15;
    void Start()
    {
        nodes = GameObject.FindGameObjectsWithTag("Web").Select(obj => obj.GetComponent<Node>()).Where(node => node != null && Vector2.Distance(transform.position,
            node.transform.position) < maxDistance).ToList();

        //Debug.Log("My graph has " + nodes.Count + " nodes");
    }

}
