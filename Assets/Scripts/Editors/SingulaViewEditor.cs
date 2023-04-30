using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Construct.Views;

namespace Construct.Editors {
    [CustomEditor(typeof(SingulaView))]
    public class SingulaViewEditor : Editor {
        private readonly float ITEM_LIST_HEIGHT = EditorGUIUtility.singleLineHeight * 2 + 5;

        private SingulaView _singulaTarget;
        private ReorderableList _pimpleList;

        private void OnEnable() {
            _singulaTarget = target as SingulaView;
            _pimpleList = new ReorderableList(
                serializedObject: serializedObject, 
                elements: serializedObject.FindProperty("Pimples"), 
                draggable: true, 
                displayHeader: true, 
                displayAddButton: true, 
                displayRemoveButton: true);

            _pimpleList.drawHeaderCallback = DrawHeader;
            _pimpleList.drawElementCallback = DrawListItems;
            _pimpleList.onAddCallback = AddItem;
            // _pimpleList.onRemoveCallback = RemoveItem;
            _pimpleList.elementHeight = ITEM_LIST_HEIGHT;
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("HasSlot"));

            if (_singulaTarget.HasSlot) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Slot"));
            }

            EditorGUILayout.Separator();

            _pimpleList.DoLayoutList();

            EditorGUILayout.Separator();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI() {
            ProcessPimples();
            ProcessSlot();
        }

        private void ProcessPimples() {
            Handles.color = Color.blue;

            for (int i = -1; ++i < _singulaTarget.Pimples.Length;) {
                if (_singulaTarget.Pimples[i] == null) continue;

                EditorGUI.BeginChangeCheck();
                var position = _singulaTarget.transform.TransformPoint(_singulaTarget.Pimples[i].Position);

                var newPosition = Handles.FreeMoveHandle(
                    position, 
                    Quaternion.identity, 
                    HandleUtility.GetHandleSize(position) * 0.05f, 
                    Vector3.zero, 
                    Handles.DotHandleCap);

                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(_singulaTarget, "Change leg vertex position");
                    _singulaTarget.Pimples[i].Position = _singulaTarget.transform.InverseTransformPoint(newPosition);
                    serializedObject.Update();
                }
            }
        }

        private void ProcessSlot() {
            if (!_singulaTarget.HasSlot) return;

            Handles.color = Color.red;
            EditorGUI.BeginChangeCheck();

            var position = _singulaTarget.transform.TransformPoint(_singulaTarget.Slot);

            var newPosition = Handles.FreeMoveHandle(
                    position, 
                    Quaternion.identity, 
                    HandleUtility.GetHandleSize(position) * 0.05f, 
                    Vector3.zero, 
                    Handles.DotHandleCap);

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(_singulaTarget, "Change leg vertex position");
                _singulaTarget.Slot = _singulaTarget.transform.InverseTransformPoint(newPosition);
                serializedObject.Update();
            }
        }

        private void DrawHeader(Rect rect) {
            const string headerName = "Pimples";
            EditorGUI.LabelField(rect, headerName);
        }

        private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused) {
            var element = _pimpleList.serializedProperty.GetArrayElementAtIndex(index);

            var labelRect = new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight);
            var propertyRect = new Rect(rect.x + labelRect.width, rect.y, rect.width - labelRect.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.LabelField(labelRect, "Position");
            EditorGUI.PropertyField(propertyRect, element.FindPropertyRelative("Position"), GUIContent.none);
        }

        private void AddItem(ReorderableList list) {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;

            var pimple = list.serializedProperty.GetArrayElementAtIndex(index);

            pimple.FindPropertyRelative("Id").intValue = index;

            var triggerPimple = new GameObject("TriggerPimple");
            triggerPimple.AddComponent<BoxCollider>();
            var triggerPimpleView = triggerPimple.AddComponent<TriggerPimpleView>();
            triggerPimple.transform.SetParent(_singulaTarget.transform);
            triggerPimple.transform.position = _singulaTarget.transform.position;

            triggerPimpleView.PimpleId = index;
        }
    }
}