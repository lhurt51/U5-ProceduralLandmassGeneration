using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapPreview))]
public class MapPriviewEditor : Editor {

    public override void OnInspectorGUI()
    {
		base.OnInspectorGUI();

        MapPreview mapGen = (MapPreview)target;

        if (DrawDefaultInspector()) {
            if (mapGen.autoUpdate) {
                mapGen.DrawMapInEditor();
            }
        }

        if (GUILayout.Button("Generate")) {
            mapGen.DrawMapInEditor();
        }
    }

}
