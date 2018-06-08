using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomGradient {

    [SerializeField]
    List<ColorKey> keys = new List<ColorKey>();

    public int NumKeys
    {
        get { return keys.Count; }
    }

    public ColorKey GetKey(int i)
    {
        return keys[i];
    }

    public void AddKey(Color color, float time)
    {
        ColorKey newKey = new ColorKey(color, time);

        for (int i = 0; i < NumKeys; i++) if (newKey.Time < keys[i].Time) { keys.Insert(i, newKey); return; }

        keys.Add(newKey);
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
        if (keys.Count == 0) return Color.white;

        ColorKey keyLeft = keys[0];
        ColorKey keyRight = keys[NumKeys - 1];

        for (int i = 0; i < NumKeys - 1; i++)
        {
            if (keys[i].Time <= time && keys[i + 1].Time >= time)
            {
                keyLeft = keys[i];
                keyRight = keys[i + 1];
                break;
            }
        }

        float blendTime = Mathf.InverseLerp(keyLeft.Time, keyRight.Time, time);

        return Color.Lerp(keyLeft.Color, keyRight.Color, blendTime);
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
