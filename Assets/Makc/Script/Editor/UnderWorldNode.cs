using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Underworld.Editors
{
    public class UnderWorldNode : Node,IBinding
    {
        public readonly int id;

        private EnumField _typeField;
        private NodeSetting _setting;

        public Edge Edge = null;
        public UnderWorldNode(Vector2 position,ModeTypeNew type,bool firstNode,int id)
        {
            this.id = id;
            SetPosition(new Rect(position, Vector2.zero));
            DrawNode(firstNode,type);
            SetMode(type);
            _setting.Position = position;
        }
        public UnderWorldNode(Vector2 position, NodeSetting setting, bool firstNode, int id)
        {
            this.id = id;
            SetPosition(new Rect(position, Vector2.zero));
            DrawNode(firstNode, setting.TypeMode);
            _setting = setting;
            _setting.Position = position;
        }
        public string NodeName { get; set; }
        public NodeSetting NodeSetting => _setting;

        public delegate void SelectNode(NodeSetting setting);
        public delegate void UnSelectNode();
        public event SelectNode SelectNodeAction;
        public event UnSelectNode UnSelectNodeAction;

        public override void OnSelected()
        {
            if (SelectNodeAction != null)
                SelectNodeAction(NodeSetting);
        }
        public override void OnUnselected()
        {
            if (UnSelectNodeAction != null)
                UnSelectNodeAction();
        }
        private void DrawNode(bool isFirstNode, ModeTypeNew type)
        {
            _typeField = new EnumField(type);
            _typeField.binding = this;
            titleContainer.Add(_typeField);
            var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output,
                Port.Capacity.Single, typeof(bool));
            outputPort.portName = "Next mode";
            outputContainer.Insert(0, outputPort);
            if (!isFirstNode)
            {
                var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input,
                  Port.Capacity.Single, typeof(bool));
                inputPort.portName = "Previus mode";
                inputContainer.Insert(0, inputPort);
            }
        }
        public void SetMode(ModeTypeNew mode)
        {
            var position = NodeSetting != null ? NodeSetting.Position : Vector2.zero;
            switch (mode)
            {
                case ModeTypeNew.BaseMode:
                    _setting = new BaseNodeSetting();
                    break;
                case ModeTypeNew.IslandMode:
                    _setting = new IslandNodeSetting();
                    break;
                case ModeTypeNew.BoxMode:
                    _setting = new BoxNodeSetting();
                    break;
                case ModeTypeNew.CoruselMode:
                    _setting = new CoruselNodeSetting();
                    break;
                case ModeTypeNew.PaternCreater:
                    _setting = new PaternCreaterNodeSetting();
                    break;
                case ModeTypeNew.TrindetMode:
                    _setting = new TridenNodeSetting();
                    break;
                case ModeTypeNew.RayMode:
                    _setting = new RayNodeSetting();
                    break;
                case ModeTypeNew.ViseMode:
                    _setting = new ViseNodeSetting();
                    break;
                default:
                    _setting = new BaseNodeSetting();
                    break;
            }
            _setting.Constructor(position);
        }
        public void SetMode(NodeSetting setting)
        {
            _setting = setting;
        }
        public void SetInputNode(int index)
        {
            NodeSetting.NextNode = index;
        }
        public void PreUpdate()
        {
            if (_typeField.value is ModeTypeNew type && _setting.TypeMode != type) 
                SetMode(type);
            else
                return;
            if (SelectNodeAction != null)
                SelectNodeAction(_setting);
        }

        public void Update()
        {
            
        }
        public void Release()
        {
        }
    }
}
