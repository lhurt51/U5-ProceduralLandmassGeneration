using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CreateAssetMenu()]
public class GradTextureData : UpdatableData
{

    const int textureSize = 512;
    const TextureFormat textureFormat = TextureFormat.RGB565;

    public CustomGradMatLevel gradMatLevel;

    float savedMinHeight;
    float savedMaxHeight;

	public void ApplyToMaterial(Material material) {

        material.SetInt("layerCount", gradMatLevel.NumMats);
        material.SetColorArray("baseColors", gradMatLevel.GetAllMatLevels().Select(x => x.Tint).ToArray());
        material.SetFloatArray("baseStartHeights", gradMatLevel.GetAllMatLevels().Select(x => x.Height).ToArray());
        material.SetFloatArray("baseBlends", gradMatLevel.GetAllMatLevels().Select(x => x.BlendStrength).ToArray());
        material.SetFloatArray("baseColorStrength", gradMatLevel.GetAllMatLevels().Select(x => x.TintStrength).ToArray());
        material.SetFloatArray("baseTextureScales", gradMatLevel.GetAllMatLevels().Select(x => x.TextureScale).ToArray());
        Texture2DArray texturesArray = GenerateTextureArray(gradMatLevel.GetAllMatLevels().Select(x => x.Texture).ToArray());
        material.SetTexture("baseTextures", texturesArray);

        UpdateMeshHeights(material, savedMinHeight, savedMaxHeight);
	}

    public void UpdateMeshHeights(Material material, float minHeight, float maxHeight) {
        savedMinHeight = minHeight;
        savedMaxHeight = maxHeight;

        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);
    }

    Texture2DArray GenerateTextureArray(Texture2D[] textures) {
        Texture2DArray textureArray = new Texture2DArray(textureSize, textureSize, textures.Length, textureFormat, true);
        for (int i = 0; i < textures.Length; i++) {
            textureArray.SetPixels(textures[i].GetPixels(), i);
        }
        textureArray.Apply();
        return textureArray;
    }

}
