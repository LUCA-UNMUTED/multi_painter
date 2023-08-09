using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class LineThickness : MonoBehaviour
{

    public InputActionReference LineSizePlus = null;
    public InputActionReference LineSizeMin = null;
    public BrushStrokeMesh brushStrokeMesh;
    public GameObject BrushPrefab;

    private void Awake()
    {
        LineSizePlus.action.started += ChangeLineSizePlus;
        LineSizeMin.action.started += ChangeLineSizeMin;
        brushStrokeMesh = BrushPrefab.GetComponentInChildren<BrushStrokeMesh>();
    }

    private void OnDestroy()
    {

        LineSizePlus.action.started -= ChangeLineSizePlus;
        LineSizeMin.action.started -= ChangeLineSizeMin;

    }
    // Update is called once per frame
    public void ChangeLineSizePlus(InputAction.CallbackContext context)
    {

        brushStrokeMesh._brushStrokeWidth = brushStrokeMesh._brushStrokeWidth + 0.01f;

    }
    public void ChangeLineSizeMin(InputAction.CallbackContext context)
    {

        brushStrokeMesh._brushStrokeWidth = brushStrokeMesh._brushStrokeWidth - 0.01f;

    }
}
