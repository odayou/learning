#if ENABLE_CLOUD_FEATURES
#if UNITY_EDITOR
using UnityEditor;

namespace UnityEngine.Cloud.Foundation
{
    /// <summary>
    /// CloudLoader interface for retrieving the Cloud PreInit library name from an CloudLoader instance
    /// </summary>
    public interface ICloudLoaderPreInit
    {
        /// <summary>
        /// Get the library name, if any, to use for Cloud PreInit.
        /// </summary>
        ///
        /// <param name="buildTarget">An enum specifying which platform this build is for.</param>
        /// <param name="buildTargetGroup">An enum specifying which platform group this build is for.</param>
        /// <returns>A string specifying the library name used for Cloud PreInit.</returns>
        string GetPreInitLibraryName(BuildTarget buildTarget, BuildTargetGroup buildTargetGroup);
    }
}
#endif
#endif