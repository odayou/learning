using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEditor;
using UnityEditor.Cloud.Foundation.Metadata;

[assembly:InternalsVisibleTo("UnityEditor.Cloud.Foundation.Tests")]
namespace Unity.Cloud.Foundation.TestPackage.Editor
{
    class TestLoaderMetadata : ICloudLoaderMetadata 
    {
        public string loaderName { get; set; }
        public string loaderType { get; set; }
        public List<BuildTargetGroup> supportedBuildTargets { get; set; }
    }

    class TestPackageMetadata : ICloudPackageMetadata
    {
        public string packageName { get; set; }
        public string packageId { get; set; }
        public string settingsType { get; set; }
        public List<ICloudLoaderMetadata> loaderMetadata { get; set; } 
    }

    static class TestMetadata
    {
        static TestPackageMetadata s_Metadata = null;

        internal static TestPackageMetadata CreateAndGetMetadata()
        {
            if (s_Metadata == null)
            {
                s_Metadata = new TestPackageMetadata();
                s_Metadata.packageName = "Test Package";
                s_Metadata.packageId = "com.unity.cloud.testpackage";
                s_Metadata.settingsType = typeof(TestSettings).FullName;

                s_Metadata.loaderMetadata = new List<ICloudLoaderMetadata>() {
                    new TestLoaderMetadata() {
                        loaderName = "Test Loader One",
                        loaderType = typeof(TestLoaderOne).FullName,
                        supportedBuildTargets = new List<BuildTargetGroup>() {
                            BuildTargetGroup.Standalone,
                            BuildTargetGroup.WSA
                        }
                    },
                    new TestLoaderMetadata() {
                        loaderName = "Test Loader Three",
                        loaderType = typeof(TestLoaderThree).FullName,
                        supportedBuildTargets = new List<BuildTargetGroup>() {
                            BuildTargetGroup.Unknown
                        }
                    },
                };
            }

            return s_Metadata;
        }
    }
}
