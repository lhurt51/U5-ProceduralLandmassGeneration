using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GradientEditor : EditorWindow {

    CustomGradient gradient;
    const int borderSize = 10;
    const float keyWidth = 10.0f;
    const float keyHeight = 20.0f;

    Rect gradPrevRect;
    Rect[] keyRects;
    bool mouseIsOverKey;
    bool shouldRepaint;
    int selectedKeyIndex;

    private void OnGUI()
    {
        Draw();
        HandleInput();

        if (shouldRepaint)
        {
            shouldRepaint = false;
            Repaint();
        }
    }

    private void Draw()
    {
        gradPrevRect = new Rect(borderSize, borderSize, position.width - borderSize * 2, 25);

        GUI.DrawTexture(gradPrevRect, gradient.GetTexture((int)gradPrevRect.width));
        keyRects = new Rect[gradient.NumKeys];
        for (int i = 0; i < gradient.NumKeys; i++)
        {
            CustomGradient.ColorKey key = gradient.GetKey(i);
            Rect keyRect = new Rect(gradPrevRect.x + gradPrevRect.width * key.Time - keyWidth / 2.0f, gradPrevRect.yMax + borderSize, keyWidth, keyHeight);

            if (i == selectedKeyIndex) EditorGUI.DrawRect(new Rect(keyRect.x - 2, keyRect.y - 2, keyRect.width + 4, keyRect.height + 4), Color.black);
            EditorGUI.DrawRect(keyRect, key.Color);
            keyRects[i] = keyRect;
        }

        Rect settingsRect = new Rect(borderSize, keyRects[0].yMax + borderSize, position.width - borderSize * 2, position.height - borderSize);
        GUILayout.BeginArea(settingsRect);
        EditorGUI.BeginChangeCheck();

        Color newColor = EditorGUILayout.ColorField(gradient.GetKey(selectedKeyIndex).Color);

        if (EditorGUI.EndChangeCheck()) gradient.UpdateKeyColor(selectedKeyIndex, newColor);
        gradient.blendMode = (CustomGradient.BlendMode)EditorGUILayout.EnumPopup("Blend Mode", gradient.blendMode);
        gradient.bRandomizeColor = EditorGUILayout.Toggle("Randomize Color", gradient.bRandomizeColor);
        GUILayout.EndArea();
    }

    private void HandleInput()
    {
        Event guiEvent = Event.current;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            for (int i = 0; i < keyRects.Length; i++)
            {
                if (keyRects[i].Contains(guiEvent.mousePosition))
                {
                    mouseIsOverKey = true;
                    selectedKeyIndex = i;
                    shouldRepaint = true;
                    break;
                }
            }

            if (!mouseIsOverKey)
            {
                float keyTime = Mathf.InverseLerp(gradPrevRect.x, gradPrevRect.xMax, guiEvent.mousePosition.x);
                Color interpColor = gradient.Eval(keyTime);
                Color randColor = new Color(Random.value, Random.value, Random.value);

                selectedKeyIndex = gradient.AddKey((gradient.bRandomizeColor) ? randColor : interpColor, keyTime);
                mouseIsOverKey = true;
                shouldRepaint = true;
            }
        }
        else if (guiEvent.type == EventType.MouseUp && guiEvent.button == 0) mouseIsOverKey = false;
        else if (mouseIsOverKey && guiEvent.type == EventType.MouseDrag && guiEvent.button == 0)
        {
            float keyTime = Mathf.InverseLerp(gradPrevRect.x, gradPrevRect.xMax, guiEvent.mousePosition.x);

            selectedKeyIndex = gradient.UpdateKeyTime(selectedKeyIndex, keyTime);
            shouldRepaint = true;
        }
        else if (guiEvent.keyCode == KeyCode.Backspace && guiEvent.type == EventType.KeyDown)
        {
            if (selectedKeyIndex >= gradient.NumKeys) selectedKeyIndex--;
            gradient.RemoveKey(selectedKeyIndex);
            shouldRepaint = true;
        }
    }

    public void SetGradient(CustomGradient gradient)
    {
        this.gradient = gradient;
    }

	private void OnEnable()
    {
        titleContent.text = "Gradient Editor";
        position.Set(position.x, position.y, 400, 150);
        minSize = new Vector2(200, 150);
        maxSize = new Vector2(1920, 150);
    }

    private void OnDisable()
    {
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
}
