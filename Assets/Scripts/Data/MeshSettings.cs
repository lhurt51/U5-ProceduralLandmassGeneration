﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MeshSettings : UpdatableData {

	public float meshScale = 2.5f;
	public bool useFlatShadding;

    public const int numSupportedLODs = 5;
    public const int numSupportedChunkSizes = 9;
    public const int numSupportedFlatshadedChunkSizes = 3;
    public static readonly int[] supportedChunkSizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };

    [Range(0, numSupportedChunkSizes - 1)]
    public int chunkSizeIndex;
    [Range(0, numSupportedFlatshadedChunkSizes - 1)]
    public int flatshadedChunkSizeIndex;

    // num vertices per line of mesh rendered at LOD = 0. Includes the 2 extra verts that are excluded from final mesh, but used for calc normals
    public int numVertsPerLine {
        get {
            return supportedChunkSizes[(useFlatShadding) ? flatshadedChunkSizeIndex : chunkSizeIndex] + 1;
        }
    }

    public float meshWorldSize {
        get {
            return (numVertsPerLine - 3) * meshScale;
        }
    }

}
