using System;
using System.Collections;
using UnityEngine;

namespace Kits.ClientKit.Handlers.LoopSystems
{
    public static class CoroutineHandler
    {
        /// <summary>
        /// Skips frames until the givem amout of game time has passed.
        /// </summary>
        /// <param name="duration">The time in seconds to wait.</param>
        public static IEnumerator Wait(float duration)
        {
            var startTime = Time.time;

            while (Time.time - startTime < duration)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Waits for the given amout of frames.
        /// </summary>
        /// <param name="frames">The number of frames to wait.</param>
        public static IEnumerator Wait(int frames)
        {
            var startFrame = Time.frameCount;

            while (Time.frameCount - startFrame != frames)
            {
                yield return null;
            }
        }
        
        public static IEnumerator DelayedCall(Action action, float delay)
        {
            yield return CoroutineHandler.Wait(delay);
            action?.Invoke();
        }
    }
}
