#if ENABLE_CLOUD_FEATURES
using System;

#if UNITY_EDITOR

using UnityEditor;

namespace UnityEditor.Cloud.Foundation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CloudSupportedBuildTargetAttribute : Attribute
    {
        /// <summary>
        /// String representation of <see href="https://docs.unity3d.com/ScriptReference/BuildTargetGroup.html">UnityEditor.Build.BuildTargetGroup
        /// </summary>
        public BuildTargetGroup buildTargetGroup { get; set; }

        /// <summary>
        /// Array of BuildTargets, each of which is the representation of <see href="https://docs.unity3d.com/ScriptReference/BuildTarget.html">UnityEditor.Build.BuildTarget
        /// aligned with <see cref="buildTargetGroup"/>.
        ///
        /// Currently only advisory.
        /// </summary>
        public BuildTarget[] buildTargets { get; set; }

        private CloudSupportedBuildTargetAttribute() { }

        /// <summary>Constructor for attribute. We assume that all build targets for this group will be supported.</summary>
        /// <param name="buildTargetGroup">Build Target Group that will be supported.</param>
        public CloudSupportedBuildTargetAttribute(BuildTargetGroup buildTargetGroup)
        {
            this.buildTargetGroup = buildTargetGroup;
        }

        /// <summary>Constructor for attribute</summary>
        /// <param name="buildTargetGroup">Build Target Group that will be supported.</param>
        /// <param name="buildTargets">The set of build targets of Build Target Group that will be supported.</param>
        public CloudSupportedBuildTargetAttribute(BuildTargetGroup buildTargetGroup, BuildTarget[] buildTargets)
        {
            this.buildTargetGroup = buildTargetGroup;
            this.buildTargets = buildTargets;
        }
    }
}

#endif
#endif