#if ENABLE_CLOUD_FEATURES
using System;

using UnityEngine;

namespace UnityEditor.Cloud.Foundation
{
    internal class CloudCustomLoaderUIManager
    {
        public static ICloudCustomLoaderUI GetCustomLoaderUI(string loaderTypeName, BuildTargetGroup buildTargetGroup)
        {
            ICloudCustomLoaderUI ret = null;

            var customLoaderTypes = TypeCache.GetTypesDerivedFrom(typeof(ICloudCustomLoaderUI));
            foreach (var customLoader in customLoaderTypes)
            {
                var attribs = customLoader.GetCustomAttributes(typeof(CloudCustomLoaderUIAttribute), true);
                foreach (var attrib in attribs)
                {
                    if (attrib is CloudCustomLoaderUIAttribute)
                    {
                        var customUiAttrib = attrib as CloudCustomLoaderUIAttribute;
                        if (String.Compare(loaderTypeName, customUiAttrib.loaderTypeName, true) == 0 &&
                            buildTargetGroup == customUiAttrib.buildTargetGroup)
                        {
                            if (ret != null)
                            {
                                Debug.Log($"Multiple custom ui renderers found for ({loaderTypeName}, {buildTargetGroup}). Defaulting to built-in rendering instead.");
                                return null;
                            }
                            ret = Activator.CreateInstance(customLoader) as ICloudCustomLoaderUI;
                        }
                    }
                }
            }

            return ret;
        }
    }
}
#endif
