using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using Underworld.Save;

namespace Underworld.Editors
{
    public class UnderWorldEditor : EditorWindow
    {
        private Button _addNewSecunce;
        private Button _focusButton = null;
        private Seqcunce _curretSecunce;
        private ModeSwitchController _controller;
        private Dictionary<VisualElement, Seqcunce> _listSecunce = new Dictionary<VisualElement, Seqcunce>();
        private VisualElement _container;
        private VisualElement _inspector;
        private SeqcunceEdit _secunce = null;

        public static UnderWorldEditor Intializate()
        {
            var window = (UnderWorldEditor)EditorWindow.GetWindow(typeof(UnderWorldEditor));
            window.Show();
            window.position = new Rect(0, 0, 1000, 1000);
            EditorUtility.SetDirty(window);
            return window;
        }
        public void SetParent(ModeSwitchController parent)
        {
            _controller = parent;
            _inspector.Clear();
            _container.Clear();
            foreach (var sequnce in parent.Seqcuncs)
            {
                AddSecunce(sequnce);
            }
            if (_controller.Seqcuncs.Count > 0)
            {
                _secunce.SetEnabled(true);
                _secunce.OutputSecunce(_controller.Seqcuncs[0]);
            }
        }
        private void CreateGUI()
        {
            CreateBaseInterface();
        }
        private void OnEnable()
        {
            _secunce = new SeqcunceEdit();
            _secunce.SetEnabled(false);
            _secunce.UnSelectNodeAction += () => _inspector.Clear();
            _secunce.ChoiseNodeAction += OutputSetting;
        }
        private void OnDisable()
        {
            _secunce.SaveSecunce();
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
        private void AddSecunce()
        {
            var seqcunce = new Seqcunce();
            AddSecunce(seqcunce);
            _controller.Add(seqcunce);
        }
        private void AddSecunce(Seqcunce seqcunce)
        {
            var button = CreateButton($"Sequnce{_container.childCount+1}", "buttonSequnce");
            _container.Add(button);
            _listSecunce.Add(button, seqcunce);
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
            VisualElement deleteButton = null;
            if (_focusButton != null)
            {
                deleteButton = _focusButton;
            }
            else if (_container.childCount > 0)
            {
                deleteButton = _container[(_container.childCount - 1)];
            }
            _controller.Remove(_listSecunce[deleteButton]);
            _listSecunce.Remove(deleteButton);
            _container.Remove(deleteButton);
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