#if ENABLE_CLOUD_FEATURES
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace UnityEditor.Cloud.Foundation.Metadata
{
    internal class KnownPackages
    {
        internal static string k_KnownPackageMockHMDLoader = "Unity.Cloud.MockHMD.MockHMDLoader";

        class KnownLoaderMetadata : ICloudLoaderMetadata
        {
            public string loaderName { get; set; }
            public string loaderType { get; set; }
            public List<BuildTargetGroup> supportedBuildTargets { get; set; }
        }

        class KnownPackageMetadata : ICloudPackageMetadata
        {
            public string packageName { get; set; }
            public string packageId { get; set; }
            public string settingsType { get; set; }
            public List<ICloudLoaderMetadata> loaderMetadata { get; set; }
        }

        class KnownPackage : ICloudPackage
        {
            public ICloudPackageMetadata metadata { get; set; }
            public bool PopulateNewSettingsInstance(ScriptableObject obj) { return true; }
        }

        private static Lazy<List<ICloudPackage>> s_KnownPackages = new Lazy<List<ICloudPackage>>(InitKnownPackages);

        internal static List<ICloudPackage> Packages => s_KnownPackages.Value;

        static List<ICloudPackage> InitKnownPackages()
        {
            List<ICloudPackage> packages = new List<ICloudPackage>();
/*
            packages.Add(new KnownPackage() {
                metadata = new KnownPackageMetadata() {
                    packageName = "MockHMD Cloud Plugin",
                    packageId = "com.unity.cloudfoundation.mock",
                    settingsType = "Unity.Cloud.MockHMD.MockHMDBuildSettings",
                    loaderMetadata = new List<ICloudLoaderMetadata>() {
                    new KnownLoaderMetadata() {
                            loaderName = "Unity Cloud Mock",
                            loaderType = "Unity.Cloud.MockHMD.MockHMDLoader",
                            supportedBuildTargets = new List<BuildTargetGroup>() {
                                BuildTargetGroup.Standalone,
                                BuildTargetGroup.Android
                            }
                        },
                    }
                }
            });*/

            packages.Add(new KnownPackage() {
                metadata = new KnownPackageMetadata() {
                    packageName = "TencentSDK Cloud Plugin",
                    packageId = "com.unity.cloudfoundation.tencent",
                    settingsType = "Unity.Cloud.TencentSDK.TencentSDKBuildSettings",
                    loaderMetadata = new List<ICloudLoaderMetadata>() {
                    new KnownLoaderMetadata() {
                            loaderName = "Unity Cloud Tencent",
                            loaderType = "Unity.Cloud.TencentSDK.TencentSDKLoader",
                            supportedBuildTargets = new List<BuildTargetGroup>() {
                                BuildTargetGroup.Standalone,
                                BuildTargetGroup.Android
                            }
                        },
                    }
                }
            });
            return packages;
        }
    }
}
#endif