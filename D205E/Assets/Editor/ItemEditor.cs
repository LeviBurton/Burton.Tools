using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Characters;
using System.Linq;
using System.Reflection;

public class ItemEditorWindow : EditorWindow
{
    void OnGUI()
    {
        GUILayout.Label("Item Editor", EditorStyles.boldLabel);
    }
}

