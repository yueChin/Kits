using System.Collections.Generic;
using Kits.ClientKit.Handlers.Graphic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kits.ClientKit.Handlers
{
    public static class ParticleSystemHandler
    {
        public static void SortForRendering(this List<ParticleSystem> self, UnityEngine.Transform transform, bool sortByMaterial)
        {
            self.Sort((a, b) =>
            {
                UnityEngine.Transform tr = transform;
                ParticleSystemRenderer aRenderer = a.GetComponent<ParticleSystemRenderer>();
                ParticleSystemRenderer bRenderer = b.GetComponent<ParticleSystemRenderer>();

                // Render queue: ascending
                Material aMat = aRenderer.sharedMaterial ?? aRenderer.trailMaterial;
                Material bMat = bRenderer.sharedMaterial ?? bRenderer.trailMaterial;
                if (!aMat && !bMat) return 0;
                if (!aMat) return -1;
                if (!bMat) return 1;

                if (sortByMaterial)
                    return aMat.GetInstanceID() - bMat.GetInstanceID();

                if (aMat.renderQueue != bMat.renderQueue)
                    return aMat.renderQueue - bMat.renderQueue;

                // Sorting layer: ascending
                if (aRenderer.sortingLayerID != bRenderer.sortingLayerID)
                    return aRenderer.sortingLayerID - bRenderer.sortingLayerID;

                // Sorting order: ascending
                if (aRenderer.sortingOrder != bRenderer.sortingOrder)
                    return aRenderer.sortingOrder - bRenderer.sortingOrder;

                // Z position & sortingFudge: descending
                UnityEngine.Transform aTransform = a.transform;
                UnityEngine.Transform bTransform = b.transform;
                float aPos = tr.InverseTransformPoint(aTransform.position).z + aRenderer.sortingFudge;
                float bPos = tr.InverseTransformPoint(bTransform.position).z + bRenderer.sortingFudge;
                if (!Mathf.Approximately(aPos, bPos))
                    return (int)Mathf.Sign(bPos - aPos);

                return (int)Mathf.Sign(GetIndex(self, a) - GetIndex(self, b));
            });
        }

        private static int GetIndex(IList<ParticleSystem> list, Object ps)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].GetInstanceID() == ps.GetInstanceID()) return i;
            }

            return 0;
        }

        /// <summary>
        /// Gets and returns the particle renderer material
        /// </summary>
        /// <param name="checkSelf"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public static Material GetParticleMaterial(GameObject selfObject)
        {
            Material material = null;
            if (selfObject != null)
            {
                ParticleSystemRenderer systemRenderer = selfObject.GetComponent<ParticleSystemRenderer>();
                if (systemRenderer != null)
                {
                    material = systemRenderer.sharedMaterial;
                }
            }

            return material;
        }
        
        /// <summary>
        /// Gets and returns the particle renderer material
        /// </summary>
        /// <param name="checkSelf"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public static Material GetParticleMaterial(string objectName)
        {
            Material material = null;
            if (objectName.Length > 0)
            {
                GameObject particleObject = GameObject.Find(objectName);
                if (particleObject != null)
                {
                    ParticleSystemRenderer systemRenderer = particleObject.GetComponent<ParticleSystemRenderer>();
                    if (systemRenderer != null)
                    {
                        material = systemRenderer.sharedMaterial;
                    }
                }
            }

            return material;
        }
        
        public static long GetMaterialHash(this ParticleSystem self, bool trail)
        {
            if (!self) return 0;

            ParticleSystemRenderer r = self.GetComponent<ParticleSystemRenderer>();
            Material mat = trail ? r.trailMaterial : r.sharedMaterial;

            if (!mat) return 0;

            Texture2D tex = trail ? null : self.GetTextureForSprite();
            return ((long)mat.GetHashCode() << 32) + (tex ? tex.GetHashCode() : 0);
        }

        public static Texture2D GetTextureForSprite(this ParticleSystem self)
        {
            if (!self) return null;

            // Get sprite's texture.
            ParticleSystem.TextureSheetAnimationModule tsaModule = self.textureSheetAnimation;
            if (!tsaModule.enabled || tsaModule.mode != ParticleSystemAnimationMode.Sprites) return null;

            for (int i = 0; i < tsaModule.spriteCount; i++)
            {
                Sprite sprite = tsaModule.GetSprite(i);
                if (!sprite) continue;

                return sprite.GetActualTexture();
            }

            return null;
        }

        public static void Execute(this List<ParticleSystem> self, System.Action<ParticleSystem> action)
        {
            self.RemoveAll(p => !p);
            self.ForEach(action);
        }
    }
}