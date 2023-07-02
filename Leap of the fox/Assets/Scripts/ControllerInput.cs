using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    public float gravity = 3;
    public float sensitivity = 3;
    public float axisValueHorizontal;
    public float axisValueVertical;
    private bool lockedHorizontal = false;
    private bool lockedVertical = false;
    public static ControllerInput instance;
    float targetHorizontal;
    float targetVertical;

    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        if (Input.GetAxis("Controller Horizontal") == 0)
        {
            lockedHorizontal = false;
        }

        if (Input.GetAxis("Controller Vertical") == 0)
        {
            lockedVertical = false;
        }

        targetHorizontal = Input.GetAxis("Controller Horizontal");
        targetVertical = Input.GetAxis("Controller Vertical");

        float speedHorizontal = (targetHorizontal == 0) ? gravity : sensitivity;
        axisValueHorizontal = Mathf.MoveTowards(axisValueHorizontal, targetHorizontal, speedHorizontal * Time.deltaTime);

        float speedVertical = (targetVertical == 0) ? gravity : sensitivity;
        axisValueVertical = Mathf.MoveTowards(axisValueVertical, targetVertical, speedVertical * Time.deltaTime);
        /*        float horiztontal = Input.GetAxis("Horizontal");
                float diff = Mathf.Abs(axisValueHorizontal - Input.GetAxis("Horizontal"));
                if(diff!=0)
                Debug.Log("Generated: " + axisValueHorizontal + "; Real: " + horiztontal + "; Diff: " + diff);*/
    }


    public float GetAxisHorizontal()
    {
        return axisValueHorizontal;
    }

    public float GetAxisRawHorizontal()
    {
        return targetHorizontal;
    }

    public float GetAxisVertical()
    {
        return axisValueVertical;
    }

    public float GetAxisRawVertical()
    {
        return targetVertical;
    }

    public bool GetButtonDown(DirectionEnum direction)
    {

        switch (direction)
        {
            case DirectionEnum.up:
                    if(targetVertical > 0 && !lockedVertical)
                    {
                        lockedVertical = true;
                        return true;
                    }
                return false;

            case DirectionEnum.down:
                if (targetVertical < 0 && !lockedVertical)
                {
                    lockedVertical = true;
                    return true;
                }
                return false;

            case DirectionEnum.left:
                if (targetHorizontal < 0 && !lockedHorizontal)
                {
                    lockedHorizontal = true;
                    return true;
                }
                return false;

            case DirectionEnum.right:
                if (targetHorizontal > 0 && !lockedHorizontal)
                {
                    lockedHorizontal = true;
                    return true;
                }
                return false;
        }

        return false;

    }
}
