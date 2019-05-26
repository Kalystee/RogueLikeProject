using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MeshSettings : UpdatableData
{
    public const int numberSupportedLOD = 5;
    public const int numberSupportedChunkSizes = 9;
    public const int numberSupportedFlatshadedChunkSizes = 3;
    public static readonly int[] supportedChunkSizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };

    public float meshScale  = 2.5f;
    public bool useFlatShading;

    [Range(0, numberSupportedChunkSizes - 1)]
    public int chunkSizeIndex;

    [Range(0, numberSupportedFlatshadedChunkSizes - 1)]
    public int flatShadedChunkSizeIndex;

    // number of vertices per line of mesh rendered at LOD = 0. Includes the 2 extra verts that are excluded from final mesh, but used for calculating normals
    public int NumVertsPerLine
    {
        get
        {
            return supportedChunkSizes[(useFlatShading) ? flatShadedChunkSizeIndex : chunkSizeIndex] +5;
        }
    }

    public float MeshWorldSize
    {
        get
        {
            return (NumVertsPerLine - 3) * meshScale;
        }
    }
}
