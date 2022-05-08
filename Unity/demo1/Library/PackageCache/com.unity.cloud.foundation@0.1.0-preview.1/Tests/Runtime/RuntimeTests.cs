using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;

using UnityEditor;
using UnityEngine.Rendering;

namespace UnityEngine.Cloud.Foundation.Tests
{
    public abstract class CloudSettingsManagerTestBase
    {
        private CloudManagerSettings m_Manager;
        public CloudManagerSettings manager => m_Manager;

        private List<CloudLoader> m_Loaders;
        public List<CloudLoader> loaders => m_Loaders;

        public int loaderCount { get; }

        public CloudSettingsManagerTestBase(int numberOfLoaders)
        {
            loaderCount = numberOfLoaders;
        }

        protected void SetupBase()
        {
            m_Manager = ScriptableObject.CreateInstance<CloudManagerSettings>();
            manager.automaticLoading = false;

            m_Loaders = new List<CloudLoader>();

            for (int i = 0; i < loaderCount; i++)
            {
                DummyLoader dl = ScriptableObject.CreateInstance(typeof(DummyLoader)) as DummyLoader;
                dl.id = i;
                dl.shouldFail = true;
                loaders.Add(dl);
                m_Manager.currentLoaders.Add(dl);
                m_Manager.registeredLoaders.Add(dl);
            }
        }

        protected void TeardownBase()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Object.DestroyImmediate(m_Manager);
            }
            else
#endif
            {
                Object.Destroy(m_Manager);
            }

            m_Manager = null;
        }
    }

    [TestFixture(0, -1)] // No loaders, should never have any results
    [TestFixture(1, -1)] // 1 loader, fails so no active loaders
    [TestFixture(1, 0)] // All others, make sure the active loader is expected loader.
    [TestFixture(2, 0)]
    [TestFixture(2, 1)]
    [TestFixture(3, 2)]
    class ManualLifetimeTests : CloudSettingsManagerTestBase
    {
        int m_LoaderIndexToWin;

        public ManualLifetimeTests(int loaderCount, int loaderIndexToWin)
            : base(loaderCount)
        {
            m_LoaderIndexToWin = loaderIndexToWin;
        }

        [SetUp]
        public void SetupCloudManagerTest()
        {
            SetupBase();

            if (loaderCount > m_LoaderIndexToWin && m_LoaderIndexToWin >= 0 && loaders[m_LoaderIndexToWin] is DummyLoader)
            {
                var dlToWin = manager.activeLoaders[m_LoaderIndexToWin] as DummyLoader;
                dlToWin.shouldFail = false;
            }
        }

        [TearDown]
        public void TeardownCloudManagerTest()
        {
            TeardownBase();
        }

        [UnityTest]
        public IEnumerator CheckActivatedLoader()
        {
            Assert.IsNotNull(manager);

            yield return manager.InitializeLoader();

            if (m_LoaderIndexToWin < 0 || m_LoaderIndexToWin >= loaders.Count)
            {
                Assert.IsNull(manager.activeLoader);
            }
            else
            {
                Assert.IsNotNull(manager.activeLoader);
                Assert.AreEqual(loaders[m_LoaderIndexToWin], manager.activeLoader);
            }

            manager.DeinitializeLoader();

            Assert.IsNull(manager.activeLoader);

            manager.TrySetLoaders(new List<CloudLoader>());
        }
    }

    [TestFixture(0)] // Test case where no loaders exist in the list
    [TestFixture(1)]
    [TestFixture(2)]
    [TestFixture(3)]
    [TestFixture(4)]
    class RuntimeActiveLoadersManipulationTests : CloudSettingsManagerTestBase
    {
        public RuntimeActiveLoadersManipulationTests(int loaderCount)
            : base(loaderCount)
        {
        }

        [SetUp]
        public void SetupRuntimeActiveLoadersManipulationTest()
        {
            SetupBase();
        }

        [TearDown]
        public void TeardownRuntimeActiveLoadersManipulationTest()
        {
            TeardownBase();
        }

        [Test]
        public void CheckIfSetLegalLoaderListWorks()
        {
            Assert.IsNotNull(manager);

            var originalLoaders = new List<CloudLoader>(manager.activeLoaders);

            // An empty loader list is valid
            Assert.True(manager.TrySetLoaders(new List<CloudLoader>()));

            // Make sure that the registered loaders hasn't been modified at all
            if (loaderCount > 0)
                Assert.IsNotEmpty(manager.registeredLoaders);
            Assert.AreEqual(manager.registeredLoaders.Count, loaderCount >= 0 ? loaderCount : 0);

            // All loaders should be registered
            Assert.True(manager.TrySetLoaders(originalLoaders));
        }

        [Test]
        public void CheckIfLegalAddSucceeds()
        {
            Assert.IsNotNull(manager);

            var originalLoaders = new List<CloudLoader>(manager.activeLoaders);

            Assert.True(manager.TrySetLoaders(new List<CloudLoader>()));

            if (loaderCount > 0)
                Assert.IsNotEmpty(manager.registeredLoaders);
            Assert.AreEqual(manager.registeredLoaders.Count, loaderCount > 0 ? loaderCount : 0);

            for (var i = 0; i < originalLoaders.Count; ++i)
            {
                Assert.True(manager.TryAddLoader(originalLoaders[originalLoaders.Count - 1 - i]));
            }

            Assert.AreEqual(manager.registeredLoaders.Count, loaderCount >= 0 ? loaderCount : 0);
        }

        [Test]
        public void CheckIfIllegalAddFails()
        {
            Assert.IsNotNull(manager);

            var dl = ScriptableObject.CreateInstance(typeof(DummyLoader)) as DummyLoader;
            dl.id = -1;
            dl.shouldFail = true;

            Assert.False(manager.TryAddLoader(dl));
        }

        [Test]
        public void CheckIfIllegalSetLoaderListFails()
        {
            Assert.IsNotNull(manager);

            var dl = ScriptableObject.CreateInstance(typeof(DummyLoader)) as DummyLoader;
            dl.id = -1;
            dl.shouldFail = true;

            var invalidList = new List<CloudLoader>(manager.activeLoaders) { dl };

            Assert.False(manager.TrySetLoaders(invalidList));

            invalidList = new List<CloudLoader> { dl };

            Assert.False(manager.TrySetLoaders(invalidList));
        }

        [Test]
        public void CheckIfRemoveAndReAddAtSameIndexWorks()
        {
            Assert.IsNotNull(manager);

            var originalList = manager.activeLoaders;

            for (var i = 0; i < originalList.Count; ++i)
            {
                var loader = originalList[i];
                Assert.True(manager.TryRemoveLoader(loader));

                if (loaderCount > 0)
                    Assert.IsNotEmpty(manager.registeredLoaders);
                Assert.AreEqual(manager.registeredLoaders.Count, loaderCount > 0 ? loaderCount : 0);

                Assert.True(manager.TryAddLoader(loader, i));

                Assert.AreEqual(originalList[i], manager.activeLoaders[i]);
            }
        }

        [Test]
        public void CheckIfAttemptToAddDuplicateLoadersFails()
        {
            Assert.IsNotNull(manager);

            var originalLoaders = manager.activeLoaders;
            foreach (var loader in originalLoaders)
            {
                Assert.False(manager.TryAddLoader(loader));
            }
        }

        [Test]
        public void CheckIfAttemptsToSetLoaderListThatContainDuplicatesFails()
        {
            Assert.IsNotNull(manager);

            if (loaderCount > 0)
            {
                var originalLoaders = manager.activeLoaders;
                var loadersWithDuplicates = new List<CloudLoader>(manager.activeLoaders);

                loadersWithDuplicates.AddRange(originalLoaders);

                Assert.False(manager.TrySetLoaders(loadersWithDuplicates));
            }
        }
    }
}
