using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ScriptableObjectWithId))]
    public class ScriptableObjectIdEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var obj = target as ScriptableObjectWithId;

            if (!obj) return;

            GUILayout.Label($"ID: {obj.InternalID}");
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
#endif
}
