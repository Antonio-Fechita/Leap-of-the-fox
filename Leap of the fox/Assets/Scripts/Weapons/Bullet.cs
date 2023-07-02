using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool hitSomething = false;
    private Animator animator;
    public Coroutine coroutineReturnBullet;

    private void Start()
    {
        transform.localPosition = new Vector3(0.200000003f, 0.15625f, 1);
        animator = GetComponent<Animator>();
    }

    public void ReturnBulletToGun()
    {
        if (coroutineReturnBullet != null)
            StopCoroutine(coroutineReturnBullet);
        transform.parent = PlayerController.instance.GetComponentInChildren<SingleShotPistol>().gameObject.transform; //sets bullet parent to pistol
        transform.localPosition = new Vector3(0.200000003f, 0.15625f, 1); //positions bullet in pistol
        Destroy(GetComponent<Rigidbody2D>());
        GetComponent<BoxCollider2D>().isTrigger = true;
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector2(1, 1);
        hitSomething = false;
        PlayerController.instance.GetComponentInChildren<SingleShotPistol>().loaded = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && hitSomething)
        {
            ReturnBulletToGun();
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hello");
            EnemyHealthController enemyHealthController = collision.gameObject.GetComponent<EnemyHealthController>();
            enemyHealthController.healthPoints -= 50;
            if (enemyHealthController.healthPoints <= 0)
                Destroy(collision.gameObject);
        }

        hitSomething = true;
        animator.Play("static");
        GetComponent<Animator>().Play("static");
    }
}
