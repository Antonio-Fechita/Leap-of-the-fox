using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBody : MonoBehaviour
{
    [SerializeField] int numberOfPieces;
    [SerializeField] Sprite bodyPartSprite;
    [SerializeField] Sprite tailPartSprite;
    [SerializeField] Sprite headSprite;
    [SerializeField] RuntimeAnimatorController headAnimator;
    [SerializeField] int damageValue;
    private float initialRotation;
    private float timeElapsed = 0;
    private int generatedBodyParts = 0;
    [SerializeField] float distanceFromHead;
    private float time;
    private List<Transform> travelPoints;
    private float moveSpeed;
    private float timeToRotate;

    private void Start()
    {
        time = distanceFromHead / GetComponent<PlatformController>().moveSpeed;
        travelPoints = GetComponent<PlatformController>().travelPoints;
        moveSpeed = GetComponent<PlatformController>().moveSpeed;
        timeToRotate = GetComponent<BodyPart>().timeToRotate;

        Destroy(transform.GetComponent<SpriteRenderer>());
        initialRotation = transform.localEulerAngles.z;

        timeElapsed = time / 2;

    }


    // Update is called once per frame
    void Update()
    {
        if(generatedBodyParts < numberOfPieces)
        {
            if(timeElapsed < time)
            {
                timeElapsed += Time.deltaTime;
            }
            else
            {
                GameObject bodyPart = new GameObject("bodyPart" + generatedBodyParts);
                bodyPart.transform.parent = transform.parent;
                bodyPart.transform.localScale = transform.localScale;
                bodyPart.transform.eulerAngles = new Vector3(0, 0, initialRotation);
                bodyPart.AddComponent<SpriteRenderer>();
                if(generatedBodyParts == 0)
                {
                    bodyPart.GetComponent<SpriteRenderer>().sprite = headSprite;
                    bodyPart.AddComponent<Animator>();
                    bodyPart.GetComponent<Animator>().runtimeAnimatorController = headAnimator;
                }
                else if(generatedBodyParts + 1 == numberOfPieces)
                    bodyPart.GetComponent<SpriteRenderer>().sprite = tailPartSprite;
                else
                    bodyPart.GetComponent<SpriteRenderer>().sprite = bodyPartSprite;
                bodyPart.AddComponent<PlatformController>();
                bodyPart.GetComponent<PlatformController>().travelPoints = travelPoints;
                bodyPart.GetComponent<PlatformController>().moveSpeed = moveSpeed;
                bodyPart.GetComponent<PlatformController>().loopPath = true;
                bodyPart.AddComponent<BodyPart>();
                bodyPart.GetComponent<BodyPart>().timeToRotate = timeToRotate;
                bodyPart.AddComponent<BoxCollider2D>();
                bodyPart.GetComponent<BoxCollider2D>().isTrigger = true;
                bodyPart.AddComponent<Spikes>();
                bodyPart.GetComponent<Spikes>().damageValue = damageValue;
                bodyPart.GetComponent<SpriteRenderer>().sortingLayerName = "Enemy";



                generatedBodyParts++;
                timeElapsed = Time.deltaTime;
            }
        }
    }
}
