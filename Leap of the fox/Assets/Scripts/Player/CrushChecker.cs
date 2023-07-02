using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushChecker : MonoBehaviour
{
    [SerializeField] Transform left;
    [SerializeField] Transform right;
    [SerializeField] Transform up;
    [SerializeField] Transform down;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        LayerMask layerMask = Physics2D.AllLayers;
        layerMask &= ~(1 << LayerMask.NameToLayer("Player"));
        layerMask &= ~(1 << LayerMask.NameToLayer("Camera"));

        Collider2D leftCollider = Physics2D.OverlapCircle(left.position, 0.05f, layerMask);
        Collider2D rightCollider = Physics2D.OverlapCircle(right.position, 0.05f, layerMask);
        Collider2D upCollider = Physics2D.OverlapCircle(up.position, 0.05f, layerMask);
        Collider2D downCollider = Physics2D.OverlapCircle(down.position, 0.05f, layerMask);



        if (leftCollider!=null && rightCollider != null) //Player is colliding with objects below and above him at the same time
        {
            CrushingObject leftCrusherComponent = leftCollider.GetComponent<CrushingObject>();
            CrushingObject rightCrusherComponent = rightCollider.GetComponent<CrushingObject>();

            if ((leftCrusherComponent != null && leftCrusherComponent.currentMovingDirection == DirectionEnum.right) ||
                (rightCrusherComponent != null && rightCrusherComponent.currentMovingDirection == DirectionEnum.left))
            {
                PlayerHealthController.instance.takeDamage(100);
            }
        }
        else if(upCollider!=null && downCollider != null)
        {
            CrushingObject upCrusherComponent = upCollider.GetComponent<CrushingObject>();
            CrushingObject downCrusherComponent = downCollider.GetComponent<CrushingObject>();

            if ((upCrusherComponent != null && upCrusherComponent.currentMovingDirection == DirectionEnum.down) ||
                (downCrusherComponent != null && downCrusherComponent.currentMovingDirection == DirectionEnum.up))
            {
                PlayerHealthController.instance.takeDamage(100);
            }
        }
    }
}
