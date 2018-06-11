using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomGradMatLevel {

    public bool bRandomizeTint;

    [System.Serializable]
    public class MatLevel
    {
        [SerializeField]
        Texture2D texture;
        [SerializeField]
        Color tint;
        [SerializeField]
        float height;
        [SerializeField]
        float tintStrength;
        [SerializeField]
        float blendStrength;
        [SerializeField]
        float textureScale;

        public MatLevel(Color tint, float height, Texture2D texture, float tintStrength, float blendStrength, float textureScale)
        {
            this.tint = tint;
            this.height = height;
            this.texture = texture;
            this.tintStrength = tintStrength;
            this.blendStrength = blendStrength;
            this.textureScale = textureScale;
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Color Tint
        {
            get { return tint; }
        }

        public float Height
        {
            get { return height; }
        }

        public float TintStrength
        {
            get { return tintStrength; }
        }

        public float BlendStrength
        {
            get { return blendStrength; }
        }

        public float TextureScale
        {
            get { return textureScale; }
        }
    }

    [SerializeField]
    List<MatLevel> mats = new List<MatLevel>();

    public CustomGradMatLevel()
    {
        AddMat(Color.white, 0.0f);
        AddMat(Color.black, 1.0f);
    }

    public int NumMats
    {
        get { return mats.Count; }
    }

    public MatLevel GetMatLevel(int i)
    {
        return mats[i];
    }

    public List<MatLevel> GetAllMatLevels()
    {
        return mats;
    }

    public int AddMat(Color color, float time, Texture2D texture = null, float tintStrength = 1.0f, float blendStrength = 0.1f, float textureScale = 25.0f)
    {
        MatLevel newMat = new MatLevel(color, time, texture, tintStrength, blendStrength, textureScale);

        for (int i = 0; i < NumMats; i++) if (newMat.Height < mats[i].Height) { mats.Insert(i, newMat); return i; }

        mats.Add(newMat);
        return NumMats - 1;
    }

    public void RemoveMat(int index, bool bOverride = false)
    {
        if (bOverride || mats.Count >= 2) mats.RemoveAt(index);
    }

    public int UpdateMatHeight(int index, float height)
    {
        Color tint = mats[index].Tint;
        Texture2D tex = mats[index].Texture;
        float tintStrengeth = mats[index].TintStrength;
        float blendStrength = mats[index].BlendStrength;
        float textScale = mats[index].TextureScale;

        RemoveMat(index, true);

        return AddMat(tint, height, tex, tintStrengeth, blendStrength, textScale);
    }

    public void UpdateMatTint(int index, Color col)
    {
        mats[index] = new MatLevel(col, mats[index].Height, mats[index].Texture, mats[index].TintStrength, mats[index].BlendStrength, mats[index].TextureScale);
    }

    public void UpdateMatTexture(int index, Texture2D tex)
    {
        mats[index] = new MatLevel(mats[index].Tint, mats[index].Height, tex, mats[index].TintStrength, mats[index].BlendStrength, mats[index].TextureScale);
    }

    public void UpdateMatTintStrength(int index, float tintStrength)
    {
        mats[index] = new MatLevel(mats[index].Tint, mats[index].Height, mats[index].Texture, tintStrength, mats[index].BlendStrength, mats[index].TextureScale);
    }

    public void UpdateMatBlendStrength(int index, float blendStrength)
    {
        mats[index] = new MatLevel(mats[index].Tint, mats[index].Height, mats[index].Texture, mats[index].TintStrength, blendStrength, mats[index].TextureScale);
    }

    public void UpdateMatTextureScale(int index, float textScale)
    {
        mats[index] = new MatLevel(mats[index].Tint, mats[index].Height, mats[index].Texture, mats[index].TintStrength, mats[index].BlendStrength, textScale);
    }

    public Color Eval(float height)
    {
        MatLevel keyLeft = mats[0];
        MatLevel keyRight = mats[NumMats - 1];

        for (int i = 0; i < NumMats; i++)
        {
            if (mats[i].Height < height)
            {
                keyLeft = mats[i];
            }
            if (mats[i].Height > height)
            {
                keyRight = mats[i];
                break;
            }
        }

        return keyLeft.Tint;
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
