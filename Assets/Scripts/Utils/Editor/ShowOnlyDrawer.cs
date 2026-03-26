using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    // THX to: https://discussions.unity.com/t/how-to-make-a-readonly-property-in-inspector/75448/5
    [CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
    public class ShowOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            string valueStr;

            switch (prop.propertyType)
            {
                case SerializedPropertyType.Integer:
                    valueStr = prop.intValue.ToString();
                    break;
                case SerializedPropertyType.Boolean:
                    valueStr = prop.boolValue.ToString();
                    break;
                case SerializedPropertyType.Float:
                    valueStr = prop.floatValue.ToString("0.00000");
                    break;
                case SerializedPropertyType.String:
                    valueStr = prop.stringValue;
                    break;
                case SerializedPropertyType.Generic:
                case SerializedPropertyType.Color:
                case SerializedPropertyType.ObjectReference:
                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.Enum:
                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Rect:
                case SerializedPropertyType.ArraySize:
                case SerializedPropertyType.Character:
                case SerializedPropertyType.AnimationCurve:
                case SerializedPropertyType.Bounds:
                case SerializedPropertyType.Gradient:
                case SerializedPropertyType.Quaternion:
                case SerializedPropertyType.ExposedReference:
                case SerializedPropertyType.FixedBufferSize:
                case SerializedPropertyType.Vector2Int:
                case SerializedPropertyType.Vector3Int:
                case SerializedPropertyType.RectInt:
                case SerializedPropertyType.BoundsInt:
                case SerializedPropertyType.ManagedReference:
                case SerializedPropertyType.Hash128:
                case SerializedPropertyType.RenderingLayerMask:
                default:
                    valueStr = "(not supported)";
                    break;
            }

            EditorGUI.LabelField(position, label.text, valueStr);
        }
    }
}