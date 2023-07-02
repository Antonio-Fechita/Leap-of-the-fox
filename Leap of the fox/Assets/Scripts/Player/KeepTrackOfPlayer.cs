using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepTrackOfPlayer : MonoBehaviour
{
    public bool seeingPlayer = false;
    [SerializeField] float visibilityRange = 10f;

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(PlayerController.instance.transform.position.x - transform.position.x, PlayerController.instance.transform.position.y - transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visibilityRange, LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position, direction, Color.red);
        seeingPlayer = (hit.collider != null);
    }
}
