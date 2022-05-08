using System.Runtime.CompilerServices;

using UnityEngine;

using UnityEditor.Cloud.Foundation.Metadata;

[assembly:InternalsVisibleTo("Unity.Cloud.Foundation.EditorTests")]
[assembly:InternalsVisibleTo("Unity.Cloud.Foundation.Tests.Standalone")]
namespace Unity.Cloud.Foundation.TestPackage.Editor
{
    internal class TestPackage : ICloudPackage
    {
        public TestPackage() {}

        public ICloudPackageMetadata metadata 
        { 
            get
            {
                return TestMetadata.CreateAndGetMetadata();
            }
        }
        
        public bool PopulateNewSettingsInstance(ScriptableObject obj)
        {
            TestSettings packageSettings = obj as TestSettings;
            if (packageSettings != null)
            {
                return true;
            }
            return false;
        }

    }
}
