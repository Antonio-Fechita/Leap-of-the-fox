using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielding : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] Color damagedShieldColor;
    public GameObject bubbleShield;
    [SerializeField] int stepsToChangeShieldColor;
    [SerializeField] float cooldown;
    private bool inCooldown = false;
    private Coroutine shieldTimerCoroutine;

    private void Start()
    {
        bubbleShield.SetActive(false);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && inCooldown == false)
        {
            bubbleShield.SetActive(true);
            shieldTimerCoroutine = StartCoroutine(shieldTimer());
        }

        if(Input.GetKeyUp(KeyCode.Q))
        {
            StopCoroutine(shieldTimerCoroutine);
            bubbleShield.SetActive(false);
        }
    }

    IEnumerator shieldTimer() {
        for (int i = 0; i < stepsToChangeShieldColor; i++)
        {
            bubbleShield.GetComponent<SpriteRenderer>().color = Vector4.Lerp(Color.white, damagedShieldColor, i * 1.0f/(float)stepsToChangeShieldColor);
            yield return new WaitForSeconds(timer / stepsToChangeShieldColor);
        }
        PlayerHealthController.instance.takeDamage(2);
        StartCoroutine(cooldownTimer(cooldown));

    }

    public void BreakShield(float cooldown)
    {
        StartCoroutine(cooldownTimer(cooldown));
    }

    IEnumerator cooldownTimer(float cooldown)
    {
        bubbleShield.SetActive(false);
        inCooldown = true;
        yield return new WaitForSeconds(cooldown);
        inCooldown = false;
    }
}
