using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesChecker : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] AnimatorOverrideController boots;
    void Start()
    {
        List<bool> active = new List<bool>(SaveSystem.LoadShop().active);


        if (active[0]) //bubble shield
        {
            GetComponent<Shielding>().enabled = true;
        }

        if (active[1]) //gun
        {
            gun.SetActive(true);
        }

        if (active[2]) //boots
        {
            Debug.Log("Should equip boots");
            PlayerController.instance.hasBootsPowerup = true;
            //PlayerController.instance.animator.runtimeAnimatorController = boots;
            PlayerController.instance.transform.GetComponent<Animator>().runtimeAnimatorController = boots;
            Debug.Log(PlayerController.instance.animator.runtimeAnimatorController.name);
        }

        if (active[3]) //dash
        {
            GetComponent<PlayerDash>().enabled = true;
        }
    }

}
