using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TerrainTools;

namespace Kits.ClientKit.Handlers
{
    
    public static class TerrainHandler
    {
        public enum StitchDirection { North, South, West, East };
        
        /// <summary>
        /// Calculates the scalar u/v position on a terrain from a world space position
        /// </summary>
        /// <param name="terrain">The terrain for which to perform the calculation.</param>
        /// <param name="worldSpacePosition">The world space position to transform to UV space.</param>
        /// <returns></returns>
        public static Vector2 ConvertPositonToTerrainUV(Terrain terrain, Vector2 worldSpacePosition)
        {
            float u = (worldSpacePosition.x - terrain.transform.position.x) / terrain.terrainData.size.x;
            float v = (worldSpacePosition.y - terrain.transform.position.z) / terrain.terrainData.size.z;
            return new Vector2(u, v);
        }
        
        
        /// <summary>
        /// Gets the max spawn range for a spawner tool (spawner / biome controller)
        /// </summary>
        /// <param name="currentTerrain"></param>
        /// <returns></returns>
        public static float GetMaxSpawnRange(Terrain currentTerrain)
        {
            if (currentTerrain != null)
            {
                return Mathf.Round((float)8192 / (float)currentTerrain.terrainData.heightmapResolution * currentTerrain.terrainData.size.x / 2f);
            }
            else
            {
                return 1000;
            }
        }
        
          /// <summary>
        /// Calculate a normal map for a terrain
        /// </summary>
        /// <returns>Normals for the terrain in a Texture 2D</returns>
        public static Texture2D CalculateNormals(Terrain terrain)
        {
            int width = terrain.terrainData.heightmapResolution;
            int height = terrain.terrainData.heightmapResolution;
            float ux = 1.0f / (width - 1.0f);
            float uy = 1.0f / (height - 1.0f);
            float terrainHeight = width / 2f;
            float scaleX = terrainHeight / (float)width;
            float scaleY = terrainHeight / (float)height;
            float[] heights = new float[width * height];
            Buffer.BlockCopy(terrain.terrainData.GetHeights(0, 0, width, height), 0, heights, 0, heights.Length * sizeof(float));
            Texture2D normalMap = new Texture2D(width, height, TextureFormat.RGBAFloat, false, true);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int xp1 = (x == width - 1) ? x : x + 1;
                    int xn1 = (x == 0) ? x : x - 1;

                    int yp1 = (y == height - 1) ? y : y + 1;
                    int yn1 = (y == 0) ? y : y - 1;

                    float l = heights[xn1 + y * width] * scaleX;
                    float r = heights[xp1 + y * width] * scaleX;

                    float b = heights[x + yn1 * width] * scaleY;
                    float t = heights[x + yp1 * width] * scaleY;

                    float dx = (r - l) / (2.0f * ux);
                    float dy = (t - b) / (2.0f * uy);

                    Vector3 normal;
                    normal.x = -dx;
                    normal.y = -dy;
                    normal.z = 1;
                    normal.Normalize();

                    Color pixel;
                    pixel.r = normal.x * 0.5f + 0.5f;
                    pixel.g = normal.y * 0.5f + 0.5f;
                    pixel.b = normal.z;
                    pixel.a = 1.0f;

                    normalMap.SetPixel(x, y, pixel);
                }
            }
            normalMap.Apply();
            return normalMap;
        }
          
                  /// <summary>
        /// Flatten all the active terrains
        /// </summary>
        public static void Flatten()
        {
            FlattenTerrain(Terrain.activeTerrains);
        }

        /// <summary>
        /// Flatten the terrain passed in
        /// </summary>
        /// <param name="terrain">Terrain to be flattened</param>
        public static void FlattenTerrain(Terrain terrain)
        {
            float[,] heights = new float[terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution];
            terrain.terrainData.SetHeights(0, 0, heights);
        }

        /// <summary>
        /// Flatten all the terrains passed in
        /// </summary>
        /// <param name="terrains">Terrains to be flattened</param>
        public static void FlattenTerrain(Terrain[] terrains)
        {
            foreach (Terrain terrain in terrains)
            {
                float[,] heights = new float[terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution];
                terrain.terrainData.SetHeights(0, 0, heights);
            }
        }

        /// <summary>
        /// Stitch the terrains together with unity set neighbors calls
        /// </summary>
        public static void Stitch()
        {
            StitchTerrains(Terrain.activeTerrains);
        }

        /// <summary>
        /// Stitch the terrains together - wont align them although should update this to support that as well.
        /// </summary>
        /// <param name="terrains">Array of terrains to organise as neighbors</param>
        public static void StitchTerrains(Terrain[] terrains)
        {
            Terrain right = null;
            Terrain left = null;
            Terrain bottom = null;
            Terrain top = null;

            foreach (Terrain terrain in terrains)
            {
                right = null;
                left = null;
                bottom = null;
                top = null;

                foreach (Terrain neighbor in terrains)
                {
                    //Check to see if neighbor is above or below
                    if (neighbor.transform.position.x == terrain.transform.position.x)
                    {
                        if ((neighbor.transform.position.z + neighbor.terrainData.size.z) == terrain.transform.position.z)
                        {
                            top = neighbor;
                        }
                        else if ((terrain.transform.position.z + terrain.terrainData.size.z) == neighbor.transform.position.z)
                        {
                            bottom = neighbor;
                        }
                    }
                    else if (neighbor.transform.position.z == terrain.transform.position.z)
                    {
                        if ((neighbor.transform.position.x + neighbor.terrainData.size.z) == terrain.transform.position.z)
                        {
                            left = neighbor;
                        }
                        else if ((terrain.transform.position.x + terrain.terrainData.size.x) == neighbor.transform.position.x)
                        {
                            right = neighbor;
                        }
                    }
                }

                terrain.SetNeighbors(left, top, right, bottom);
            }
        }
        
        /// <summary>
        /// Get the vector of the centre of the active terrain, and flush to ground level if asked to
        /// </summary>
        /// <param name="flushToGround">If true set it flush to the ground</param>
        /// <returns>Vector3.zero if no terrain, otherwise the centre of it</returns>
        public static Vector3 GetActiveTerrainCenter(bool flushToGround = true)
        {
            Bounds b = new Bounds();
            Terrain t = GetActiveTerrain();
            if (GetTerrainBounds(t, ref b))
            {
                if (flushToGround == true)
                {
                    return new Vector3(b.center.x, t.SampleHeight(b.center), b.center.z);
                }
                else
                {
                    return b.center;
                }
            }
            return Vector3.zero;
        }

        /// <summary>
        /// Get any active terrain - pref active terrain
        /// </summary>
        /// <returns>Any active terrain or null</returns>
        public static Terrain GetActiveTerrain()
        {
            //Grab active terrain if we can
            Terrain terrain = Terrain.activeTerrain;
            if (terrain != null && terrain.isActiveAndEnabled)
            {
                return terrain;
            }

            //Then check rest of terrains
            for (int idx = 0; idx < Terrain.activeTerrains.Length; idx++)
            {
                terrain = Terrain.activeTerrains[idx];
                if (terrain != null && terrain.isActiveAndEnabled)
                {
                    return terrain;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the layer mask of the active terrain, or default if there isnt one
        /// </summary>
        /// <returns>Layermask of activer terrain or default if there isnt one</returns>
        public static LayerMask GetActiveTerrainLayer()
        {
            LayerMask layer = new LayerMask();
            Terrain terrain = GetActiveTerrain();
            if (terrain != null)
            {
                layer.value = 1 << terrain.gameObject.layer;
                return layer;
            }
            layer.value = 1 << LayerMask.NameToLayer("Default");
            return layer;
        }

        /// <summary>
        /// Get the layer mask of the active terrain, or default if there isnt one
        /// </summary>
        /// <returns>Layermask of activer terrain or default if there isnt one</returns>
        public static LayerMask GetActiveTerrainLayerAsInt()
        {
            LayerMask layerValue = GetActiveTerrainLayer().value;
            for (int layerIdx = 0; layerIdx < 32; layerIdx++)
            {
                if (layerValue == (1 << layerIdx))
                {
                    return layerIdx;
                }
            }
            return LayerMask.NameToLayer("Default");
        }
        
        /// <summary>
        /// Returns the Quaternion rotation from the active terrain normal
        /// </summary>
        /// <param name="terrain"></param>
        /// <param name="playerObj"></param>
        /// <returns></returns>
        public static Vector3 GetRotationFromTerrainNormal(Terrain terrain, GameObject playerObj)
        {
            if (terrain != null && playerObj != null)
            {
                float scalarX = (playerObj.transform.position.x - terrain.transform.position.x) / (float)terrain.terrainData.size.x;
                float scalarZ = (playerObj.transform.position.z - terrain.transform.position.z) / (float)terrain.terrainData.size.z;
                Vector3 interpolatedNormal = terrain.terrainData.GetInterpolatedNormal(scalarX, scalarZ);
                Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, interpolatedNormal) * playerObj.transform.rotation;
                return quaternion.eulerAngles;
            }
            else
            {
                return Vector3.zero;
            }
        }

        /// <summary>
        /// Get the bounds of the space encapsulated by the supplied terrain
        /// </summary>
        /// <param name="terrain">Terrain to get bounds for</param>
        /// <param name="bounds">Bounds to update</param>
        /// <returns>True if we got some terrain bounds</returns>
        public static bool GetTerrainBounds(Terrain terrain, ref Bounds bounds)
        {
            if (terrain == null)
            {
                return false;
            }
            bounds.center = terrain.transform.position;
            bounds.size = terrain.terrainData.size;
            bounds.center += bounds.extents;
            return true;
        }

        /// <summary>
        /// Get a random location on the terrain supplied
        /// </summary>
        /// <param name="terrain">Terrain to check</param>
        /// <param name="start">Start locaton</param>
        /// <param name="radius">Radius to hunt in</param>
        /// <returns></returns>
        public static Vector3 GetRandomPositionOnTerrain(Terrain terrain, Vector3 start, float radius)
        {
            Vector3 newLocation;
            Vector3 terrainMin = terrain.GetPosition();
            Vector3 terrainMax = terrainMin + terrain.terrainData.size;
            while (true)
            {
                //Get a new location
                newLocation = UnityEngine.Random.insideUnitSphere * radius;
                newLocation = start + newLocation;
                //Make sure the new location is within the terrain bounds
                if (newLocation.x >= terrainMin.x && newLocation.x <= terrainMax.x)
                {
                    if (newLocation.z >= terrainMin.z && newLocation.z <= terrainMax.z)
                    {
                        //Update it to be on the terrain surface
                        newLocation.y = terrain.SampleHeight(newLocation);
                        return newLocation;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the bounds of a terrain in world space (The bounds in the terrainData object is in local space of the terrain itself)
        /// </summary>
        /// <param name="t">The terrain to get the bounds in world space for.</param>
        /// <returns></returns>
        public static Bounds GetWorldSpaceBounds(Terrain t)
        {
            Bounds worldSpaceBounds = t.terrainData.bounds;
            worldSpaceBounds.center = new Vector3(worldSpaceBounds.center.x + t.transform.position.x, worldSpaceBounds.center.y + t.transform.position.y, worldSpaceBounds.center.z + t.transform.position.z);
            return worldSpaceBounds;
        }

        /// <summary>
        /// Resizes the terrain splatmap to a new resolution.
        /// </summary>
        /// <param name="terrain"></param>
        /// <param name="targetResolution"></param>
        public static void ResizeSplatmaps(Terrain terrain, int targetResolution)
        {
            TerrainData terrainData = terrain.terrainData;
            Material blitMaterial = TerrainPaintUtility.GetCopyTerrainLayerMaterial();      // special blit that forces copy from highest mip only
            RenderTexture[] resizedSplatMaps = new RenderTexture[terrainData.alphamapTextureCount];

            int targetResolutionU = targetResolution;
            int targetResolutionV = targetResolution;
            float invTargetResolutionU = 1.0f / targetResolutionU;
            float invTargetResolutiuonV = 1.0f / targetResolutionV;

            RenderTexture currentRT = RenderTexture.active;

            for (int i = 0; i < terrainData.alphamapTextureCount; i++)
            {
                Texture2D oldSplatmap = terrainData.alphamapTextures[i];

                int sourceResolutionU = oldSplatmap.width;
                int sourceResolutionV = oldSplatmap.height;
                float invSourceResoultionU = 1.0f / sourceResolutionU;
                float invSourceResolutionV = 1.0f / sourceResolutionV;

                resizedSplatMaps[i] = RenderTexture.GetTemporary(targetResolution, targetResolution, 0, oldSplatmap.graphicsFormat);

                float scaleU = (1.0f - invSourceResoultionU) / (1.0f - invTargetResolutionU);
                float scaleV = (1.0f - invSourceResolutionV) / (1.0f - invTargetResolutiuonV);
                float offsetU = 0.5f * (invSourceResoultionU - scaleU * invTargetResolutionU);
                float offsetV = 0.5f * (invSourceResolutionV - scaleV * invTargetResolutiuonV);

                Vector2 scale = new Vector2(scaleU, scaleV);
                Vector2 offset = new Vector2(offsetU, offsetV);

                blitMaterial.mainTexture = oldSplatmap;
                blitMaterial.mainTextureScale = scale;
                blitMaterial.mainTextureOffset = offset;

                oldSplatmap.filterMode = FilterMode.Bilinear;
                RenderTexture.active = resizedSplatMaps[i];
                GL.PushMatrix();
                GL.LoadPixelMatrix(0, targetResolution, 0, targetResolution);
                blitMaterial.SetPass(2);

                RectInt targetPixelRect = new RectInt(0, 0, targetResolution, targetResolution);
                RectInt sourcePixelRect = new RectInt(0, 0, sourceResolutionU, sourceResolutionV);

                if ((targetPixelRect.width > 0) && (targetPixelRect.height > 0))
                {
                    Rect sourceUVs = new Rect(
                        (sourcePixelRect.x) / (float)oldSplatmap.width,
                        (sourcePixelRect.y) / (float)oldSplatmap.height,
                        (sourcePixelRect.width) / (float)oldSplatmap.width,
                        (sourcePixelRect.height) / (float)oldSplatmap.height);

                    Rect sourceUVs2 = new Rect(
                        (sourcePixelRect.x) / (float)oldSplatmap.width,
                        (sourcePixelRect.y) / (float)oldSplatmap.height,
                        (sourcePixelRect.width) / (float)oldSplatmap.width,
                        (sourcePixelRect.height) / (float)oldSplatmap.height);

                    GL.Begin(GL.QUADS);
                    GL.Color(new Color(1.0f, 1.0f, 1.0f, 1.0f));
                    GL.MultiTexCoord2(0, sourceUVs.x, sourceUVs.y);
                    GL.MultiTexCoord2(1, sourceUVs2.x, sourceUVs2.y);
                    GL.Vertex3(targetPixelRect.x, targetPixelRect.y, 0.0f);
                    GL.MultiTexCoord2(0, sourceUVs.x, sourceUVs.yMax);
                    GL.MultiTexCoord2(1, sourceUVs2.x, sourceUVs2.yMax);
                    GL.Vertex3(targetPixelRect.x, targetPixelRect.yMax, 0.0f);
                    GL.MultiTexCoord2(0, sourceUVs.xMax, sourceUVs.yMax);
                    GL.MultiTexCoord2(1, sourceUVs2.xMax, sourceUVs2.yMax);
                    GL.Vertex3(targetPixelRect.xMax, targetPixelRect.yMax, 0.0f);
                    GL.MultiTexCoord2(0, sourceUVs.xMax, sourceUVs.y);
                    GL.MultiTexCoord2(1, sourceUVs2.xMax, sourceUVs2.y);
                    GL.Vertex3(targetPixelRect.xMax, targetPixelRect.y, 0.0f);
                    GL.End();
                }

                GL.PopMatrix();
            }

            terrainData.alphamapResolution = targetResolution;
            for (int i = 0; i < resizedSplatMaps.Length; i++)
            {
                RenderTexture.active = resizedSplatMaps[i];
                terrainData.CopyActiveRenderTextureToTexture(TerrainData.AlphamapTextureName, i, new RectInt(0, 0, targetResolution, targetResolution), Vector2Int.zero, false);
            }
            terrainData.SetBaseMapDirty();
            RenderTexture.active = currentRT;
            for (int i = 0; i < resizedSplatMaps.Length; i++)
                RenderTexture.ReleaseTemporary(resizedSplatMaps[i]);
        }

        /// <summary>
        /// Resizes the heightmap of a terrain to the target resolution
        /// </summary>
        /// <param name="terrain"></param>
        /// <param name="targetResolution"></param>
        public static void ResizeHeightmap(Terrain terrain, int targetResolution)
        {
            RenderTexture currentRT = RenderTexture.active;

            RenderTexture oldHeightmap = RenderTexture.GetTemporary(terrain.terrainData.heightmapTexture.descriptor);
            Graphics.Blit(terrain.terrainData.heightmapTexture, oldHeightmap);

            RenderTexture oldHoles = RenderTexture.GetTemporary(terrain.terrainData.holesTexture.width, terrain.terrainData.holesTexture.height, 0, Terrain.holesRenderTextureFormat);
            Graphics.Blit(terrain.terrainData.holesTexture, oldHoles);

            int dWidth = terrain.terrainData.heightmapResolution;
            int sWidth = targetResolution;

            Vector3 oldSize = terrain.terrainData.size;
            terrain.terrainData.heightmapResolution = targetResolution;
            terrain.terrainData.size = oldSize;

            oldHeightmap.filterMode = FilterMode.Bilinear;

            float k = (dWidth - 1.0f) / (sWidth - 1.0f) / dWidth;
            float scaleX = (sWidth * k);
            float offsetX = (float)(0.5 / dWidth - 0.5 * k);
            Vector2 scale = new Vector2(scaleX, scaleX);
            Vector2 offset = new Vector2(offsetX, offsetX);

            Graphics.Blit(oldHeightmap, terrain.terrainData.heightmapTexture, scale, offset);
            RenderTexture.ReleaseTemporary(oldHeightmap);

            oldHoles.filterMode = FilterMode.Point;
            RenderTexture newHoles = RenderTexture.GetTemporary(terrain.terrainData.holesTexture.width, terrain.terrainData.holesTexture.height, 0, Terrain.holesRenderTextureFormat);
            Graphics.Blit(oldHoles, newHoles);
            Graphics.CopyTexture(newHoles, terrain.terrainData.holesTexture);
            RenderTexture.ReleaseTemporary(oldHoles);
            RenderTexture.ReleaseTemporary(newHoles);

            RenderTexture.active = currentRT;

            terrain.terrainData.DirtyHeightmapRegion(new RectInt(0, 0, terrain.terrainData.heightmapTexture.width, terrain.terrainData.heightmapTexture.height), TerrainHeightmapSyncControl.HeightAndLod);
            terrain.terrainData.DirtyTextureRegion(TerrainData.HolesTextureName, new RectInt(0, 0, terrain.terrainData.holesTexture.width, terrain.terrainData.holesTexture.height), false);

        }

        /// <summary>
        /// Averages the heightmaps between two unity terrains so that they align at the seam.
        /// </summary>
        /// <param name="terrain1">The first terrain.</param>
        /// <param name="terrain2">The second terrain.</param>
        /// <param name="extraSeamSize">Extra seam in heightmap pixels where the heights are averaged out to better disguise the seam.</param>
        /// <param name="maxDifference">A maximum difference in height for which align pixels to - if no pixes are within the tolerance, no action will be performed at all. This can be used to ensure the terrains somewhat match up before stitching them</param>
        public static void StitchTerrainHeightmaps(Terrain terrain1, Terrain terrain2, int extraSeamSize = 1, float maxDifference = 1f)
        {
            StitchDirection stitchDirection = StitchDirection.North;

            //What is larger, the difference on x or z axis?
            if (Mathf.Abs(terrain1.transform.position.x - terrain2.transform.position.x) > Mathf.Abs(terrain1.transform.position.z - terrain2.transform.position.z))
            {
                if (terrain1.transform.position.x > terrain2.transform.position.x)
                {
                    stitchDirection = StitchDirection.West;
                }
                else
                {
                    stitchDirection = StitchDirection.East;
                }
            }
            else
            {
                if (terrain1.transform.position.z > terrain2.transform.position.z)
                {
                    stitchDirection = StitchDirection.South;
                }
                else
                {
                    stitchDirection = StitchDirection.North;
                }
            }


            int terrain1XBase = 0, terrain1YBase = 0;
            int terrain2XBase = 0, terrain2YBase = 0;
            int seamWidth = 0, seamHeight = 0;
            int seamDiameter = extraSeamSize + 1;

            float[,] terrain1Heights = new float[0, 0];
            float[,] terrain2Heights = new float[0, 0]; 

            switch (stitchDirection)
            {
                case StitchDirection.North:
                    terrain1XBase = Mathf.RoundToInt(Mathf.Max(0, terrain2.transform.position.x - terrain1.transform.position.x) / terrain1.terrainData.heightmapScale.x);
                    terrain1YBase = terrain1.terrainData.heightmapResolution - 1 - extraSeamSize;
                    seamWidth = terrain1.terrainData.heightmapResolution - Mathf.RoundToInt(Mathf.Abs(terrain1.transform.position.x - terrain2.transform.position.x) / terrain1.terrainData.heightmapScale.x);
                    seamHeight = seamDiameter;
                    terrain1Heights = terrain1.terrainData.GetHeights(terrain1XBase, terrain1YBase, seamWidth, seamHeight);

                    terrain2XBase = Mathf.RoundToInt(Mathf.Max(0, terrain1.transform.position.x - terrain2.transform.position.x) / terrain2.terrainData.heightmapScale.x);
                    terrain2YBase = 0;
                    terrain2Heights = terrain2.terrainData.GetHeights(terrain2XBase, terrain2YBase, seamWidth, seamHeight);

                    StitchBordersWithSeam(stitchDirection, seamWidth, extraSeamSize, ref terrain1Heights, ref terrain2Heights);
                    break;
                case StitchDirection.South:
                    terrain1XBase = Mathf.RoundToInt(Mathf.Max(0, terrain1.transform.position.x - terrain2.transform.position.x) / terrain2.terrainData.heightmapScale.x);
                    terrain1YBase = 0;
                    seamWidth = terrain1.terrainData.heightmapResolution - Mathf.RoundToInt(Mathf.Abs(terrain1.transform.position.x - terrain2.transform.position.x) / terrain1.terrainData.heightmapScale.x);
                    seamHeight = seamDiameter;
                    terrain1Heights = terrain1.terrainData.GetHeights(terrain1XBase, terrain1YBase, seamWidth, seamHeight);
                    
                    terrain2XBase = Mathf.RoundToInt(Mathf.Max(0, terrain2.transform.position.x - terrain1.transform.position.x) / terrain1.terrainData.heightmapScale.x);
                    terrain2YBase = terrain1.terrainData.heightmapResolution - 1 - extraSeamSize;
                    terrain2Heights = terrain2.terrainData.GetHeights(terrain2XBase, terrain2YBase, seamWidth, seamDiameter);
                    StitchBordersWithSeam(stitchDirection, seamWidth, extraSeamSize, ref terrain2Heights, ref terrain1Heights);
                    break;
                case StitchDirection.West:
                    terrain1XBase = 0;
                    terrain1YBase = Mathf.RoundToInt(Mathf.Max(0, terrain2.transform.position.z - terrain1.transform.position.z) / terrain1.terrainData.heightmapScale.z);
                    seamWidth = seamDiameter;
                    seamHeight = terrain1.terrainData.heightmapResolution - Mathf.RoundToInt(Mathf.Abs(terrain1.transform.position.z - terrain2.transform.position.z) / terrain1.terrainData.heightmapScale.z);
                    terrain1Heights = terrain1.terrainData.GetHeights(terrain1XBase, terrain1YBase, seamWidth, seamHeight);

                    terrain2XBase = terrain2.terrainData.heightmapResolution - 1 - extraSeamSize; 
                    terrain2YBase = Mathf.RoundToInt(Mathf.Max(0, terrain1.transform.position.z - terrain2.transform.position.z) / terrain2.terrainData.heightmapScale.z);
                    terrain2Heights = terrain2.terrainData.GetHeights(terrain2XBase, terrain2YBase, seamWidth, seamHeight);
                    StitchBordersWithSeam(stitchDirection, seamHeight, extraSeamSize, ref terrain2Heights, ref terrain1Heights);
                    break;
                case StitchDirection.East:
                    terrain1XBase = terrain2.terrainData.heightmapResolution - 1 - extraSeamSize;
                    terrain1YBase = Mathf.RoundToInt(Mathf.Max(0, terrain1.transform.position.z - terrain2.transform.position.z) / terrain2.terrainData.heightmapScale.z);
                    seamWidth = seamDiameter;
                    seamHeight = terrain1.terrainData.heightmapResolution - Mathf.RoundToInt(Mathf.Abs(terrain1.transform.position.z - terrain2.transform.position.z) / terrain1.terrainData.heightmapScale.z);
                    terrain1Heights = terrain1.terrainData.GetHeights(terrain1XBase, terrain1YBase, seamWidth, seamHeight);

                    terrain2XBase = 0;
                    terrain2YBase = Mathf.RoundToInt(Mathf.Max(0, terrain2.transform.position.z - terrain1.transform.position.z) / terrain1.terrainData.heightmapScale.z);
                    terrain2Heights = terrain2.terrainData.GetHeights(terrain2XBase, terrain2YBase, seamWidth, seamHeight);

                    StitchBordersWithSeam(stitchDirection, seamHeight, extraSeamSize, ref terrain1Heights, ref terrain2Heights);
                    break;
            }
            terrain1.terrainData.SetHeights(terrain1XBase, terrain1YBase, terrain1Heights);
            terrain2.terrainData.SetHeights(terrain2XBase, terrain2YBase, terrain2Heights);
            
            //bool wasStitched = AverageHeightPixels(terrain1, terrain2, terrain1XBase, terrain1YBase, terrain2XBase, terrain2YBase, seamWidth, seamHeight, 1, 1, true, true, maxDifference);

            //if (extraSeamSize > 0 && wasStitched)
            //{
            //    for (int s = 1; s <= extraSeamSize; s++)
            //    {
            //        float terrain1Weight = Mathf.InverseLerp(0, extraSeamSize, s);
            //        float terrain2Weight = Mathf.InverseLerp(extraSeamSize, 0, s);

            //        switch (stitchDirection)
            //        {
            //            case StitchDirection.North:
            //                AverageHeightPixels(terrain1, terrain1, terrain1XBase, terrain1YBase - s, terrain1XBase, terrain1YBase, seamWidth, seamHeight, terrain1Weight, terrain2Weight, true, false, 1);
            //                AverageHeightPixels(terrain2, terrain2, terrain2XBase, terrain2YBase + s, terrain2XBase, terrain2YBase, seamWidth, seamHeight, terrain1Weight, terrain2Weight, true, false, 1);
            //                break;
            //            case StitchDirection.South:
            //                AverageHeightPixels(terrain1, terrain1, terrain1XBase, terrain1YBase + s, terrain1XBase, terrain1YBase, seamWidth, seamHeight, terrain1Weight, terrain2Weight, true, false, 1);
            //                AverageHeightPixels(terrain2, terrain2, terrain2XBase, terrain2YBase - s, terrain2XBase, terrain2YBase, seamWidth, seamHeight, terrain1Weight, terrain2Weight, true, false, 1);
            //                break;
            //            case StitchDirection.West:
            //                AverageHeightPixels(terrain1, terrain1, terrain1XBase + s, terrain1YBase, terrain1XBase, terrain1YBase, seamWidth, seamHeight, terrain1Weight, terrain2Weight, true, false, 1);
            //                AverageHeightPixels(terrain2, terrain2, terrain2XBase - s, terrain2YBase, terrain2XBase, terrain2YBase, seamWidth, seamHeight, terrain1Weight, terrain2Weight, true, false, 1);
            //                break;
            //            case StitchDirection.East:
            //                AverageHeightPixels(terrain1, terrain1, terrain1XBase - s, terrain1YBase, terrain1XBase, terrain1YBase, seamWidth, seamHeight, terrain1Weight, terrain2Weight, true, false, 1);
            //                AverageHeightPixels(terrain2, terrain2, terrain2XBase + s, terrain2YBase, terrain2XBase, terrain2YBase, seamWidth, seamHeight, terrain1Weight, terrain2Weight, true, false, 1);
            //                break;
            //        }

            //    }

            //}
        }

        private static void StitchBordersWithSeam(StitchDirection stitchDirection, int seamLength, int extraSeamSize, ref float[,] terrain1Heights, ref float[,] terrain2Heights)
        {
            for (int dimension1 = 0; dimension1 < seamLength; dimension1++)
            {
                float terrain1EndHeight = 0f;
                float terrain2EndHeight = 0f;

                bool isHorizontalSeam = stitchDirection == StitchDirection.North || stitchDirection == StitchDirection.South;

                if (isHorizontalSeam)
                {
                    terrain1EndHeight = terrain1Heights[0, dimension1];
                    terrain2EndHeight = terrain2Heights[extraSeamSize, dimension1];
                }
                else
                {
                    terrain1EndHeight = terrain1Heights[dimension1, 0];
                    terrain2EndHeight = terrain2Heights[dimension1, extraSeamSize];
                }

                //float[] oldPoints = terrain1Heights[dimension1,];

                //if (terrain1EndHeight > 0 || terrain2EndHeight > 0)
                //{
                //    string message = "BEFORE:";
                //    message = "\r\nTerrain 1 End: " + terrain1EndHeight.ToString();
                //    message += "\r\nTerrain 2 End: " + terrain2EndHeight.ToString();
                //    message += "\r\n";
                //    for (int dimension2 = 1; dimension2 < extraSeamSize * 2; dimension2++)
                //    {
                //        if (dimension2 <= extraSeamSize)
                //        {
                //            message += "\r\n" + dimension2.ToString() + ": " + terrain1Heights[dimension2, dimension1].ToString();
                //        }
                //        else
                //        {
                //            message += "\r\n" + dimension2.ToString() + ": " + terrain2Heights[dimension2 - extraSeamSize, dimension1].ToString();
                //        }
                //    }
                //    Debug.Log(message);
                //}

                for (int dimension2 = 1; dimension2 < extraSeamSize * 2; dimension2++)
                {
                    float linearHeight = Mathf.Lerp(terrain1EndHeight, terrain2EndHeight, Mathf.InverseLerp(1, extraSeamSize * 2 - 1, dimension2));

                    if (dimension2 == extraSeamSize)
                    {
                        //Do not process the actual seam between the two terrains, this will be done after the rest of the points have been processed
                    }
                    else
                    {
                        if (dimension2 < extraSeamSize)
                        {
                            if (isHorizontalSeam)
                            {
                                terrain1Heights[dimension2, dimension1] = Mathf.Lerp(terrain1Heights[dimension2, dimension1], linearHeight, Mathf.InverseLerp(0, extraSeamSize, dimension2));
                            }
                            else
                            {
                                terrain1Heights[dimension1, dimension2] = Mathf.Lerp(terrain1Heights[dimension1, dimension2], linearHeight, Mathf.InverseLerp(0, extraSeamSize, dimension2));
                            }
                        }
                        else
                        {
                            if (isHorizontalSeam)
                            {
                                terrain2Heights[dimension2 - extraSeamSize, dimension1] = Mathf.Lerp(linearHeight, terrain2Heights[dimension2 - extraSeamSize, dimension1], Mathf.InverseLerp(0, extraSeamSize, dimension2 - extraSeamSize));
                            }
                            else
                            {
                                terrain2Heights[dimension1, dimension2-extraSeamSize] = Mathf.Lerp(linearHeight, terrain2Heights[dimension1, dimension2 - extraSeamSize], Mathf.InverseLerp(0, extraSeamSize, dimension2 - extraSeamSize));
                            }
                        }
                    }
                }
                //Now do the actual seam between the two points based on the changed data
                //just do the average between the closest points at the actual seam - this gives the best results without any visible bends between two terrains
                if (isHorizontalSeam)
                {
                    terrain1Heights[extraSeamSize, dimension1] = (terrain1Heights[extraSeamSize - 1, dimension1] + terrain2Heights[extraSeamSize + 1 - extraSeamSize, dimension1]) / 2f;
                    terrain2Heights[0, dimension1] = terrain1Heights[extraSeamSize, dimension1];
                }
                else
                {
                    terrain1Heights[dimension1, extraSeamSize] = (terrain1Heights[dimension1, extraSeamSize - 1] + terrain2Heights[dimension1, extraSeamSize + 1 - extraSeamSize]) / 2f;
                    terrain2Heights[dimension1, 0] = terrain1Heights[dimension1, extraSeamSize];
                }

                //if (terrain1EndHeight > 0 || terrain2EndHeight > 0)
                //{
                //    string message = "AFTER:";
                //    message = "\r\nTerrain 1 End: " + terrain1EndHeight.ToString();
                //    message += "\r\nTerrain 2 End: " + terrain2EndHeight.ToString();
                //    message += "\r\n";
                //    for (int dimension2 = 1; dimension2 < extraSeamSize * 2; dimension2++)
                //    {
                //        if (dimension2 <= extraSeamSize)
                //        {
                //            message += "\r\n" + dimension2.ToString() + ": " + terrain1Heights[dimension2, dimension1].ToString();
                //        }
                //        else
                //        {
                //            message += "\r\n" + dimension2.ToString() + ": " + terrain2Heights[dimension2 - extraSeamSize, dimension1].ToString();
                //        }
                //    }
                //    Debug.Log(message);
                //    Debug.Log("################################################");
                //}
            }
        }
        

        public static Terrain GetTerrainNeighbor(Terrain terrain, StitchDirection direction)
        {
            Bounds boundsOriginTerrain = GetWorldSpaceBounds(terrain);
            Terrain neighbor = null;
            foreach (Terrain t in Terrain.activeTerrains)
            {
                Bounds boundsTerrain = GetWorldSpaceBounds(t);
                if (boundsTerrain.Intersects(boundsOriginTerrain))
                {
                    switch (direction)
                    {
                        case StitchDirection.North:
                            if (boundsOriginTerrain.max.z == boundsTerrain.min.z && boundsTerrain.max.x > boundsOriginTerrain.min.x && boundsTerrain.min.x < boundsOriginTerrain.max.x)
                            {
                                neighbor = t;
                            }
                            break;
                        case StitchDirection.South:
                            if (boundsOriginTerrain.min.z == boundsTerrain.max.z && boundsTerrain.max.x > boundsOriginTerrain.min.x && boundsTerrain.min.x < boundsOriginTerrain.max.x)
                            {
                                neighbor = t;
                            }
                            break;
                        case StitchDirection.East:
                            if (boundsOriginTerrain.max.x == boundsTerrain.min.x && boundsTerrain.max.z > boundsOriginTerrain.min.z && boundsTerrain.min.z < boundsOriginTerrain.max.z)
                            {
                                neighbor = t;
                            }
                            break;
                        case StitchDirection.West:
                            if (boundsOriginTerrain.min.x == boundsTerrain.max.x && boundsTerrain.max.z > boundsOriginTerrain.min.z && boundsTerrain.min.z < boundsOriginTerrain.max.z)
                            {
                                neighbor = t;
                            }
                            break;
                    }
                    if (neighbor != null)
                    { break; }
                }
            }
            return neighbor;
        }

        private static bool AverageHeightPixels(Terrain terrain1, Terrain terrain2, int terrain1XBase, int terrain1YBase, int terrain2XBase, int terrain2YBase, int seamWidth, int seamHeight, float terrain1Weight, float terrain2Weight, bool applyTo1, bool applyTo2, float maxDifference)
        {
            float[,] terrain1Heights = terrain1.terrainData.GetHeights(terrain1XBase, terrain1YBase, seamWidth, seamHeight);
            float[,] terrain2Heights = terrain2.terrainData.GetHeights(terrain2XBase, terrain2YBase, seamWidth, seamHeight);
            float[,] avgheights = new float[seamHeight, seamWidth];

            bool withinMaxDistance = false;

            for (int x = 0; x < avgheights.GetLength(0); x++)
            {
                for (int y = 0; y < avgheights.GetLength(1); y++)
                {
                    if (Mathf.Abs(terrain1Heights[x, y] - terrain2Heights[x, y]) < maxDifference)
                    {
                        withinMaxDistance = true;
                    }
                    avgheights[x, y] = (terrain1Heights[x, y] * terrain1Weight + terrain2Heights[x, y] * terrain2Weight) / (terrain1Weight + terrain2Weight);
                    //avgheights[x, y] = Mathf.Lerp(terrain1Heights[x, y], terrain2Heights[x, y], terrain2Weight);
                    //avgheights[x,y] = Mathf.Min(terrain1Heights[x,y], terrain2Heights[x,y]);
                }
            }
            if (withinMaxDistance)
            {
                if (applyTo1)
                {
                    terrain1.terrainData.SetHeights(terrain1XBase, terrain1YBase, avgheights);
                }
                if (applyTo2)
                {
                    terrain2.terrainData.SetHeights(terrain2XBase, terrain2YBase, avgheights);
                }
            }
            return withinMaxDistance;
        }

        /// <summary>
        /// Resizes the terrain details to a new resolution setting.
        /// </summary>
        /// <param name="terrainData"></param>
        /// <param name="targetDetailRes"></param>
        /// <param name="resolutionPerPatch"></param>
        public static void ResizeTerrainDetails(Terrain terrain, int targetDetailRes, int resolutionPerPatch)
        {
            TerrainData terrainData = terrain.terrainData;

            if (targetDetailRes == terrainData.detailResolution)
            {
                var layers = new List<int[,]>();
                for (int i = 0; i < terrainData.detailPrototypes.Length; i++)
                    layers.Add(terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, i));

                terrainData.SetDetailResolution(targetDetailRes, resolutionPerPatch);

                for (int i = 0; i < layers.Count; i++)
                    terrainData.SetDetailLayer(0, 0, i, layers[i]);
            }
            else
            {
                terrainData.SetDetailResolution(targetDetailRes, resolutionPerPatch);
            }
        }

        /// <summary>
        /// Get the range from the terrain
        /// </summary>
        /// <returns></returns>
        public static float GetRangeFromTerrain()
        {
            Terrain t = GetActiveTerrain();
            if (t != null)
            {
                return Mathf.Max(t.terrainData.size.x, t.terrainData.size.z) / 2f;
            }
            return 0f;
        }


    }
}
