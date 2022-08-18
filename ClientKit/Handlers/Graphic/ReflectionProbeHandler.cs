using UnityEngine;

namespace Kits.ClientKit.Handlers.Graphic
{
    public static class ReflectionProbeHandler
    {
        /// <summary>
        /// Configures reflections to LWRP
        /// </summary>
        public static void ConfigureReflectionProbes()
        {
            ReflectionProbe[] reflectionProbes = Object.FindObjectsOfType<ReflectionProbe>();
            if (reflectionProbes != null)
            {
                foreach(ReflectionProbe probe in reflectionProbes)
                {
                    if (probe.resolution > 512)
                    {
                        Debug.Log(probe.name + " This probes resolution is quite high and could cause performance issues in Lightweight Pipeline. Recommend lowing the resolution if you're targeting mobile platform");
                    }
                }
            }
        }
    }
}
