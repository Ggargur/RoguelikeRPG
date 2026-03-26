using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace Plugins.SerializedCollections.Editor.Scripts.Utility
{
    internal static class SCEnumUtility
    {
        private static Dictionary<Type, EnumCache> _cache = new Dictionary<Type, EnumCache>();

        internal static EnumCache GetEnumCache(Type enumType)
        {
            if (_cache.TryGetValue(enumType, out var val))
                return val;

            try
            {
                var classType = typeof(EditorGUI).Assembly.GetType("UnityEditor.EnumDataUtility");
                var methodInfo = classType.GetMethod("GetCachedEnumData", BindingFlags.Static | BindingFlags.NonPublic);
                var parameters = new object[] { enumType, true };
                var result = methodInfo.Invoke(null, parameters);
                var flagValues = (int[])result.GetType().GetField("flagValues").GetValue(result);
                var names = (string[])result.GetType().GetField("names").GetValue(result);
                var cache = new EnumCache(enumType, flagValues, names);
                _cache.Add(enumType, cache);
                return cache;
            }
            catch
            {
                throw;
            }
        }
    }
}