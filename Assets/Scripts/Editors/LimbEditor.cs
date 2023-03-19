using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Limb))]
public class LimbEditor : Editor {
    private readonly float ITEM_LIST_HEIGHT = EditorGUIUtility.singleLineHeight * 2 + 5;

    private Limb _limbTarget;
    private ReorderableList _pimpleList;

    private void OnEnable() {
        _limbTarget = target as Limb;
        _pimpleList = new ReorderableList(
            serializedObject: serializedObject, 
            elements: serializedObject.FindProperty("Pimples"), 
            draggable: true, 
            displayHeader: true, 
            displayAddButton: true, 
            displayRemoveButton: true);

        _pimpleList.drawHeaderCallback = DrawHeader;
        _pimpleList.drawElementCallback = DrawListItems; 
        _pimpleList.elementHeight = ITEM_LIST_HEIGHT;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("HasSlot"));

        if (_limbTarget.HasSlot) {
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

        for (int i = -1; ++i < _limbTarget.Pimples.Length;) {
            if (_limbTarget.Pimples[i] == null) continue;

            EditorGUI.BeginChangeCheck();
            var position = _limbTarget.transform.TransformPoint(_limbTarget.Pimples[i].Position);

            var newPosition = Handles.FreeMoveHandle(
                position, 
                Quaternion.identity, 
                HandleUtility.GetHandleSize(position) * 0.05f, 
                Vector3.zero, 
                Handles.DotHandleCap);

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(_limbTarget, "Change leg vertex position");
                _limbTarget.Pimples[i].Position = _limbTarget.transform.InverseTransformPoint(newPosition);
                serializedObject.Update();
            }
        }
    }

    private void ProcessSlot() {
        if (!_limbTarget.HasSlot) return;

        Handles.color = Color.red;
        EditorGUI.BeginChangeCheck();

        var position = _limbTarget.transform.TransformPoint(_limbTarget.Slot);

        var newPosition = Handles.FreeMoveHandle(
                position, 
                Quaternion.identity, 
                HandleUtility.GetHandleSize(position) * 0.05f, 
                Vector3.zero, 
                Handles.DotHandleCap);

        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(_limbTarget, "Change leg vertex position");
            _limbTarget.Slot = _limbTarget.transform.InverseTransformPoint(newPosition);
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

        labelRect.y += EditorGUIUtility.singleLineHeight + 5;
        propertyRect.y += EditorGUIUtility.singleLineHeight + 5;

        EditorGUI.LabelField(labelRect, "Trigger");
        EditorGUI.PropertyField(propertyRect, element.FindPropertyRelative("Trigger"), GUIContent.none);
    }
}
