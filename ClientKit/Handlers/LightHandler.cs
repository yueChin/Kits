using UnityEngine;

namespace Kits.ClientKit.Handlers
{
    public static class LightHandler
    {
        /// <summary>
        /// Get the main directional light in the scene
        /// </summary>
        /// <returns>Main light or null</returns>
        public static Light GetMainDirectionalLight(bool create = true)
        {
            GameObject lightObject = GameObject.Find("Directional Light");
            Light mainSunLight = null;

            if (lightObject == null)
            {
                lightObject = GameObject.Find("Enviro Directional Light");
            }

            if (lightObject != null)
            {
                if (lightObject.activeInHierarchy)
                {
                    mainSunLight = lightObject.GetComponent<Light>();
                }
            }
            if (lightObject == null)
            {
                //Grab the first directional light we can find
                Light[] lights = GameObject.FindObjectsOfType<Light>();
                foreach (var light in lights)
                {
                    if (light.type == LightType.Directional && light.name != "Moon Light")
                    {
                        if (light.isActiveAndEnabled)
                        {
                            lightObject = light.gameObject;
                            mainSunLight = light;
                        }
                    }
                }
            }

            if (create)
            {
                if (lightObject == null)
                {
                    lightObject = new GameObject("Directional Light");
                    lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
                    Light lightSettings = lightObject.AddComponent<Light>();
                    lightSettings.type = LightType.Directional;
                    mainSunLight = lightSettings;
                    mainSunLight.shadowStrength = 0.8f;

#if HDPipeline
                //Applies HDRP defaults to the light so it renders correctly in the scene upon creaton
                if (GetActivePipeline() == EnvironmentRenderer.HighDefinition)
                {
                    Gaia.Pipeline.HDRP.GaiaHDRPRuntimeUtils.SetupDefaultHDRPSunLight(lightSettings);
                }
#endif
                }

                GameObject parentObject = GameObject.Find("Gaia Lighting");
                if (parentObject != null)
                {
                    bool isPartOfPrefab = false;
#if UNITY_EDITOR
                    isPartOfPrefab = UnityEditor.PrefabUtility.IsPartOfAnyPrefab(lightObject);
#endif
                    if (!isPartOfPrefab)
                    {
                        lightObject.transform.SetParent(parentObject.transform);
                    }
                }
            }

            return mainSunLight;
        }

        /// <summary>
        /// Gets or creates the moon object
        /// </summary>
        /// <returns></returns>
        public static Light GetMainMoonLight(bool create = true)
        {
            Light moonLight = null;
            GameObject moonObject = GameObject.Find("Moon Light");
            if (create)
            {
                if (moonObject == null)
                {
                    moonObject = new GameObject("Moon Light");
                }

                moonLight = moonObject.GetComponent<Light>();
                if (moonLight == null)
                {
                    moonLight = moonObject.AddComponent<Light>();
                }

                moonLight.type = LightType.Directional;
                moonLight.shadows = LightShadows.Soft;
                moonLight.color = ColorHandler.GetColorFromHTML("6A95CF");
                moonLight.intensity = 0f;
            }
            else
            {
                if (moonObject != null)
                {
                    moonLight = moonObject.GetComponent<Light>();
                    if (moonLight == null)
                    {
                        moonLight = moonObject.AddComponent<Light>();
                    }

                    moonLight.type = LightType.Directional;
                    moonLight.shadows = LightShadows.Soft;
                    moonLight.color = ColorHandler.GetColorFromHTML("6A95CF");
                    moonLight.intensity = 0f;
                }
            }

            return moonLight;
        }
    }
}
