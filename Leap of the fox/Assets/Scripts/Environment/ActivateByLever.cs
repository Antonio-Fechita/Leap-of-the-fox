using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateByLever : MonoBehaviour
{
    [SerializeField] Lever lever;
    [SerializeField] bool stateSameAsLever = true;

    private void Start()
    {
        lever.upEvent.AddListener(OnLeverUp);
        lever.downEvent.AddListener(OnLeverDown);
    }


    private void OnLeverUp()
    {
        transform.gameObject.SetActive(stateSameAsLever);
    }

    private void OnLeverDown()
    {
        transform.gameObject.SetActive(!stateSameAsLever);
    }
}
