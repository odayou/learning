#if ENABLE_CLOUD_FEATURES
using System;
using System.Collections.Generic;

using UnityEngine;

using UnityEditor.Cloud.Foundation.Metadata;

namespace UnityEditor.Cloud.Foundation
{
    /// <summary>Interface for specifying package initialization information</summary>
    public interface CloudPackageInitializationBase
    {
        /// <summary>Package name property</summary>
        /// <value>The name of the package</value>
        string PackageName { get; }
        /// <summary>The loader full type name for this package</summary>
        /// <value>Loader fulltype name</value>
        string LoaderFullTypeName { get; }
        /// <summary>The loader type name for this package</summary>
        /// <value>Loader type name</value>
        string LoaderTypeName { get; }
        /// <summary>The settings full type name for this package</summary>
        /// <value>Settings full type name</value>
        string SettingsFullTypeName { get; }
        /// <summary>The settings type name for this package</summary>
        /// <value>Settings type name</value>
        string SettingsTypeName { get; }
        /// <summary>Package initialization key</summary>
        /// <value>The init key for the package</value>
        string PackageInitKey { get; }

        /// <summary>Initialize package settings</summary>
        /// <param name="obj">The scriptable object instance to initialize</param>
        /// <returns>True if successful, false if not.</returns>
        bool PopulateSettingsOnInitialization(ScriptableObject obj);
    }


    [InitializeOnLoad]
    class CloudPackageInitializationBootstrap
    {
        static CloudPackageInitializationBootstrap()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode && !Application.isBatchMode)
            {
                EditorApplication.update += BeginPackageInitialization;
            }
            else
            {
                BeginPackageInitialization();
            }

            // EditorApplication.playModeStateChanged -= PlayModeStateChanged;
            // EditorApplication.playModeStateChanged += PlayModeStateChanged;
        }

        // private static void PlayModeStateChanged(PlayModeStateChange state)
        // {
        //     switch (state)
        //     {
        //         case PlayModeStateChange.EnteredPlayMode:
        //             BeginPackageInitialization();
        //             break;

        //         case PlayModeStateChange.EnteredEditMode:
        //             BeginPackageInitialization();
        //             break;
        //     }
        // }


        internal static void BeginPackageInitialization()
        {
            EditorApplication.update -= BeginPackageInitialization;

            foreach (var t in TypeLoaderExtensions.GetAllTypesWithInterface<ICloudPackage>())
            {
                if (t.IsInterface || t.FullName.Contains("UnityEditor.Cloud.Foundation.Metadata.KnownPackages"))
                    continue;

                ICloudPackage package = Activator.CreateInstance(t) as ICloudPackage;
                if (package == null)
                {
                    Debug.LogError($"Unable to find an implementation for expected package type {t.FullName}.");
                    continue;
                }
                InitPackage(package);
            }

            foreach (var t in TypeLoaderExtensions.GetAllTypesWithInterface<CloudPackageInitializationBase>())
            {
                if (t.IsInterface)
                    continue;

                CloudPackageInitializationBase packageInit = Activator.CreateInstance(t) as CloudPackageInitializationBase;
                if (packageInit == null)
                {
                    Debug.LogError($"Unable to find an implementation for expected package type {t.FullName}.");
                    continue;
                }
                InitPackage(packageInit);
            }

            if (CloudSettingsManager.Instance != null) CloudSettingsManager.Instance.ResetUi = true;
        }

        internal static void InitPackage(ICloudPackage package)
        {
            var packageMetadata = package.metadata;
            if (packageMetadata == null)
            {
                Debug.LogError($"Package {package.GetType().Name} has a package definition but has no metadata. Skipping initialization.");
                return;
            }

            CloudPackageMetadataStore.AddPluginPackage(package);

            if (!InitializePackageFromMetadata(package, packageMetadata))
            {
                Debug.LogWarning(
                    String.Format("{0} package Initialization not completed. You will need to create any instances of the loaders and settings manually before you can use the intended Cloud Plug-in Package.", packageMetadata.packageName));
            }

        }

        static bool InitializePackageFromMetadata(ICloudPackage package, ICloudPackageMetadata packageMetadata)
        {
            bool ret = true;
            ret = ret && InitializeLoaderFromMetadata(packageMetadata.packageName, packageMetadata.loaderMetadata);
            ret = ret && InitializeSettingsFromMetadata(package, packageMetadata.packageName, packageMetadata.settingsType);
            return ret;
        }

        static bool InitializeLoaderFromMetadata(string packageName, List<ICloudLoaderMetadata> loaderMetadatas)
        {
            if (String.IsNullOrEmpty(packageName))
                return false;

            if (loaderMetadatas == null || loaderMetadatas.Count == 0)
            {
                Debug.LogWarning($"Package {packageName} has no loader metadata. Skipping loader initialization.");
                return true;
            }

            bool ret = true;
            foreach (var loader in loaderMetadatas)
            {
                bool hasInstance = EditorUtilities.AssetDatabaseHasInstanceOfType(loader.loaderType);

                if (!hasInstance)
                {
                    var obj = EditorUtilities.CreateScriptableObjectInstance(loader.loaderType,
                        EditorUtilities.GetAssetPathForComponents(EditorUtilities.s_DefaultLoaderPath));
                    hasInstance = (obj != null);
                    if (!hasInstance)
                    {
                        Debug.LogError($"Error creating instance of loader {loader.loaderName} for package {packageName}");
                    }
                }

                ret |= hasInstance;
            }

            return ret;

        }

        static bool InitializeSettingsFromMetadata(ICloudPackage package, string packageName, string settingsType)
        {
            if (String.IsNullOrEmpty(packageName))
                return false;

            if (settingsType == null)
            {
                Debug.LogWarning($"Package {packageName} has no settings metadata. Skipping settings initialization.");
                return true;
            }

            bool ret = EditorUtilities.AssetDatabaseHasInstanceOfType(settingsType);

            if (!ret)
            {
                var obj = EditorUtilities.CreateScriptableObjectInstance( settingsType,
                    EditorUtilities.GetAssetPathForComponents(EditorUtilities.s_DefaultSettingsPath));
                ret = package.PopulateNewSettingsInstance(obj);
            }

            return ret;
        }

        static void InitPackage(CloudPackageInitializationBase packageInit)
        {
            if (!InitializeLoaderInstance(packageInit))
            {
                Debug.LogWarning(
                    String.Format("{0} Loader Initialization not completed. You will need to create an instance of the loader manually before you can use the intended Cloud Plug-in Package.", packageInit.PackageName));
            }

            if (!InitializeSettingsInstance(packageInit))
            {
                Debug.LogWarning(
                    String.Format("{0} Settings Initialization not completed. You will need to create an instance of settings to customize options specific to this package.", packageInit.PackageName));
            }
        }

        static bool InitializeLoaderInstance(CloudPackageInitializationBase packageInit)
        {
            bool ret = EditorUtilities.AssetDatabaseHasInstanceOfType(packageInit.LoaderTypeName);

            if (!ret)
            {
                var obj = EditorUtilities.CreateScriptableObjectInstance(packageInit.LoaderFullTypeName,
                    EditorUtilities.GetAssetPathForComponents(EditorUtilities.s_DefaultLoaderPath));
                ret = (obj != null);
            }

            return ret;
        }

        static bool InitializeSettingsInstance(CloudPackageInitializationBase packageInit)
        {
            bool ret = EditorUtilities.AssetDatabaseHasInstanceOfType(packageInit.SettingsTypeName);

            if (!ret)
            {
                var obj = EditorUtilities.CreateScriptableObjectInstance(packageInit.SettingsFullTypeName,
                    EditorUtilities.GetAssetPathForComponents(EditorUtilities.s_DefaultSettingsPath));
                ret = packageInit.PopulateSettingsOnInitialization(obj);
            }

            return ret;
        }

    }
}
#endif