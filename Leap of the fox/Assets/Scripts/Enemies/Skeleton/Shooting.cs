using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject originalArrow;
    GameObject clonedArrow;
    public Transform firstFrameArrow;
    public Transform secondFrameArrow;
    public Transform thirdFrameArrow;

    public Sprite firstFrameSprite;
    public Sprite secondFrameSprite;
    public Sprite thirdFrameSprite;

    public float arrowVelocityX;
    public float arrowGravityScale;
    public int arrowDamage;
    public float secondsUntilArrowDespawn;

    public static Shooting instance;

    private SkeletonController skeletonController;
    private AudioSource audioSource;

    [SerializeField] AudioClip pullBow;
    [SerializeField] AudioClip arrowInAir;
    [SerializeField] AudioClip arrowHit;

    private void Awake()
    {
        instance = this;
    }

    SpriteRenderer spriteRenderer;

    int currentFrame = 0; // 0 -> first frame, 1 -> second frame, 2 -> third frame

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        clonedArrow = Instantiate(originalArrow, gameObject.transform);
        clonedArrow.transform.position = firstFrameArrow.position;
        clonedArrow.transform.rotation = firstFrameArrow.rotation;
        clonedArrow.transform.localScale = firstFrameArrow.localScale;

        skeletonController = GetComponent<SkeletonController>();
    }

    void Update()
    {
        if (spriteRenderer.sprite == firstFrameSprite && currentFrame != 0)
        {
            currentFrame = 0;
            audioSource.clip = pullBow;
            audioSource.Play();

            clonedArrow.AddComponent<Rigidbody2D>();
            clonedArrow.GetComponent<Rigidbody2D>().gravityScale = arrowGravityScale;
            clonedArrow.AddComponent<BoxCollider2D>();
            clonedArrow.AddComponent<Arrow>();
            AudioSource clonedArrowAudioSource = clonedArrow.AddComponent<AudioSource>();
            clonedArrow.GetComponent<Arrow>().hitClip = arrowHit;
            clonedArrow.GetComponent<Rigidbody2D>().velocity = new Vector2(arrowVelocityX * transform.localScale.x, 0);
            clonedArrowAudioSource.clip = arrowInAir;
            clonedArrowAudioSource.Play();
            
            clonedArrow.transform.parent = null;
            StartCoroutine(shootingCooldown());

            clonedArrow = Instantiate(originalArrow, gameObject.transform);
            clonedArrow.transform.position = firstFrameArrow.position;
            clonedArrow.transform.rotation = firstFrameArrow.rotation;
            clonedArrow.transform.localScale = firstFrameArrow.localScale;
        }
        else if (spriteRenderer.sprite == secondFrameSprite && currentFrame != 1)
        {
            currentFrame = 1;
            clonedArrow.transform.position = secondFrameArrow.position;
            clonedArrow.transform.rotation = secondFrameArrow.rotation;
            clonedArrow.transform.localScale = secondFrameArrow.localScale;
        }
        else if (spriteRenderer.sprite == thirdFrameSprite && currentFrame != 2)
        {
            currentFrame = 2;
            clonedArrow.transform.position = thirdFrameArrow.position;
            clonedArrow.transform.rotation = thirdFrameArrow.rotation;
            clonedArrow.transform.localScale = thirdFrameArrow.localScale;
        }
    }


    IEnumerator shootingCooldown()
    {
        skeletonController.inCooldown = true;
        yield return new WaitForSeconds(skeletonController.cooldown);
        skeletonController.inCooldown = false;
    }
}
