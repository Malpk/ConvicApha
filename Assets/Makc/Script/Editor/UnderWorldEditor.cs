using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

namespace Underworld.Editor
{
    public class UnderWorldEditor : EditorWindow
    {
        private Button _addNewSecunce;
        private Button _focusButton = null;
        private Sequnce _curretSecunce;
        private Dictionary<Button,Sequnce> _listSecunce = new Dictionary<Button, Sequnce> ();
        private VisualElement _container;
        private VisualElement _inspector;
        private SeqcunceEdit _secunce = null;

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
            _secunce.SetEnabled(false);
            _secunce.UnSelectNodeAction += () => _inspector.Clear();
            _secunce.ChoiseNodeAction += OutputSetting;
        }
        private void OnDestroy()
        {
            _secunce.ChoiseNodeAction -= OutputSetting;
            _secunce.UnSelectNodeAction -= () => _inspector.Clear();
        }
        private void OutputSetting(SerializedObject setting)
        {
            _inspector.Clear();
            foreach (SerializedProperty property in setting.FindProperty("setting"))
            {
                var field = new PropertyField(property, property.name);
                field.AddToClassList("proprty-field");
                _inspector.Add(field);
                field.Bind(setting);
            }
        }
        private void CreateBaseInterface()
        {
            rootVisualElement.styleSheets.Add((StyleSheet)Resources.Load("ListPanel"));
            rootVisualElement.styleSheets.Add((StyleSheet)Resources.Load("EditorViewStyle"));
            var panel = new VisualElement();
            _inspector = new VisualElement();
            _secunce = new SeqcunceEdit();
            _container = new VisualElement();
            panel.AddToClassList("horizontal-container");
            _secunce.AddToClassList("view-size");
            _container.AddToClassList("verical-container");
            _addNewSecunce = (Button)CreateButton("+", "button-control");
            var removeSecunce = (Button)CreateButton("-", "button-control-minus");
            _inspector.AddToClassList("inspector");
            _addNewSecunce.clicked += AddSecunce;
            removeSecunce.clicked += Delete;
            rootVisualElement.Add(panel);
            panel.Add(_container);
            panel.Add(_secunce);
            panel.Add(_inspector);
            panel.Add(_addNewSecunce);
            panel.Add(removeSecunce);
        }
        public void AddSecunce()
        {
            var button = CreateButton("Sequnce", "buttonSequnce");
            var secunce = new Sequnce();
            _container.Add(button);
            _listSecunce.Add(button, secunce);
            button.clicked += () => OnClickButton(button);
        }
        private void OnClickButton(Button button)
        {
            if (_focusButton != button && _focusButton != null)
            {
                _focusButton.RemoveFromClassList("buttonSequnce-focus");
                _curretSecunce = null;
            }
            if (_curretSecunce == null)
            {
                button.AddToClassList("buttonSequnce-focus");
                _secunce.SetEnabled(true);
                _curretSecunce = _listSecunce[button];
                _secunce.OutputSecunce(_curretSecunce);
            }
            _focusButton = button;
        }
        private void Delete()
        {
            if(_focusButton != null)
                _container.Remove(_focusButton);
            else if (_container.childCount > 0)
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