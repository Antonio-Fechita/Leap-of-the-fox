using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Respawn : MonoBehaviour
{
    public Transform currentCheckPoint;
    private Transform player;
    public static Respawn instance;
    private Animator transitionAnimator;
    public Image circleImage;

    private float inTime;
    private float outTime;

    public static UnityEvent respawnEvent = new UnityEvent();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        transitionAnimator = circleImage.GetComponent<Animator>();
        player = PlayerController.instance.transform;
        circleImage.transform.SetAsLastSibling();

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

    public void RespawnPlayer()
    {
        
        StartCoroutine(respawnWithTransitions());
    }


    IEnumerator respawnWithTransitions()
    {
        transitionAnimator.SetTrigger("in");
        yield return new WaitForSeconds(inTime);
        respawnEvent.Invoke();
        player.transform.position = currentCheckPoint.position;
        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
        player.transform.GetComponent<Animator>().SetBool("takenDamage", false);
        PlayerController.instance.isTakingDamage = false;
        transitionAnimator.SetTrigger("out");
        transitionAnimator.ResetTrigger("in");
        yield return new WaitForSeconds(outTime);
        transitionAnimator.ResetTrigger("out");
    }
}
