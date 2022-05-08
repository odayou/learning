using UnityEditor;

namespace Unity.Cloud.Foundation.TestPackage.Editor
{
    [CustomEditor(typeof(TestSettings))]
    public class TestSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Test only...");
        }
    }
}
