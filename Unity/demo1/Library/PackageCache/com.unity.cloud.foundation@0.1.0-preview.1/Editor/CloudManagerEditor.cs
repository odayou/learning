#if ENABLE_CLOUD_FEATURES
using UnityEngine.Cloud.Foundation;

namespace UnityEditor.Cloud.Foundation
{
    [CustomEditor(typeof(CloudManagerSettings))]
    internal class CloudManagerSettingsEditor : Editor
    {
        CloudLoaderOrderUI m_LoaderUi = new CloudLoaderOrderUI();

        internal BuildTargetGroup BuildTarget
        {
            get;
            set;
        }

        public void Reload()
        {
            m_LoaderUi.CurrentBuildTargetGroup = BuildTargetGroup.Unknown;
        }

        /// <summary><see href="https://docs.unity3d.com/ScriptReference/Editor.OnInspectorGUI.html">Editor Documentation</see></summary>
        public override void OnInspectorGUI()
        {
            if (serializedObject == null || serializedObject.targetObject == null)
                return;

            serializedObject.Update();

            m_LoaderUi.OnGUI(BuildTarget);

            serializedObject.ApplyModifiedProperties();
        }
    }

}
#endif