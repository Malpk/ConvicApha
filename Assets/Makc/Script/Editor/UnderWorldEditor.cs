using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

namespace Underworld.Editor
{
    public class UnderWorldEditor : EditorWindow
    {
        //private float _curretPosition = 100;
        private Button _addNewSecunce;
        private VisualElement _container;

        [MenuItem("Window/UnderWorldEditor")]
        public static void Intializate()
        {
            var window = (UnderWorldEditor)EditorWindow.GetWindow(typeof(UnderWorldEditor));
            window.Show();

            window.position = new Rect(0, 0, 1000, 1000);
        }
        private void CreateGUI()
        {
            CreateBaseInterface();
        }
        private void CreateBaseInterface()
        {
            rootVisualElement.styleSheets.Add((StyleSheet)Resources.Load("ListPanel"));
            rootVisualElement.styleSheets.Add((StyleSheet)Resources.Load("EditorViewStyle"));
            var panel = new VisualElement();
            var inspector = new VisualElement();
            var vievEdit = new DisplaySequnce();
            _container = new VisualElement();
            panel.AddToClassList("horizontal-container");
            vievEdit.AddToClassList("view-size");
            _container.AddToClassList("verical-container");
            _addNewSecunce = (Button)CreateButton("+", "button-control");
            var removeSecunce = (Button)CreateButton("-", "button-control-minus");
            inspector.AddToClassList("inspector");
            _addNewSecunce.clicked += AddSecunce;
            removeSecunce.clicked += Delete;
            rootVisualElement.Add(panel);
            panel.Add(_container);
            panel.Add(vievEdit);
            panel.Add(inspector);
            panel.Add(_addNewSecunce);
            panel.Add(removeSecunce);
        }
        public void AddSecunce()
        {
            _container.Add(CreateButton("Sequnce", "buttonSequnce"));
        }
        private void Delete()
        {
            if(_container.childCount > 0)
                _container.Remove(_container[_container.childCount - 1]);
        }
        private Button CreateButton(string name, string style = null)
        {
            var newButton = new Button();
            newButton.text = name;
            if(style != null)
                newButton.AddToClassList(style);
            return newButton;
        }

    }
}