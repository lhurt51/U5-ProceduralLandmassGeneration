﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPreview : MonoBehaviour {

    public Renderer textureRender;
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

    public enum DrawMode { NoiseMap, Mesh, FalloffMap };
    public DrawMode drawMode;

    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureData;
    public GradTextureData gradTextData;

    public Material terrainMaterial;

    [Range(0, MeshSettings.numSupportedLODs - 1)]
    public int editorPreviewLOD;

    public bool autoUpdate;

    public void DrawMapInEditor()
    {
        textureData.ApplyToMaterial(terrainMaterial);
        textureData.UpdateMeshHeights(terrainMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);
        HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero);

        if (drawMode == DrawMode.NoiseMap)
            DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
        else if (drawMode == DrawMode.Mesh)
            DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, editorPreviewLOD));
        else if (drawMode == DrawMode.FalloffMap)
            DrawTexture(TextureGenerator.TextureFromHeightMap(new HeightMap(FalloffGenerator.GenerateFalloffMap(meshSettings.numVertsPerLine), 0, 1)));
    }

    public void DrawMapInEditorGrad()
    {
        gradTextData.ApplyToMaterial(terrainMaterial);
        gradTextData.UpdateMeshHeights(terrainMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);
        HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero);

        if (drawMode == DrawMode.NoiseMap)
            DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
        else if (drawMode == DrawMode.Mesh)
            DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, editorPreviewLOD));
        else if (drawMode == DrawMode.FalloffMap)
            DrawTexture(TextureGenerator.TextureFromHeightMap(new HeightMap(FalloffGenerator.GenerateFalloffMap(meshSettings.numVertsPerLine), 0, 1)));
    }

    public void DrawTexture(Texture2D texture) {
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height) / 10.0f;

        textureRender.gameObject.SetActive(true);
        meshFilter.gameObject.SetActive(false);
    }

	public void DrawMesh(MeshData meshData) {
		meshFilter.sharedMesh = meshData.CreateMesh();

        textureRender.gameObject.SetActive(false);
        meshFilter.gameObject.SetActive(true);
    }

    void OnValuesUpdated() {
        if (!Application.isPlaying)
            DrawMapInEditor();
    }

    void OnValuesUpdatedGrad()
    {
        if (!Application.isPlaying)
            DrawMapInEditorGrad();
    }

    void OnTextureValuesUpdated() {
        textureData.ApplyToMaterial(terrainMaterial);
    }

    void OnGradTextureValuesUpdated()
    {
        gradTextData.ApplyToMaterial(terrainMaterial);
    }

    void OnValidate()
    {
        if (meshSettings != null)
        {
            meshSettings.OnValuesUpdated -= OnValuesUpdatedGrad;
            meshSettings.OnValuesUpdated += OnValuesUpdatedGrad;
        }
        if (heightMapSettings != null)
        {
            heightMapSettings.OnValuesUpdated -= OnValuesUpdatedGrad;
            heightMapSettings.OnValuesUpdated += OnValuesUpdatedGrad;
        }
        if (textureData != null)
        {
            textureData.OnValuesUpdated -= OnTextureValuesUpdated;
            textureData.OnValuesUpdated += OnTextureValuesUpdated;
        }
        if (gradTextData != null)
        {
            gradTextData.OnValuesUpdated -= OnGradTextureValuesUpdated;
            gradTextData.OnValuesUpdated += OnGradTextureValuesUpdated;
        }
    }
}
