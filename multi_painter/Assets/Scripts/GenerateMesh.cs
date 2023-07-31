using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class GenerateMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //InputSystem.onActionChange +=
        //(obj, change) =>
        //{
        //    // obj can be either an InputAction or an InputActionMap
        //    // depending on the specific change.
        //    switch (change)
        //    {
        //        case InputActionChange.ActionStarted:
        //        case InputActionChange.ActionPerformed:
        //        case InputActionChange.ActionCanceled:
        //            Debug.Log($"{((InputAction)obj).name} {change}");
        //            break;
        //    }
        //}

        

    }

    // Update is called once per frame
    void Update()
    {
        //var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        //var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        //UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);

        //foreach (var device in leftHandedControllers)
        //{
        //    bool triggerValue;
        //    if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
        //    {
        //        Debug.Log("Trigger button is pressed.");
        //    }
        //}
        
    }

    static InputAction GetInputAction(InputActionReference actionReference)
    {
#pragma warning disable IDE0031 // Use null propagation -- Do not use for UnityEngine.Object types
        return actionReference != null ? actionReference.action : null;
#pragma warning restore IDE0031
    }

    static void EnableAction(InputActionReference actionReference)
    {
        var action = GetInputAction(actionReference);
        if (action != null && !action.enabled)
            action.Enable();
    }

    static void DisableAction(InputActionReference actionReference)
    {
        var action = GetInputAction(actionReference);
        if (action != null && action.enabled)
            action.Disable();
    }


    public void WriteThings()
    {
        Debug.Log("check");
    }

    public void WarnThings(string text)
    {
        Debug.LogWarning(text);
    }
}
