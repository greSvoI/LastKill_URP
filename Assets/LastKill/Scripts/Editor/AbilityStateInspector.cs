using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LastKill
{
    [CustomEditor(typeof(AbstractAbilityState), true)]
    public class AbilityStateInspector : Editor
    {
        SerializedProperty _script = null;
        SerializedProperty _priority = null;

        protected List<string> customProperties = new List<string>();

        protected virtual void OnEnable()
        {
            if (target == null) return;

            _priority = serializedObject.FindProperty("statePriority");
            _script = serializedObject.FindProperty("m_Script");

            customProperties.Add(_priority.name);
            customProperties.Add(_script.name);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _priority = serializedObject.FindProperty("statePriority");

            EditorGUILayout.PropertyField(_script);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.PropertyField(_priority, EditorStyles.boldFont);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            DrawUniqueProperties();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawUniqueProperties()
        {
            SerializedProperty property = serializedObject.GetIterator();
            if (property.NextVisible(true))
            {
                do
                {
                    if (!customProperties.Contains(property.name))
                        EditorGUILayout.PropertyField(property, true);

                } while (property.NextVisible(false));
            }
        }
    }
}
