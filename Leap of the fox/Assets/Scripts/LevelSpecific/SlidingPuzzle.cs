using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPuzzle : MonoBehaviour
{
    private Transform[,] currentState;
    private Transform[] pieces = new Transform[15];
    private Vector2 offset;
    private Vector2Int emptySpacePosition; //x -> row, y -> column
    private float distance;
    private bool moving = false;
    private bool solved = false;
    [SerializeField] GameObject hiddenKey;
    //[SerializeField] Transform missingPiece;
    [SerializeField] float speed = 20f;
    [SerializeField] Lever upLever, downLever, leftLever, rightLever;
    [SerializeField] GameObject unlockableZone;

    private int[,] scramble = new int[4, 4] { { 1, 3, -1, 4  }, { 5, 2, 6, 8 }, { 9, 14, 7, 11 }, { 13, 15, 10, 12 } };
    //private int[,] scramble = new int[4, 4] { { 1, 2, 3, 4}, { 5, 6, 7, 8}, { 9, 10, 11, -1 }, { 13, 14, 15, 12 } };

    void Start()
    {
        upLever.upEvent.AddListener(UpMove);
        upLever.downEvent.AddListener(UpMove);

        downLever.upEvent.AddListener(DownMove);
        downLever.downEvent.AddListener(DownMove);

        rightLever.upEvent.AddListener(RightMove);
        rightLever.downEvent.AddListener(RightMove);

        leftLever.upEvent.AddListener(LeftMove);
        leftLever.downEvent.AddListener(LeftMove);


        currentState = new Transform[4, 4];
        for (int _ = 0; _ < 15; _++)
        {
            pieces[_] = transform.GetChild(_);
        }
        currentState[2, 2] = pieces[scramble[2, 2]];
        offset = pieces[0].position;
        distance = Vector2.Distance(pieces[0].position, pieces[1].position);
        Scramble();

    }

    private void LeftMove()
    {
        if(ValidateMove(DirectionEnum.left) && !moving)
        {
            StartCoroutine(MakeMove(DirectionEnum.left));
        }
    }

    private void RightMove()
    {
        if (ValidateMove(DirectionEnum.right) && !moving)
        {
            StartCoroutine(MakeMove(DirectionEnum.right));
        }
    }

    private void UpMove()
    {
        if (ValidateMove(DirectionEnum.up) && !moving)
        {
            StartCoroutine(MakeMove(DirectionEnum.up));
        }
    }

    private void DownMove()
    {
        if (ValidateMove(DirectionEnum.down) && !moving)
        {
            StartCoroutine(MakeMove(DirectionEnum.down));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Missing Piece"))
        {
            StartCoroutine(CompleteWithMissingPiece(collision.gameObject));
        }
    }

    IEnumerator CompleteWithMissingPiece(GameObject missingPiece)
    {
        Destroy(missingPiece.GetComponent<Rigidbody2D>());
        Destroy(missingPiece.GetComponent<BoxCollider2D>());
        Vector2 destination = GetWorldPosition(3, 3);

        while (Vector2.Distance(missingPiece.transform.position, destination) > 0.05)
        {
            missingPiece.transform.position = Vector2.MoveTowards(missingPiece.transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }
        missingPiece.transform.position = destination;
        missingPiece.transform.parent = transform;

        yield return new WaitForSeconds(1);
        hiddenKey.SetActive(true);
        transform.gameObject.AddComponent<Rigidbody2D>();

    }


    private Vector2 GetWorldPosition(int row, int column)
    {
        return new Vector2(offset.x + distance * column, offset.y - distance * row);
    }

    private bool ValidateMove(DirectionEnum direction)
    {
        Debug.Log("Validating move");
        switch (direction)
        {
            case DirectionEnum.down:
                Debug.Log(moving);
                return emptySpacePosition.x != 0;
            case DirectionEnum.up:
                return emptySpacePosition.x != 3;
            case DirectionEnum.left:
                return emptySpacePosition.y != 3;
            default: //means right
                return emptySpacePosition.y != 0;
        }
    }

    private void OnSolvedPuzzle()
    {
        unlockableZone.SetActive(true);
    }

    IEnumerator MakeMove(DirectionEnum direction) //direction from which a piece is moving
    {
        if (!solved)
        {
            Debug.Log("Making move");
            Vector2Int nextEmptySpacePosition;
            moving = true;
            Transform tileToMove;
            Debug.Log("Before switch");
            switch (direction)
            {
                case DirectionEnum.down:
                    nextEmptySpacePosition = new Vector2Int(emptySpacePosition.x - 1, emptySpacePosition.y);
                    Debug.Log("Down?");
                    break;
                case DirectionEnum.up:
                    nextEmptySpacePosition = new Vector2Int(emptySpacePosition.x + 1, emptySpacePosition.y);
                    break;
                case DirectionEnum.left:
                    nextEmptySpacePosition = new Vector2Int(emptySpacePosition.x, emptySpacePosition.y + 1);
                    break;
                default: //means right
                    nextEmptySpacePosition = new Vector2Int(emptySpacePosition.x, emptySpacePosition.y - 1);
                    break;
            }

            Debug.Log("After switch");
            tileToMove = currentState[nextEmptySpacePosition.x, nextEmptySpacePosition.y];


            Debug.Log("Tile to move: " + tileToMove.name);
            Vector2 destination = GetWorldPosition(emptySpacePosition.x, emptySpacePosition.y);

            while (Vector2.Distance(tileToMove.position, destination) > 0.05)
            {
                tileToMove.position = Vector2.MoveTowards(tileToMove.position, destination, speed * Time.deltaTime);
                yield return null;
            }
            tileToMove.position = destination;

            currentState[emptySpacePosition.x, emptySpacePosition.y] = tileToMove;
            currentState[nextEmptySpacePosition.x, nextEmptySpacePosition.y] = null;
            emptySpacePosition = new Vector2Int(nextEmptySpacePosition.x, nextEmptySpacePosition.y);

            if (CheckSolution())
            {
                solved = true;
                OnSolvedPuzzle();
            }

            moving = false;
        }

    }

    private bool CheckSolution()
    {
        for(int row = 0; row < 4; row++)
        {
            for(int column = 0; column < 4; column++)
            {
                if (currentState[row, column] != null && currentState[row, column].name != (row * 4 + column + 1).ToString())
                {
/*                    Debug.Log("Row: " + row);
                    Debug.Log("Column: " + column);
                    Debug.Log("Result: " + (row * 0 + column + 1).ToString());
                    Debug.Log("Name: " + currentState[row, column].name);*/
                    return false;
                }
                    
            }
        }

        Debug.Log("Good solution");
        return true;
    }

    private void Scramble()
    {
        bool ok = false;
        for (int row = 0; row < 4; row++)
        {
            for (int column = 0; column < 4; column++)
            {
                if (scramble[row, column] != -1)
                {
                    currentState[row, column] = pieces[scramble[row, column] - 1];
                    currentState[row, column].position = GetWorldPosition(row, column);



                }
                else
                {
                    currentState[row, column] = null;
                    emptySpacePosition = new Vector2Int(row, column);
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
