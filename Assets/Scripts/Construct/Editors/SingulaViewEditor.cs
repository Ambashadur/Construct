using UnityEngine;
using UnityEditor;
using Construct.Views;

namespace Construct.Editors {
    [CustomEditor(typeof(SingulaView))]
    sealed public class SingulaViewEditor : Editor {
        private SingulaView _singulaTarget;

        private void OnEnable() => _singulaTarget = target as SingulaView;

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Id"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Name"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EcsEntity"));

            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Joins"));

            EditorGUILayout.Separator();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI() {
            Handles.color = Color.green;

            for (int i = -1; ++i < _singulaTarget.Joins.Length;) {
                EditorGUI.BeginChangeCheck();
                var position = _singulaTarget.transform.TransformPoint(_singulaTarget.Joins[i].Position);

                var newPosition = Handles.FreeMoveHandle(
                    position, 
                    Quaternion.identity, 
                    HandleUtility.GetHandleSize(position) * 0.05f, 
                    Vector3.zero, 
                    Handles.DotHandleCap);

                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(_singulaTarget, "Change leg vertex position");
                    _singulaTarget.Joins[i].Position = _singulaTarget.transform.InverseTransformPoint(newPosition);
                    serializedObject.Update();
                }
            }
        }
    }
}