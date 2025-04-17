using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls Data containing a mapping of a control identifier (name) and the key assigned to the idfentifier
/// The user can use this to map a keypress to a more logical name.
/// </summary>
[CreateAssetMenu(fileName = "ControlsData", menuName = "HMI/VehicleControlsSetup", order = 1)]
public class Controls_ScriptableObject : ScriptableObject
{
    /// <summary>
    /// Class of Controls List that the user will generate
    /// Name is used for identification of this control
    /// Assigned Key is used for recognizing key presses to perform actions.
    /// </summary>

    [Serializable]
    public class ControlsList
    {
        /// <summary>
        /// Control identifier
        /// </summary>
        public string Name;

        /// <summary>
        /// Key assigned to the identifier
        /// </summary>
        public KeyCode AssignedKey;

        /// <summary>
        /// Should the key press be continuously triggered, or only if a complete press is registered?
        /// </summary>
        public bool Continuous;
    }

    /// <summary>
    /// List of ControlsList that user will populate
    /// </summary>
    public List<ControlsList> ControlsMappingList = new List<ControlsList>();
}