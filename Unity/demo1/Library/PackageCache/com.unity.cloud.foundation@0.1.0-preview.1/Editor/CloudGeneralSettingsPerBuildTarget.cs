#if ENABLE_CLOUD_FEATURES
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEditor.Cloud.Foundation.Metadata;
using UnityEngine.Cloud.Foundation;

namespace UnityEditor.Cloud.Foundation
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
   /// <summary>Container class that holds general settings for each build target group installed in Unity.</summary>
   public class CloudGeneralSettingsPerBuildTarget : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        List<BuildTargetGroup> Keys = new List<BuildTargetGroup>();

        [SerializeField]
        List<CloudGeneralSettings> Values = new List<CloudGeneralSettings>();
        Dictionary<BuildTargetGroup, CloudGeneralSettings> Settings = new Dictionary<BuildTargetGroup, CloudGeneralSettings>();


#if UNITY_EDITOR

        static CloudGeneralSettingsPerBuildTarget()
        {
            // EditorApplication.playModeStateChanged -= PlayModeStateChanged;
            // EditorApplication.playModeStateChanged += PlayModeStateChanged;
        }

        // Simple class to give us updates when the asset database changes.
        class AssetCallbacks : AssetPostprocessor
        {
            static bool m_Upgrade = true;
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
            {
                if (m_Upgrade)
                {
                    m_Upgrade = false;
                    BeginUpgradeSettings();
                }
            }

            static void BeginUpgradeSettings()
            {
                string searchText = "t:CloudGeneralSettings";
                string[] assets = AssetDatabase.FindAssets(searchText);
                if (assets.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(assets[0]);
                    CloudGeneralSettingsUpgrade.UpgradeSettingsToPerBuildTarget(path);
                }
            }
        }

        void OnEnable()
        {
            foreach (var setting in Settings.Values)
            {
                var assignedSettings = setting.AssignedSettings;
                if (assignedSettings == null)
                    continue;

                var filteredLoaders = from ldr in assignedSettings.activeLoaders where ldr != null select ldr;
                assignedSettings.TrySetLoaders(filteredLoaders.ToList<CloudLoader>());
            }
            CloudGeneralSettings.Instance = CloudGeneralSettingsForBuildTarget(BuildTargetGroup.Standalone);
        }

        static void PlayModeStateChanged(PlayModeStateChange state)
        {
            CloudGeneralSettingsPerBuildTarget buildTargetSettings = null;
            EditorBuildSettings.TryGetConfigObject(CloudGeneralSettings.k_SettingsKey, out buildTargetSettings);
            if (buildTargetSettings == null)
                return;

            CloudGeneralSettings instance = buildTargetSettings.SettingsForBuildTarget(BuildTargetGroup.Standalone);
            if (instance == null || !instance.InitManagerOnStart)
                return;

            instance.InternalPlayModeStateChanged(state);
        }

        internal static bool ContainsLoaderForAnyBuildTarget(string loaderTypeName)
        {

            CloudGeneralSettingsPerBuildTarget buildTargetSettings = null;
            EditorBuildSettings.TryGetConfigObject(CloudGeneralSettings.k_SettingsKey, out buildTargetSettings);
            if (buildTargetSettings == null)
                return false;

            foreach (var settings in buildTargetSettings.Settings.Values)
            {
                if (CloudPackageMetadataStore.IsLoaderAssigned(settings.Manager, loaderTypeName))
                    return true;
            }

            return false;
        }
#endif

        /// <summary>Set specific settings for a given build target.</summary>
        ///
        /// <param name="targetGroup">An enum specifying which platform group this build is for.</param>
        /// <param name="settings">An instance of <see cref="CloudGeneralSettings"/> to assign for the given key.</param>
        public void SetSettingsForBuildTarget(BuildTargetGroup targetGroup, CloudGeneralSettings settings)
        {
            // Ensures the editor's "runtime instance" is the most current for standalone settings
            if (targetGroup == BuildTargetGroup.Standalone)
                CloudGeneralSettings.Instance = settings;
            Settings[targetGroup] = settings;
        }

        /// <summary>Get specific settings for a given build target.</summary>
        /// <param name="targetGroup">An enum specifying which platform group this build is for.</param>
        /// <returns>The instance of <see cref="CloudGeneralSettings"/> assigned to the key, or null if not.</returns>
        public CloudGeneralSettings SettingsForBuildTarget(BuildTargetGroup targetGroup)
        {
            CloudGeneralSettings ret = null;
            Settings.TryGetValue(targetGroup, out ret);
            return ret;
        }

        /// <summary>Serialization override.</summary>
        public void OnBeforeSerialize()
        {
            Keys.Clear();
            Values.Clear();

            foreach (var kv in Settings)
            {
                Keys.Add(kv.Key);
                Values.Add(kv.Value);
            }
        }

        /// <summary>Serialization override.</summary>
        public void OnAfterDeserialize()
        {
            Settings = new Dictionary<BuildTargetGroup, CloudGeneralSettings>();
            for (int i = 0; i < Math.Min(Keys.Count, Values.Count); i++)
            {
                Settings.Add(Keys[i], Values[i]);
            }
        }

        /// <summary>Given a build target, get the general settings container assigned to it.</summary>
        /// <param name="targetGroup">An enum specifying which platform group this build is for.</param>
        /// <returns>The instance of <see cref="CloudGeneralSettings"/> assigned to the key, or null if not.</returns>
        public static CloudGeneralSettings CloudGeneralSettingsForBuildTarget(BuildTargetGroup targetGroup)
        {
            CloudGeneralSettingsPerBuildTarget buildTargetSettings = null;
            EditorBuildSettings.TryGetConfigObject(CloudGeneralSettings.k_SettingsKey, out buildTargetSettings);
            if (buildTargetSettings == null)
                return null;

            return buildTargetSettings.SettingsForBuildTarget(targetGroup);
        }
    }
   
#if UNITY_EDITOR
    [InitializeOnLoad]
    class CloudGeneralSettingsAutoLoader
    {
        static CloudGeneralSettingsAutoLoader()
        {
            CloudGeneralSettingsPerBuildTarget buildTargetSettings = null;
            EditorBuildSettings.TryGetConfigObject(CloudGeneralSettings.k_SettingsKey, out buildTargetSettings);
            if (buildTargetSettings == null)
                return;

            CloudGeneralSettings instance = buildTargetSettings.SettingsForBuildTarget(BuildTargetGroup.Standalone);
            if (instance == null || !instance.InitManagerOnStart)
                return;
        }
    }
#endif
}
#endif