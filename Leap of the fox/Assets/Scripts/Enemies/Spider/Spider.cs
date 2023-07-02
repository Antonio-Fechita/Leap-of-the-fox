using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] Graph graph;
    [SerializeField] float cooldownBetweenWalks = 3;
    [SerializeField] float speed = 2f;
    [SerializeField] int rotationModifier;
    [SerializeField] float rotationSpeed = 5f;
    private KeepTrackOfPlayer keepTrackOfPlayer;
    private Node startingNode;
    private Node endingNode;
    private bool inWalkingCoroutine = false;
    private bool rotating = false;
    private Vector2 targetPosition = new Vector2(0, 0);
    A_star a_Star;
    private Animator legsAnimator;
    private AudioSource audioSource;
    public bool isActive = true;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Pause();
        legsAnimator = GetComponentInChildren<Animator>();
        keepTrackOfPlayer = GetComponent<KeepTrackOfPlayer>();

        a_Star = new A_star(graph);
        startingNode = graph.nodes[Random.Range(0, graph.nodes.Count - 1)];
        endingNode = graph.nodes[Random.Range(0, graph.nodes.Count - 1)];


        while (startingNode.transform.position == endingNode.transform.position)
        {
            startingNode = graph.nodes[Random.Range(0, graph.nodes.Count - 1)];
            endingNode = graph.nodes[Random.Range(0, graph.nodes.Count - 1)];
        }

        transform.position = startingNode.transform.position;
        List<Node> path = a_Star.Run(startingNode, endingNode);
        inWalkingCoroutine = true;
        StartCoroutine(Walk(path));

    }


    // Update is called once per frame
    void Update()
    {

        if (isActive)
        {
            if (keepTrackOfPlayer.seeingPlayer)
            {
                targetPosition = PlayerController.instance.transform.position;
            }

            if (!inWalkingCoroutine)
            {
                startingNode = endingNode;
                endingNode = graph.nodes[Random.Range(0, graph.nodes.Count - 1)];

                if (startingNode.transform.position != endingNode.transform.position)
                {
                    List<Node> path = a_Star.Run(startingNode, endingNode);
                    inWalkingCoroutine = true;

                    StartCoroutine(Walk(path));
                }

            }
        }


    }

    public void FixedUpdate()
    {
        Vector2 vectorToTarget = targetPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
    }

    IEnumerator Walk(List<Node> path)
    {
        legsAnimator.speed = 1;
        audioSource.UnPause();
        for (int index = 0; index < path.Count - 1; index++)
        {
            if (!isActive)
                break;
            if(!keepTrackOfPlayer.seeingPlayer)
                targetPosition = path[index + 1].transform.position;
            while (Vector2.Distance(transform.position, path[index + 1].transform.position) > 0.05)
            {
                if (!isActive)
                    break;
                transform.position = Vector2.MoveTowards(transform.position, path[index + 1].transform.position, speed * Time.deltaTime);
                yield return null;
            }
        }

        legsAnimator.speed = 0;
        audioSource.Pause();
        yield return new WaitForSeconds(cooldownBetweenWalks);
        inWalkingCoroutine = false;
    }

}

