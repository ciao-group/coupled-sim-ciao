using System;
using UnityEngine;

/// <summary>
/// Screen setup contains the screen settings that the user can use to setup multi screen options
/// </summary>
[CreateAssetMenu(fileName = "DisplayData", menuName = "DisplaysSetup/DisplayDataSetup", order = 1)]
public class ScreenSetup_ScriptableObject : ScriptableObject
{
    /// <summary>
    /// Display data information
    /// </summary>
    [Serializable]
    public class ScreenSetupInformation
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string DisplayBrand;

        /// <summary>
        /// The screen's resolution Width
        /// </summary>
        [Tooltip("The screen's resolution Width")]
        public int Screen_WidthResolution;

        /// <summary>
        /// The screen's resolution Height
        /// </summary>
        [Tooltip("The screen's resolution Height")]
        public int Screen_HeightResolution;

        /// <summary>
        /// The screen's Refresh Rate
        /// </summary>
        [Tooltip("The screen's Refresh Rate")]
        public int Screen_RefreshRate = 60;
    }

    /// <summary>
    /// Display data
    /// </summary>
    public ScreenSetupInformation DisplayData;
}