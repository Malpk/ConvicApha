using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Underworld.Editors
{
    [CustomEditor(typeof(ModeSwitchController))]
    public class CastumerSequnce : Editor
    {
        private UnderWorldEditor _edit;
        private ModeSwitchController _controller;
        private void OnEnable()
        {
            _controller = (ModeSwitchController)target;
            _controller.Load();
        }
        private void OnGUI()
        {
            if (GUI.changed)
            {
                EditorUtility.SetDirty(_controller);
            }
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Open editor"))
            {
                _edit = UnderWorldEditor.Intializate();
                _edit.SetParent(_controller);
            }
            EditorGUILayout.EndVertical();
            Undo.RecordObject(_controller,"sa");
            base.OnInspectorGUI();
        }
    }
}