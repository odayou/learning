#if ENABLE_CLOUD_FEATURES
using UnityEngine;

namespace UnityEditor.Cloud.Foundation.Metadata
{

    /// <summary>
    /// Implement this interface to provide package level information and actions.
    ///
    /// Cloud Foundation will reflect on all types in the project to find implementers
    /// of this interface. These instances are used to get information required to integrate
    /// your package with the Cloud Foundation system.
    /// </summary>
    public interface ICloudPackage
    {
        /// <summary>
        /// Returns an instance of <see cref="ICloudPackageMetadata"/>. Information will be used
        /// to allow the Cloud Foundation to provide settings and loaders through the settings UI.
        /// </summary>
        ICloudPackageMetadata metadata { get; }

        /// <summary>
        /// Allows the package to configure new settings and/or port old settings to the instance passed
        /// in.
        ///
        /// </summary>
        /// <param name="obj">ScriptableObject instance representing an instance of the settings
        /// type provided by <see cref="ICloudPackageMetadata.settingsType"/>.</param>
        /// <returns>True if the operation succeeded, false if not. If implementation is empty, just return true.</returns>
        bool PopulateNewSettingsInstance(ScriptableObject obj);
    }
}
#endif
