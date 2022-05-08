#if ENABLE_CLOUD_FEATURES
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Cloud.Foundation
{
    /// <summary>General settings container used to house the instance of the active settings as well as the manager
    /// instance used to load the loaders with.
    /// </summary>
    public class CloudGeneralSettings : ScriptableObject
    {
        /// <summary>The key used to query to get the current loader settings.</summary>
        public static string k_SettingsKey = "com.unity.cloud.foundation.loader_settings";
        internal static CloudGeneralSettings s_RuntimeSettingsInstance = null;

        [SerializeField]
        internal CloudManagerSettings m_LoaderManagerInstance = null;

        [SerializeField]
        [Tooltip("Toggling this on/off will enable/disable the automatic startup of Cloud at run time.")]
        internal bool m_InitManagerOnStart = true;

        /// <summary>The current active manager used to manage Cloud lifetime.</summary>
        public CloudManagerSettings Manager
        {
            get { return m_LoaderManagerInstance; }
            set { m_LoaderManagerInstance = value; }
        }

        private CloudManagerSettings m_CloudManager = null;

#pragma warning disable 414 // Suppress warning for needed variables.
        private bool m_ProviderIntialized = false;
        private bool m_ProviderStarted = false;
#pragma warning restore 414

        /// <summary>The current settings instance.</summary>
        public static CloudGeneralSettings Instance
        {
            get
            {
                return s_RuntimeSettingsInstance;
            }
#if UNITY_EDITOR
            set
            {
                s_RuntimeSettingsInstance = value;
            }
#endif
        }

        /// <summary>The current active manager used to manage Cloud lifetime.</summary>
        public CloudManagerSettings AssignedSettings
        {
            get
            {
                return m_LoaderManagerInstance;
            }
#if UNITY_EDITOR
            set
            {
                m_LoaderManagerInstance = value;
            }
#endif
        }

        /// <summary>Used to set if the manager is activated and initialized on startup.</summary>
        public bool InitManagerOnStart
        {
            get
            {
                return m_InitManagerOnStart;
            }
            #if UNITY_EDITOR
            set
            {
                m_InitManagerOnStart = value;
            }
            #endif
        }


#if !UNITY_EDITOR
        void Awake()
        {
            Debug.Log("CloudGeneral Settings awakening...");
            s_RuntimeSettingsInstance = this;
            Application.quitting += Quit;
            DontDestroyOnLoad(s_RuntimeSettingsInstance);
        }
#endif

#if UNITY_EDITOR
        /// <summary>For internal use only.</summary>
        [System.Obsolete("Deprecating internal only API.")]
        public void InternalPauseStateChanged(PauseState state)
        {
            throw new NotImplementedException();
        }

        /// <summary>For internal use only.</summary>
        public void InternalPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingPlayMode:
                    Quit();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                case PlayModeStateChange.EnteredEditMode:
                    break;
            }
        }
#endif

        static void Quit()
        {
            CloudGeneralSettings instance = CloudGeneralSettings.Instance;
            if (instance == null)
                return;

            instance.DeInitCloudSDK();
        }

        void Start()
        {
            StartCloudSDK();
        }

        void OnDestroy()
        {
            DeInitCloudSDK();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void AttemptInitializeCloudSDKOnLoad()
        {
#if !UNITY_EDITOR
            CloudGeneralSettings instance = CloudGeneralSettings.Instance;
            if (instance == null || !instance.InitManagerOnStart)
                return;

            instance.InitCloudSDK();
#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        internal static void AttemptStartCloudSDKOnBeforeSplashScreen()
        {
#if !UNITY_EDITOR
            CloudGeneralSettings instance = CloudGeneralSettings.Instance;
            if (instance == null || !instance.InitManagerOnStart)
                return;

            instance.StartCloudSDK();
#endif
        }

        private void InitCloudSDK()
        {
            if (CloudGeneralSettings.Instance == null || CloudGeneralSettings.Instance.m_LoaderManagerInstance == null || CloudGeneralSettings.Instance.m_InitManagerOnStart == false)
                return;

            m_CloudManager = CloudGeneralSettings.Instance.m_LoaderManagerInstance;
            if (m_CloudManager == null)
            {
                Debug.LogError("Assigned GameObject for Cloud Management loading is invalid. No Cloud Providers will be automatically loaded.");
                return;
            }

            m_CloudManager.automaticLoading = false;
            m_CloudManager.automaticRunning = false;
            m_CloudManager.InitializeLoaderSync();
            m_ProviderIntialized = true;
        }

        private void StartCloudSDK()
        {
            if (m_CloudManager != null && m_CloudManager.activeLoader != null)
            {
                m_CloudManager.StartSubsystems();
                m_ProviderStarted = true;
            }
        }

        private void StopCloudSDK()
        {
            if (m_CloudManager != null && m_CloudManager.activeLoader != null)
            {
                m_CloudManager.StopSubsystems();
                m_ProviderStarted = false;
            }
        }

        private void DeInitCloudSDK()
        {
            if (m_CloudManager != null && m_CloudManager.activeLoader != null)
            {
                m_CloudManager.DeinitializeLoader();
                m_CloudManager = null;
                m_ProviderIntialized = false;
            }
        }

    }
}
#endif