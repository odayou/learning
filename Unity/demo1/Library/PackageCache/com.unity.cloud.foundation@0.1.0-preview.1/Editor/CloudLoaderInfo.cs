#if ENABLE_CLOUD_FEATURES
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.Cloud.Foundation;

namespace UnityEditor.Cloud.Foundation
{
    internal class CloudLoaderInfo : IEquatable<CloudLoaderInfo>
    {
        public Type loaderType;
        public string assetName;
        public CloudLoader instance;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CloudLoaderInfo && Equals((CloudLoaderInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (loaderType != null ? loaderType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (instance != null ? instance.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(CloudLoaderInfo other)
        {
            return other != null && Equals(loaderType, other.loaderType) && Equals(instance, other.instance);
        }

        static string[] s_LoaderBlackList = { "DummyLoader", "SampleLoader", "CloudLoaderHelper" };

        internal static void GetAllKnownLoaderInfos(List<CloudLoaderInfo> newInfos)
        {
            var loaderTypes = TypeLoaderExtensions.GetAllTypesWithInterface<CloudLoader>();
            foreach (Type loaderType in loaderTypes)
            {
                if (loaderType.IsAbstract)
                    continue;

                if (s_LoaderBlackList.Contains(loaderType.Name))
                    continue;

                var assets = AssetDatabase.FindAssets(String.Format("t:{0}", loaderType));
                if (!assets.Any())
                {
                    CloudLoaderInfo info = new CloudLoaderInfo();
                    info.loaderType = loaderType;
                    newInfos.Add(info);
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(asset);

                        CloudLoaderInfo info = new CloudLoaderInfo();
                        info.loaderType = loaderType;
                        info.instance = AssetDatabase.LoadAssetAtPath(path, loaderType) as CloudLoader;
                        info.assetName = Path.GetFileNameWithoutExtension(path);
                        newInfos.Add(info);
                    }
                }
            }
        }
    }
}
#endif