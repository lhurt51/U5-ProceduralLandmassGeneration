using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CustomGradMatLevel))]
public class GradMatLevelDrawer : PropertyDrawer {

    static MapPreview mapPrev = null;

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        Event guiEvent = Event.current;
        CustomGradMatLevel gradient = (CustomGradMatLevel)fieldInfo.GetValue(prop.serializedObject.targetObject);
        float labelWidth = GUI.skin.label.CalcSize(label).x + 5;
        Rect textRect = new Rect(pos.x + labelWidth, pos.y, pos.width - labelWidth, pos.height);

        if (!mapPrev) mapPrev = GameObject.Find("MapPreview").GetComponent<MapPreview>();

        if (guiEvent.type == EventType.Repaint)
        {
            GUIStyle gradStyle = new GUIStyle();

            GUI.Label(pos, label);
            gradStyle.normal.background = gradient.GetTexture((int)pos.width);
            GUI.Label(textRect, GUIContent.none, gradStyle);

            if (mapPrev && mapPrev.autoUpdate) mapPrev.DrawMapInEditorGrad();
        }
        else if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && textRect.Contains(guiEvent.mousePosition))
        {
            GradMatLevelEditor window = EditorWindow.GetWindow<GradMatLevelEditor>();

            window.SetGradient(gradient);
        }
    }

}
