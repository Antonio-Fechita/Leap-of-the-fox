using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public int currentHealth, maxHealth;
    public static PlayerHealthController instance;
    public Animator animator;
    [SerializeField] AnimationClip damageAnimation;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }


    public void takeDamage(int damageAmount, float xVelocity = -5, float yVelocity = 15)
    {
        PlayerController.instance.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity);
        if(PlayerController.instance.isTakingDamage == false)
        {
            currentHealth = Mathf.Max(0, currentHealth - damageAmount);
            PlayerController.instance.isTakingDamage = true;
            animator.SetBool("takenDamage", true);
            
            if (currentHealth == 0)
            {
                StartCoroutine(callRespawnPlayer());
            }
            else
            {
                StartCoroutine(recentlyNotTakenDamage()); //start timer while player is in damage animation and cannot take additional damage
            }
        }
    }

    IEnumerator callRespawnPlayer()
    {
        yield return new WaitForSeconds(damageAnimation.length * 0.75f); //wait less than the damage animation time so you don't see the player standing on his feet again before respawning
        Respawn.instance.RespawnPlayer();
    }

    IEnumerator recentlyNotTakenDamage()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerController.instance.isTakingDamage = false;
        animator.SetBool("takenDamage", false);
    }
}
