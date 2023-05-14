using UnityEditor;
using UnityEngine;
using Construct.Views;
using Construct.Model;

namespace Construct.Editors
{
    [CustomEditor(typeof(SingulaView))]
    public sealed class SingulaViewEditor : Editor
    {
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
            Handles.color = Color.red;

            for (int i = 0; i < _singulaTarget.Pimples.Length; i++) {
                EditorGUI.BeginChangeCheck();
                var pimple = _singulaTarget.Pimples[i];
                var position = _singulaTarget.transform.TransformPoint(pimple.Position);

                var newPosition = Handles.FreeMoveHandle(
                    position,
                    Quaternion.identity,
                    HandleUtility.GetHandleSize(position) * 0.05f,
                    Vector3.zero,
                    Handles.DotHandleCap);

                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(_singulaTarget, "Change leg vertex position");
                    var value = pimple;
                    value.Position = _singulaTarget.transform.InverseTransformPoint(newPosition);
                    _singulaTarget.Pimples[i] = value;
                    serializedObject.Update();
                }
            }
        }
    }
}