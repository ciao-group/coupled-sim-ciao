using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Creates an attribute that is used to create a dropdown menu with a list of data in the inspector
/// </summary>
public class ListToPopupAttribute : PropertyAttribute
{
    public Type MyType;
    public string PropertyName;

    public ListToPopupAttribute(Type myType, string propertyName)
    {
        MyType = myType;
        PropertyName = propertyName;
    }
}

/// <summary>
/// Creates a property drawer for ListToPopupAttribute
/// </summary>
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ListToPopupAttribute))]
public class ListToPopupDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var atb = attribute as ListToPopupAttribute;
        List<string> stringList = null;
        if (atb.MyType.GetField(atb.PropertyName) != null)
        {
            stringList = atb.MyType.GetField(atb.PropertyName).GetValue(atb.MyType) as List<string>;
        }

        if (stringList != null && stringList.Count != 0)
        {
            var selectedIndex = Mathf.Max(stringList.IndexOf(property.stringValue), 0);
            selectedIndex = EditorGUI.Popup(position, property.name, selectedIndex, stringList.ToArray());
            property.stringValue = stringList[selectedIndex];
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }

    }
}
#endif