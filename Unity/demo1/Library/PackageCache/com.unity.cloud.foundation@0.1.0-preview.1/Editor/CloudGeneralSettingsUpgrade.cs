#if ENABLE_CLOUD_FEATURES
using UnityEngine;
using UnityEngine.Cloud.Foundation;


namespace UnityEditor.Cloud.Foundation
{
    /// <summary>Helper class to auto update settings across versions.</summary>
    public static class CloudGeneralSettingsUpgrade
    {
        /// <summary>Worker API to do the actual upgrade</summary>
        /// <param name="path">Path to asset to upgrade</param>
        /// <returns>True if settings were successfullly upgraded, else false.</returns>
        public static bool UpgradeSettingsToPerBuildTarget(string path)
        {
            var generalSettings = GetCloudGeneralSettingsInstance(path);
            if (generalSettings == null)
                return false;

            if (!AssetDatabase.IsMainAsset(generalSettings))
                return false;

            CloudGeneralSettings newSettings = ScriptableObject.CreateInstance<CloudGeneralSettings>() as CloudGeneralSettings;
            newSettings.Manager = generalSettings.Manager;
            generalSettings = null;

            AssetDatabase.DeleteAsset(path);

            CloudGeneralSettingsPerBuildTarget buildTargetSettings = ScriptableObject.CreateInstance<CloudGeneralSettingsPerBuildTarget>() as CloudGeneralSettingsPerBuildTarget;
            AssetDatabase.CreateAsset(buildTargetSettings, path);

            buildTargetSettings.SetSettingsForBuildTarget(EditorUserBuildSettings.selectedBuildTargetGroup, newSettings);
            newSettings.name = $"{EditorUserBuildSettings.selectedBuildTargetGroup.ToString()} Settings";
            AssetDatabase.AddObjectToAsset(newSettings, path);
            AssetDatabase.SaveAssets();

            Debug.LogWarningFormat("Cloud General Settings have been upgraded to be per-Build Target Group. Original settings were moved to Build Target Group {0}.", EditorUserBuildSettings.selectedBuildTargetGroup);
            return true;
        }

        private static CloudGeneralSettings GetCloudGeneralSettingsInstance(string pathToSettings)
        {
            CloudGeneralSettings ret = null;
            if (pathToSettings.Length > 0)
            {
                ret = AssetDatabase.LoadAssetAtPath(pathToSettings, typeof(CloudGeneralSettings)) as CloudGeneralSettings;
            }

            return ret;
        }
    }

}
#endif