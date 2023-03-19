using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Editors {
    [CustomEditor(typeof(Leg))]
    public class LegEditor : Editor {
        private LegStatus _status = LegStatus.Manipulation;
        private Action _legProcess;
        private Vertex[] _vertecies;
        private bool _isGeneratingMesh = false;

        private void OnEnable() {
            _legProcess = ProcessManipulationStatus;
            _vertecies = Array.Empty<Vertex>();
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            serializedObject.Update();

            EditorGUILayout.Separator();

#region Set Leg status
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

            GUILayout.Label("Leg vertices status");
            var selectedStatus = (LegStatus)EditorGUILayout.EnumPopup(_status);
            SetLegStatus(selectedStatus);

            EditorGUILayout.EndHorizontal();
#endregion

            EditorGUILayout.Separator();

            if (_isGeneratingMesh) {
                if (GUILayout.Button("Save mesh")) {
                    _isGeneratingMesh = false;
                }
            } else {
                if (GUILayout.Button("Start generating mesh")) {
                    _isGeneratingMesh = true;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        void OnSceneGUI() {
            var legTarget = target as Leg;

            _legProcess?.Invoke();
        }

        // private void PositionVertexEditor(Leg legTarget) {
        //     if (_isSeleceted) {
        //         EditorGUI.BeginChangeCheck();
        //         var newVertex = Handles.PositionHandle(legTarget.Vertex + legTarget.transform.position, Quaternion.identity);

        //         if (EditorGUI.EndChangeCheck()) {
        //             Undo.RecordObject(legTarget, "Change vertex position");
        //             legTarget.Vertex = newVertex - legTarget.transform.position;
        //             serializedObject.Update();
        //         }
        //     } else {
        //         Handles.color = Color.green;
        //         var size = HandleUtility.GetHandleSize(legTarget.Vertex + legTarget.transform.position) * 0.05f;
        //         _isSeleceted = Handles.Button(legTarget.Vertex + legTarget.transform.position, Quaternion.identity, size, size, Handles.DotHandleCap);
        //     }
        // }

        private void SetLegStatus(LegStatus newStatus) {
            if (newStatus != _status) {
                _status = newStatus;

                switch (_status) {
                    case LegStatus.Selection:
                        GetVerticies();
                        _legProcess = ProcessSelectionStatus;
                        break;

                    case LegStatus.Manipulation:
                        SaveVerticies();
                        _legProcess = ProcessManipulationStatus;
                        break;
                }
            }
        }

        private void GetVerticies() {
            var legTarget = target as Leg;
            var meshVerticies = legTarget.GetVertices();
            var legVerticies = legTarget.LegVertecices == null ? Array.Empty<Vector3>() : legTarget.LegVertecices;

            _vertecies = new Vertex[meshVerticies.Length];

            for (int i = -1; ++i < meshVerticies.Length;) {
                var isFind = false;

                for (int j = -1; ++j < legVerticies.Length;) {
                    if (meshVerticies[i] == legVerticies[j]) {
                        isFind = true;
                        continue;
                    }
                }

                _vertecies[i] = new Vertex 
                { 
                    Status = isFind ? VertexStatus.Select : VertexStatus.NotSelected, 
                    Position = meshVerticies[i] + legTarget.transform.position,
                    Color = isFind ? Color.blue : Color.green
                };
            }
        }

        private void SaveVerticies() {
            var legTarget = target as Leg;
            var selectedVerticies = new List<Vector3>();

            for (int i = -1; ++i < _vertecies.Length;) {
                if (_vertecies[i].Status == VertexStatus.Select) {
                    selectedVerticies.Add(_vertecies[i].Position - legTarget.transform.position);
                }
            }

            legTarget.LegVertecices = selectedVerticies.ToArray();
        }

        private void ProcessSelectionStatus() {
            for (int i = -1; ++i < _vertecies.Length;) {
                var vertex = _vertecies[i];
                Handles.color = vertex.Color;
                var buttonSize = HandleUtility.GetHandleSize(vertex.Position) * 0.05f;

                if (Handles.Button(vertex.Position, Quaternion.identity, buttonSize, buttonSize, Handles.DotHandleCap)) {
                    if (vertex.Status == VertexStatus.Select) {
                        vertex.Color = Color.green;
                        vertex.Status = VertexStatus.NotSelected;
                    } else {
                        vertex.Color = Color.blue;
                        vertex.Status = VertexStatus.Select;
                    }

                    _vertecies[i] = vertex;
                }
            }
        }

        private void ProcessManipulationStatus() {
            var legTarget = target as Leg;

            if (legTarget.LegVertecices == null) return;

            Handles.color = Color.blue;

            for (int i = -1; ++i < legTarget.LegVertecices.Length;) {
                EditorGUI.BeginChangeCheck();

                var newPosition = Handles.FreeMoveHandle(
                    legTarget.LegVertecices[i] + legTarget.transform.position, 
                    Quaternion.identity, 
                    HandleUtility.GetHandleSize(legTarget.LegVertecices[i] + legTarget.transform.position) * 0.05f, 
                    Vector3.zero, 
                    Handles.DotHandleCap);

                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(legTarget, "Change leg vertex position");
                    legTarget.LegVertecices[i] = newPosition - legTarget.transform.position;
                    serializedObject.Update();
                }
            }
        }
    }
}
