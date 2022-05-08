using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine.Cloud.Foundation;
using UnityEngine.Cloud.Foundation.Tests;
using UnityEngine;

namespace UnityEditor.Cloud.Foundation.Tests
{
    public class DomainReloadTests
    {
        [SerializeField] CloudManagerSettings m_Manager;

        [TearDown]
        public void TearDown()
        {
            if (m_Manager != null)
            {
                Object.DestroyImmediate(m_Manager);
                m_Manager = null;
            }
        }

        private void InitializeManager()
        {
            m_Manager = ScriptableObject.CreateInstance<CloudManagerSettings>();
            m_Manager.automaticLoading = false;

            var dl = ScriptableObject.CreateInstance(typeof(DummyLoader)) as DummyLoader;
            dl.id = 0;
            dl.shouldFail = true;
            m_Manager.TryAddLoader(dl);

            dl = ScriptableObject.CreateInstance(typeof(DummyLoader)) as DummyLoader;
            dl.id = 1;
            dl.shouldFail = false;
            m_Manager.TryAddLoader(dl);
        }

        [UnityTest]
        public IEnumerator DomainReload()
        {
            InitializeManager();

            Assert.IsNull(m_Manager.activeLoader);

            yield return m_Manager.InitializeLoader();

            Assert.IsNotNull(m_Manager.activeLoader);

            // Perform a domain reload and wait for it to complete
            UnityEditor.EditorUtility.RequestScriptReload();
            yield return new WaitForDomainReload();

            Assert.IsNotNull(m_Manager.activeLoader);

            // Ensure the active loader is the same loader
            Assert.IsTrue(m_Manager.activeLoader.GetType() == typeof(DummyLoader));
            Assert.IsTrue((m_Manager.activeLoader as DummyLoader).id == 1);

            m_Manager.DeinitializeLoader();

            Assert.IsNull(m_Manager.activeLoader);
        }
    }
}
