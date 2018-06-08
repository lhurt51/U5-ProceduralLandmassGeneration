using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CustomGradient))]
public class GradientDrawer : PropertyDrawer {

	public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        Event guiEvent = Event.current;
        CustomGradient gradient = (CustomGradient)fieldInfo.GetValue(prop.serializedObject.targetObject);
        float labelWidth = GUI.skin.label.CalcSize(label).x + 5;
        Rect textRect = new Rect(pos.x + labelWidth, pos.y, pos.width - labelWidth, pos.height);

        if (guiEvent.type == EventType.Repaint)
        {
            GUIStyle gradStyle = new GUIStyle();

            GUI.Label(pos, label);
            gradStyle.normal.background = gradient.GetTexture((int)pos.width);
            GUI.Label(textRect, GUIContent.none, gradStyle);
        }
        else if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && textRect.Contains(guiEvent.mousePosition))
        {
            EditorWindow.GetWindow<GradientEditor>();
        }
    }

}
