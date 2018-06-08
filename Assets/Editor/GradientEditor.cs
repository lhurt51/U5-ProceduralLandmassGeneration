using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GradientEditor : EditorWindow {

    CustomGradient gradient;
    const int borderSize = 10;
    const float keyWidth = 10.0f;
    const float keyHeight = 20.0f;

    private void OnGUI()
    {
        Event guiEvent = Event.current;
        Rect gradPrevRect = new Rect(borderSize, borderSize, position.width - borderSize * 2, 25);

        GUI.DrawTexture(gradPrevRect, gradient.GetTexture((int)gradPrevRect.width));

        for (int i = 0; i < gradient.NumKeys; i++)
        {
            CustomGradient.ColorKey key = gradient.GetKey(i);
            Rect keyRect = new Rect(gradPrevRect.x + gradPrevRect.width * key.Time - keyWidth / 2.0f, gradPrevRect.yMax + borderSize, keyWidth, keyHeight);

            EditorGUI.DrawRect(keyRect, key.Color);
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            Color randColor = new Color(Random.value, Random.value, Random.value);
            float keyTime = Mathf.InverseLerp(gradPrevRect.x, gradPrevRect.xMax, guiEvent.mousePosition.x);

            gradient.AddKey(randColor, keyTime);
            Repaint();
        }
    }

    public void SetGradient(CustomGradient gradient)
    {
        this.gradient = gradient;
    }

	private void OnEnable()
    {
        titleContent.text = "Gradient Editor";
    }
}
