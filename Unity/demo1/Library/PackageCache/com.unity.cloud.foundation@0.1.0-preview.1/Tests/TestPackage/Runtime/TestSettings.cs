using UnityEngine;
using UnityEngine.Cloud.Foundation;

namespace Unity.Cloud.Foundation.TestPackage
{
    [CloudConfigurationData("Test Settings", Constants.k_SettingsKey)]
    public class TestSettings : ScriptableObject
    {

#if !UNITY_EDITOR
        internal static TestSettings s_Settings;

        public void Awake()
        {
            s_Settings = this;
        }
#endif
    }
}
