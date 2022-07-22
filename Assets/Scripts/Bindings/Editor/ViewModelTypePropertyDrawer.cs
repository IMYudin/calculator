using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ViewModelTypeAttribute))]
public class ViewModelTypePropertyDrawer : PropertyDrawer
{
    private int _selectedTypeIdx;
    private static string[] _viewModelTypeNames;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        _viewModelTypeNames ??= FindAllViewModelTypeNames();

        _selectedTypeIdx = Array.IndexOf(_viewModelTypeNames, property.stringValue);
        _selectedTypeIdx = EditorGUI.Popup(position, label.text, Array.IndexOf(_viewModelTypeNames, property.stringValue), _viewModelTypeNames);

        if (_selectedTypeIdx != -1)
        {
            property.stringValue = _viewModelTypeNames[_selectedTypeIdx];
        }
    }

    private static string[] FindAllViewModelTypeNames()
    {
        var viewModelType = typeof(ViewModelBase);
        return Assembly
            .GetAssembly(viewModelType)
            .GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(viewModelType))
            .Select(type => type.FullName)
            .ToArray();
    }
}