using UnityEditor.SceneManagement;
using UnityEngine;

namespace Kits.ClientKit.Handlers
{
    public static class SceneHandler
    {
        /// <summary>
        /// Marks the active scene dirty.
        /// </summary>
        public static void MarkSceneDirty()
        {
            if (!Application.isPlaying)
            {
                if (!EditorSceneManager.GetActiveScene().isDirty)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
            }
        }
    }
}
