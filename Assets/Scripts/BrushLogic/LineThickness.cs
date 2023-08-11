using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class LineThickness : MonoBehaviour
{


    public InputActionReference triggerActionL;
    public InputActionReference triggerActionR;
    public float triggerValueL;
    public float triggerValueR;

    public BrushStrokeMesh brushStrokeMesh;
    public GameObject BrushPrefab;
    private Coroutine triggerCoroutine;

    private void OnEnable()
    {
        triggerActionL.action.Enable();
        triggerActionR.action.Enable();
    }

    private void OnDisable()
    {
        triggerActionL.action.Disable();
        triggerActionR.action.Enable();
    }
    private void Awake()
    {

        brushStrokeMesh = BrushPrefab.GetComponentInChildren<BrushStrokeMesh>();
    }


    void Update()
    {
        triggerValueL = triggerActionL.action.ReadValue<float>();
        triggerValueR = triggerActionR.action.ReadValue<float>();

        //  Debug.Log(triggerValueL);
        // Debug.Log(triggerValueR);

        ChangeBrushLeft();
        ChangeBrushRight();
    }
    private void ChangeBrushLeft()
    {
        if (triggerValueL > 0.0f && triggerValueL < 0.2f)
        {

            brushStrokeMesh._brushStrokeWidth = 0.05f;
            Debug.Log(brushStrokeMesh._brushStrokeWidth);

        }

        else if (triggerValueL >= 0.2f && triggerValueL <= 0.5)
        {
            brushStrokeMesh._brushStrokeWidth = 0.1f;

           Debug.Log(brushStrokeMesh._brushStrokeWidth);
        }
        else if (triggerValueL >= 0.5f && triggerValueL < 0.8f)
        {
            brushStrokeMesh._brushStrokeWidth = 0.2f;

          Debug.Log(brushStrokeMesh._brushStrokeWidth);
        }

        else if (triggerValueL >= 0.8f)
        {
            brushStrokeMesh._brushStrokeWidth = 1f;

           Debug.Log(brushStrokeMesh._brushStrokeWidth);
        }
    }
    private void ChangeBrushRight()
    {
        if (triggerValueR > 0.0f && triggerValueR < 0.2f)
        {
            brushStrokeMesh._brushStrokeWidth = 0.05f;

        }

        else if (triggerValueR >= 0.2f && triggerValueR <= 0.5)
        {
            brushStrokeMesh._brushStrokeWidth = 0.05f;

        }
        else if (triggerValueR >= 0.5f && triggerValueR < 0.8f)
        {
            brushStrokeMesh._brushStrokeWidth = 0.08f;

        }

        else if (triggerValueR >= 0.8f)
        {
            brushStrokeMesh._brushStrokeWidth = 0.1f;

        }

    }
}
