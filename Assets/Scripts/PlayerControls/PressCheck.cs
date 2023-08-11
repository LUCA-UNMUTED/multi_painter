using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
public class PressCheck : MonoBehaviour
{

    public LineThickness lineThickness;
    public ActionBasedController controller;
    public PlayerInput playerInput;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        controller.selectAction.action.ReadValue<float>();


    }

}
