using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPreview : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        Mesh,
        FalloffMap
    }

    public DrawMode drawMode;

    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureData;

    public Material terrainMaterial;



    [Range(0, MeshSettings.numberSupportedLOD - 1)]
    public int editorPreviewLevelOfDetail;

    public bool autoUpdate;
    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawMapInEditor()
    {
        textureData.ApplyToMaterial(terrainMaterial);
        textureData.UpdateMeshHeights(terrainMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);

        HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.NumVertsPerLine, meshSettings.NumVertsPerLine, heightMapSettings, Vector2.zero);

        if (this.drawMode == DrawMode.NoiseMap)
        {
            DrawTexture(Texture_Generator.TextureFromHeightMap(heightMap));
        }

        else if (this.drawMode == DrawMode.Mesh)
        {
            DrawMesh(Mesh_Generator.GenerateTerrainMesh(heightMap.values, meshSettings, this.editorPreviewLevelOfDetail));
        }
        else if (this.drawMode == DrawMode.FalloffMap)
        {
            DrawTexture(Texture_Generator.TextureFromHeightMap(new HeightMap(FalloffGenerator.GenerateFalloffMap(meshSettings.NumVertsPerLine),0,1)));
        }
    }

    public void DrawTexture(Texture2D texture)
    {

        this.textureRender.sharedMaterial.mainTexture = texture;
        this.textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height) /10f;

        this.textureRender.gameObject.SetActive(true);
        this.meshFilter.gameObject.SetActive(false);
    }

    public void DrawMesh(MeshData meshData)
    {
        this.meshFilter.sharedMesh = meshData.CreateMesh();

        this.textureRender.gameObject.SetActive(false);
        this.meshFilter.gameObject.SetActive(true);
    }

    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    void OnTextureValuesUpdated()
    {
        textureData.ApplyToMaterial(terrainMaterial);
    }

    void OnValidate()
    {
        if (meshSettings != null)
        {
            meshSettings.OnValuesUpdated -= OnValuesUpdated;
            meshSettings.OnValuesUpdated += OnValuesUpdated;
        }

        if (heightMapSettings != null)
        {
            heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
            heightMapSettings.OnValuesUpdated += OnValuesUpdated;
        }

        if (textureData != null)
        {
            textureData.OnValuesUpdated -= OnTextureValuesUpdated;
            textureData.OnValuesUpdated += OnTextureValuesUpdated;

        }
    }
}
