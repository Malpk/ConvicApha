using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Underworld.Editors
{
    [CustomEditor(typeof(ModeSwitchController))]
    public class CastumerSequnce : Editor
    {
        private static UnderWorldEditor _edit;

        private ModeSwitchController _controller;
        private void OnEnable()
        {
            _controller = (ModeSwitchController)target;
            _controller.Load();
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Open editor"))
            {
                if (_edit == null)
                {
                    _edit = UnderWorldEditor.Intializate();
                }
                if (_edit != null)
                {
                    _edit.SetParent(_controller);
                }
            }
            EditorGUILayout.EndVertical();
            base.OnInspectorGUI();
        }
    }
}