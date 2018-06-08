using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomGradient {

    public enum BlendMode { Linear, Discrete };
    public BlendMode blendMode;

    public bool bRandomizeColor;

    [SerializeField]
    List<ColorKey> keys = new List<ColorKey>();

    public CustomGradient()
    {
        AddKey(Color.white, 0.0f);
        AddKey(Color.black, 1.0f);
    }

    public int NumKeys
    {
        get { return keys.Count; }
    }

    public ColorKey GetKey(int i)
    {
        return keys[i];
    }

    public int AddKey(Color color, float time)
    {
        ColorKey newKey = new ColorKey(color, time);

        for (int i = 0; i < NumKeys; i++) if (newKey.Time < keys[i].Time) { keys.Insert(i, newKey); return i; }

        keys.Add(newKey);
        return NumKeys - 1;
    }

    public void RemoveKey(int index, bool bOverride = false)
    {
        if (bOverride || keys.Count >= 2) keys.RemoveAt(index);
    }

    public int UpdateKeyTime(int index, float time)
    {
        Color color = keys[index].Color;

        RemoveKey(index, true);

        return AddKey(color, time);
    }

    public void UpdateKeyColor(int index, Color col)
    {
        keys[index] = new ColorKey(col, keys[index].Time);
    }

    [System.Serializable]
    public struct ColorKey
    {
        [SerializeField]
        Color color;
        [SerializeField]
        float time;

        public ColorKey(Color color, float time)
        {
            this.color = color;
            this.time = time;
        }

        public Color Color
        {
            get
            { return color; }
        }

        public float Time
        {
            get { return time; }
        }
    }

	public Color Eval(float time)
    {
        ColorKey keyLeft = keys[0];
        ColorKey keyRight = keys[NumKeys - 1];

        for (int i = 0; i < NumKeys; i++)
        {
            if (keys[i].Time < time)
            {
                keyLeft = keys[i];
            }
            if (keys[i].Time > time)
            {
                keyRight = keys[i];
                break;
            }
        }

        if (blendMode == BlendMode.Linear)
        {
            float blendTime = Mathf.InverseLerp(keyLeft.Time, keyRight.Time, time);

            return Color.Lerp(keyLeft.Color, keyRight.Color, blendTime);
        }
        else
        {
            return keyRight.Color;
        }
    }

    public Texture2D GetTexture(int width)
    {
        Texture2D texture = new Texture2D(width, 1);
        Color[] colors = new Color[width];

        for (int i = 0; i < width; i++) colors[i] = Eval((float)i / (width - 1));

        texture.SetPixels(colors);
        texture.Apply();

        return texture;
    }

}
