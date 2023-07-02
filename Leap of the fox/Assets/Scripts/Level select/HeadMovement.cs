using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class HeadMovement : MonoBehaviour
{
    public Intersection currentIntersection;
    [SerializeField] float moveDuration;
    [SerializeField] TextMeshProUGUI score;
    private bool moving = false;
    LevelData levelData;

    private void Start()
    {
        levelData = SaveSystem.LoadLevels();
        Debug.Log(levelData);
        //Debug.Log("Lev data: " + levelData.bestScores[3]);
        Time.timeScale = 1f;
        
        float[] headPos = SaveSystem.LoadHeadPosition().headPosition;
        if(headPos != null)
        {
            transform.position = new Vector3(headPos[0], headPos[1], headPos[2]);
        }
        else
        {
            transform.position = currentIntersection.transform.position;
        }
        currentIntersection = Physics2D.OverlapCircle(transform.position, 0.1f).GetComponent<Intersection>();


        int build = currentIntersection.buildIndex;
        Debug.Log("Build index: " + build);
        if (build != -1)
        {
            score.text = levelData.bestScores[build].ToString();
            Debug.Log("Score:" + score.text);
        }
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Right") || ControllerInput.instance.GetButtonDown(DirectionEnum.right) && !moving)
        {
            if (currentIntersection.right != null)
            {
                currentIntersection = currentIntersection.right;
                StartCoroutine(move(currentIntersection.transform));
            }
        }
        else if (Input.GetButtonDown("Left") || ControllerInput.instance.GetButtonDown(DirectionEnum.left) && !moving)
        {
            if (currentIntersection.left != null)
            {
                currentIntersection = currentIntersection.left;
                StartCoroutine(move(currentIntersection.transform));
            }
        }

        else if (Input.GetButtonDown("Up") || ControllerInput.instance.GetButtonDown(DirectionEnum.up) && !moving)
        {
            if (currentIntersection.up != null)
            {
                currentIntersection = currentIntersection.up;
                StartCoroutine(move(currentIntersection.transform));
            }
        }
        else if (Input.GetButtonDown("Down") || ControllerInput.instance.GetButtonDown(DirectionEnum.down) && !moving)
        {
            if (currentIntersection.down != null)
            {
                currentIntersection = currentIntersection.down;
                StartCoroutine(move(currentIntersection.transform));
            }
        }
        else if (Input.GetButtonDown("Select") && !moving && currentIntersection.sceneName != null)
        {
            SaveSystem.SaveHeadPosition(new HeadPositionData(gameObject));
            SceneManager.LoadScene(currentIntersection.sceneName, LoadSceneMode.Single);
        }
    }

    IEnumerator move(Transform destination)
    {
        moving = true;
        float timeElapsed = 0;
        Vector2 initialPosition = transform.position;

        float movementRange = Vector2.Distance(initialPosition, destination.position);
        float duration = moveDuration * movementRange;
        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            transform.position = Vector2.Lerp(initialPosition, destination.position, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = destination.position;
        int build = currentIntersection.buildIndex;
        if (build != -1)
        {
            score.text = levelData.bestScores[build].ToString();
            Debug.Log("Score:" + score);
        }

        moving = false;
    }
}
