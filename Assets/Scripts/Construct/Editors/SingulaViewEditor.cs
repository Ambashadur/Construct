using UnityEditor;
using UnityEngine;
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

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Pimples"));

            EditorGUILayout.Separator();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI() {
            Handles.color = Color.green;

            foreach (var kv in _singulaTarget.Pimples) {
                EditorGUI.BeginChangeCheck();
                var position = _singulaTarget.transform.TransformPoint(kv.Value.Position);

                var newPosition = Handles.FreeMoveHandle(
                    position, 
                    Quaternion.identity, 
                    HandleUtility.GetHandleSize(position) * 0.05f, 
                    Vector3.zero, 
                    Handles.DotHandleCap);

                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(_singulaTarget, "Change leg vertex position");
                    var value = kv.Value;
                    value.Position = _singulaTarget.transform.InverseTransformPoint(newPosition);
                    _singulaTarget.Pimples[kv.Key] = value;
                    serializedObject.Update();
                }
            }
        }
    }
}