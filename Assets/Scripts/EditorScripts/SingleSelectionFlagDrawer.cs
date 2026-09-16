#if UNITY_EDITOR

using Editor;
using System;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomPropertyDrawer(typeof(SingleSelectionFlag))]
    public class SingleSelectionFlagDrawer : PropertyDrawer {

        public override void OnGUI(
            Rect position,
            SerializedProperty property,
            GUIContent label
        ) {
            EditorGUI.BeginProperty(position, label, property);

            property.intValue = Convert.ToInt32(
                EditorGUI.EnumPopup(
                    position,
                    label,
                    (Enum)Enum.ToObject(
                        fieldInfo.FieldType,
                        property.intValue
                    )
                )
            );

            EditorGUI.EndProperty();
        }
    }
}

#endif