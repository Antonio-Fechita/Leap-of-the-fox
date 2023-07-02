using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    [SerializeField] Animator transitionAnimator;
    float inTime;
    float outTime;
    ExitLevel instance;


    private void Start()
    {
        Collectables.gemsCollected = 0;

        AnimationClip[] clips = transitionAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "in":
                    inTime = clip.length;
                    break;
                case "out":
                    outTime = clip.length;
                    break;
            }
        }
    }

    public void CallGoToMap()
    {
        StartCoroutine(GoToMap());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            CallGoToMap();
    }
    bool sceneLoaded = false;
    IEnumerator GoToMap()
    {
        GameObject image = transitionAnimator.gameObject;

        transitionAnimator.SetTrigger("in");
        yield return new WaitForSecondsRealtime(inTime);
        Debug.Log("before scene swap");

        SceneManager.LoadScene("map select level", LoadSceneMode.Single);
    }

}
