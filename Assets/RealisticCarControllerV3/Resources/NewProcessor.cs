using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Controls;
using UnityEditor;
using System.Diagnostics;

[System.Serializable]
public class RemapAxisProcessornegativetopos : InputProcessor<float>
{
    public override float Process(float value, InputControl control)
    {

        UnityEngine.Debug.Log($"Original Value: {value}");

        float remappedValue = (value + 1f) / 2f;

        UnityEngine.Debug.Log($"Remapped Value: {remappedValue}");

        return remappedValue;
    }
}

#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
#endif
static class RemapAxisProcessorRegistration
{
    static RemapAxisProcessorRegistration()
    {
        InputSystem.RegisterProcessor<RemapAxisProcessornegativetopos>();
    }



}
