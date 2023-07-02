using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotPistol : MonoBehaviour
{
    public bool loaded = true;
    [SerializeField] float speed = 20;
    private GameObject bullet;
    private Rigidbody2D bulletRigidBody;
    public SpriteRenderer spriteRenderer;
    [SerializeField] Sprite emptyGunSprite;
    [SerializeField] Sprite loadedGunSprite;

    private void Start()
    {
        bullet = transform.GetChild(0).gameObject;
    }


    private void Update()
    {

        if(loaded && spriteRenderer.sprite == emptyGunSprite)
        {
            spriteRenderer.sprite = loadedGunSprite;
        }
        else if(!loaded && spriteRenderer.sprite == loadedGunSprite)
        {
            spriteRenderer.sprite = emptyGunSprite;
        }

        if (Input.GetMouseButtonDown(0) && loaded)
        {
            Debug.Log("Shot");
            bullet.GetComponent<BoxCollider2D>().isTrigger = false;
            loaded = false;
            bullet.transform.parent = null;
            bullet.AddComponent<Rigidbody2D>();
            bullet.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * Mathf.Sign(PlayerController.instance.transform.localScale.x), 0);
            bullet.GetComponent<Animator>().Play("fired");
            Coroutine returnBulletCoroutine = StartCoroutine(ReturnBulletCountdown());
            bullet.GetComponent<Bullet>().coroutineReturnBullet = returnBulletCoroutine;
        }
    }

    IEnumerator ReturnBulletCountdown()
    {
        yield return new WaitForSecondsRealtime(20);
        bullet.GetComponent<Bullet>().ReturnBulletToGun();
    }
}
