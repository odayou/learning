#if ENABLE_CLOUD_FEATURES
using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor.Cloud.Foundation.Metadata;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Cloud.Foundation;

namespace UnityEditor.Cloud.Foundation
{
    class CloudSettingsManager : SettingsProvider
    {
        internal static class Styles
        {
            public static readonly GUIStyle k_UrlLabelPersonal = new GUIStyle(EditorStyles.label)
            {
                name = "url-label",
                richText = true,
                normal = new GUIStyleState { textColor = new Color(8 / 255f, 8 / 255f, 252 / 255f) },
            };

            public static readonly GUIStyle k_UrlLabelProfessional = new GUIStyle(EditorStyles.label)
            {
                name = "url-label",
                richText = true,
                normal = new GUIStyleState { textColor = new Color(79 / 255f, 128 / 255f, 248 / 255f) },
            };

            public static readonly GUIStyle k_LabelWordWrap = new GUIStyle(EditorStyles.label) { wordWrap = true };
        }

        struct Content
        {
            public static readonly GUIContent k_InitializeOnStart = new GUIContent("Initialize Cloud on Startup");
            public static readonly GUIContent k_CloudConfigurationText = new GUIContent("Information about configuration, tracking and migration can be found below.");
            public static readonly GUIContent k_CloudConfigurationDocUriText = new GUIContent("View Documentation");
            public static readonly Uri k_CloudConfigurationUri = new Uri(" https://docs.unity3d.com/Manual/configuring-project-for-Cloud.html");
        }

        internal static GUIStyle GetStyle(string styleName)
        {
            GUIStyle s = GUI.skin.FindStyle(styleName) ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);
            if (s == null)
            {
                Debug.LogError("Missing built-in guistyle " + styleName);
                s = GUI.skin.box;
            }
            return s;
        }

        static string s_SettingsRootTitle = $"Project/{CloudConstants.k_CloudPluginManagement}";
        static CloudSettingsManager s_SettingsManager = null;

        internal static CloudSettingsManager Instance => s_SettingsManager;

        private bool resetUi = false;
        internal bool ResetUi
        {
            get
            {
                return resetUi;
            }
            set
            {
                resetUi = value;
                if (resetUi)
                    Repaint();
            }
        }

        SerializedObject m_SettingsWrapper;

        private Dictionary<BuildTargetGroup, CloudManagerSettingsEditor> CachedSettingsEditor = new Dictionary<BuildTargetGroup, CloudManagerSettingsEditor>();


        private BuildTargetGroup m_LastBuildTargetGroup = BuildTargetGroup.Unknown;

        static CloudGeneralSettingsPerBuildTarget currentSettings
        {
            get
            {
                CloudGeneralSettingsPerBuildTarget generalSettings = null;
                EditorBuildSettings.TryGetConfigObject(CloudGeneralSettings.k_SettingsKey, out generalSettings);
                if (generalSettings == null)
                {
                    lock(s_SettingsManager)
                    {
                        EditorBuildSettings.TryGetConfigObject(CloudGeneralSettings.k_SettingsKey, out generalSettings);
                        if (generalSettings == null)
                        {
                            string searchText = "t:CloudGeneralSettings";
                            string[] assets = AssetDatabase.FindAssets(searchText);
                            if (assets.Length > 0)
                            {
                                string path = AssetDatabase.GUIDToAssetPath(assets[0]);
                                generalSettings = AssetDatabase.LoadAssetAtPath(path, typeof(CloudGeneralSettingsPerBuildTarget)) as CloudGeneralSettingsPerBuildTarget;
                            }
                        }

                        if (generalSettings == null)
                        {
                            generalSettings = ScriptableObject.CreateInstance(typeof(CloudGeneralSettingsPerBuildTarget)) as CloudGeneralSettingsPerBuildTarget;
                            string assetPath = EditorUtilities.GetAssetPathForComponents(EditorUtilities.s_DefaultGeneralSettingsPath);
                            if (!string.IsNullOrEmpty(assetPath))
                            {
                                assetPath = Path.Combine(assetPath, "CloudGeneralSettings.asset");
                                AssetDatabase.CreateAsset(generalSettings, assetPath);
                            }
                        }

                        EditorBuildSettings.AddConfigObject(CloudGeneralSettings.k_SettingsKey, generalSettings, true);

                    }
                }
                return generalSettings;
            }
        }

        [UnityEngine.Internal.ExcludeFromDocs]
        CloudSettingsManager(string path, SettingsScope scopes = SettingsScope.Project) : base(path, scopes)
        {
        }

        [SettingsProvider]
        [UnityEngine.Internal.ExcludeFromDocs]
        static SettingsProvider Create()
        {
            if (s_SettingsManager == null)
            {
                s_SettingsManager = new CloudSettingsManager(s_SettingsRootTitle);
            }

            return s_SettingsManager;
        }

        [SettingsProviderGroup]
        [UnityEngine.Internal.ExcludeFromDocs]
        static SettingsProvider[] CreateAllChildSettingsProviders()
        {
            List<SettingsProvider> ret = new List<SettingsProvider>();
            if (s_SettingsManager != null)
            {
                var ats = TypeLoaderExtensions.GetAllTypesWithAttribute<CloudConfigurationDataAttribute>();
                foreach (var at in ats)
                {
                    if (at.FullName.Contains("Unity.Cloud.Foundation.TestPackage"))
                        continue;

                    CloudConfigurationDataAttribute Cloudbda = at.GetCustomAttributes(typeof(CloudConfigurationDataAttribute), true)[0] as CloudConfigurationDataAttribute;
                    string settingsPath = String.Format("{1}/{0}", Cloudbda.displayName, s_SettingsRootTitle);
                    var resProv = new CloudConfigurationProvider(settingsPath, Cloudbda.buildSettingsKey, at);
                    ret.Add(resProv);
                }
            }

            return ret.ToArray();
        }

        void InitEditorData(ScriptableObject settings)
        {
            if (settings != null)
            {
                m_SettingsWrapper = new SerializedObject(settings);
            }
        }

        /// <summary>See <see href="https://docs.unity3d.com/ScriptReference/SettingsProvider.html">SettingsProvider documentation</see>.</summary>
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            InitEditorData(currentSettings);
        }

        /// <summary>See <see href="https://docs.unity3d.com/ScriptReference/SettingsProvider.html">SettingsProvider documentation</see>.</summary>
        public override void OnDeactivate()
        {
            m_SettingsWrapper = null;
            CachedSettingsEditor.Clear();
        }

        private void DisplayLoaderSelectionUI()
        {
            BuildTargetGroup buildTargetGroup = EditorGUILayout.BeginBuildTargetSelectionGrouping();

            try
            {
                bool buildTargetChanged = m_LastBuildTargetGroup != buildTargetGroup;
                if (buildTargetChanged)
                    m_LastBuildTargetGroup = buildTargetGroup;

                CloudGeneralSettings settings = currentSettings.SettingsForBuildTarget(buildTargetGroup);
                if (settings == null)
                {
                    settings = ScriptableObject.CreateInstance<CloudGeneralSettings>() as CloudGeneralSettings;
                    currentSettings.SetSettingsForBuildTarget(buildTargetGroup, settings);
                    settings.name = $"{buildTargetGroup.ToString()} Settings";
                    AssetDatabase.AddObjectToAsset(settings, AssetDatabase.GetAssetOrScenePath(currentSettings));
                }

                var serializedSettingsObject = new SerializedObject(settings);
                serializedSettingsObject.Update();

                SerializedProperty initOnStart = serializedSettingsObject.FindProperty("m_InitManagerOnStart");
                EditorGUILayout.PropertyField(initOnStart, Content.k_InitializeOnStart);
                EditorGUILayout.Space();

                SerializedProperty loaderProp = serializedSettingsObject.FindProperty("m_LoaderManagerInstance");

                if (!CachedSettingsEditor.ContainsKey(buildTargetGroup))
                {
                    CachedSettingsEditor.Add(buildTargetGroup, null);
                }

                if (loaderProp.objectReferenceValue == null)
                {
                    var CloudManagerSettings = ScriptableObject.CreateInstance<CloudManagerSettings>() as CloudManagerSettings;
                    CloudManagerSettings.name = $"{buildTargetGroup.ToString()} Providers";
                    AssetDatabase.AddObjectToAsset(CloudManagerSettings, AssetDatabase.GetAssetOrScenePath(currentSettings));
                    loaderProp.objectReferenceValue = CloudManagerSettings;
                    serializedSettingsObject.ApplyModifiedProperties();
                }

                var obj = loaderProp.objectReferenceValue;

                if (obj != null)
                {
                    loaderProp.objectReferenceValue = obj;

                    if (CachedSettingsEditor[buildTargetGroup] == null)
                    {
                        CachedSettingsEditor[buildTargetGroup] = Editor.CreateEditor(obj) as CloudManagerSettingsEditor;

                        if (CachedSettingsEditor[buildTargetGroup] == null)
                        {
                            Debug.LogError("Failed to create a view for Cloud Manager Settings Instance");
                        }
                    }

                    if (CachedSettingsEditor[buildTargetGroup] != null)
                    {
                        if (ResetUi)
                        {
                            ResetUi = false;
                            CachedSettingsEditor[buildTargetGroup].Reload();
                        }

                        CachedSettingsEditor[buildTargetGroup].BuildTarget = buildTargetGroup;
                        CachedSettingsEditor[buildTargetGroup].OnInspectorGUI();
                    }
                }
                else if (obj == null)
                {
                    settings.AssignedSettings = null;
                    loaderProp.objectReferenceValue = null;
                }

                serializedSettingsObject.ApplyModifiedProperties();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error trying to display plug-in assingment UI : {ex.Message}");
            }

            EditorGUILayout.EndBuildTargetSelectionGrouping();
        }

        private void DisplayLink(GUIContent text, Uri link, int leftMargin)
        {
            var labelStyle = EditorGUIUtility.isProSkin ? Styles.k_UrlLabelProfessional : Styles.k_UrlLabelPersonal;
            var size = labelStyle.CalcSize(text);
            var uriRect = GUILayoutUtility.GetRect(text, labelStyle);
            uriRect.x += leftMargin;
            uriRect.width = size.x;
            if (GUI.Button(uriRect, text, labelStyle))
            {
                System.Diagnostics.Process.Start(link.AbsoluteUri);
            }
            EditorGUIUtility.AddCursorRect(uriRect, MouseCursor.Link);
            EditorGUI.DrawRect(new Rect(uriRect.x, uriRect.y + uriRect.height - 1, uriRect.width, 1), labelStyle.normal.textColor);
        }

        private void DisplayCloudTrackingDocumentationLink()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField(Content.k_CloudConfigurationText, Styles.k_LabelWordWrap);
                DisplayLink(Content.k_CloudConfigurationDocUriText, Content.k_CloudConfigurationUri, 2);
            }
            GUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        private void DisplayLoadOrderUi()
        {
            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(CloudPackageMetadataStore.isDoingQueueProcessing || EditorApplication.isPlaying || EditorApplication.isPaused);
            if (m_SettingsWrapper != null && m_SettingsWrapper.targetObject != null)
            {
                m_SettingsWrapper.Update();

                EditorGUILayout.Space();

                DisplayLoaderSelectionUI();

                m_SettingsWrapper.ApplyModifiedProperties();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();

        }

        /// <summary>See <see href="https://docs.unity3d.com/ScriptReference/SettingsProvider.html">SettingsProvider documentation</see>.</summary>
        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.Space();

            DisplayLoadOrderUi();
            DisplayCloudTrackingDocumentationLink();

            base.OnGUI(searchContext);
        }

    }
}
#endif