using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectables : MonoBehaviour
{
    public static int gemsCollected = 0;
    public static UnityEvent<int> gemCollectedEvent = new UnityEvent<int>();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("Gems collected: " + gemsCollected);
            AudioHelper.CreateOneShotSound(GetComponent<AudioSource>().clip, transform.position);
            Destroy(gameObject);
            gemsCollected++;
            gemCollectedEvent.Invoke(gemsCollected);
        }
    }
}
