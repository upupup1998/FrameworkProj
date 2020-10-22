using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework {
    public class PackSettings
    {
#if UNITY_EDITOR
        /// <summary>
        /// Simulate
        /// </summary>
        private const string simulateABKey = "simulateABKey";
        public static bool SimulateAssetBundle
        {
            get { return UnityEditor.EditorPrefs.GetBool(simulateABKey, true); }
            set { UnityEditor.EditorPrefs.SetBool(simulateABKey, value); }
        }
        /// <summary>
        /// AbPack Path
        /// </summary>
        private const string abPath = "abPath";
        public static string ABPath
        {
            get { return UnityEditor.EditorPrefs.GetString(abPath, Application.streamingAssetsPath); }
            set { UnityEditor.EditorPrefs.SetString(abPath, value); }
        }
#endif
    }
}
