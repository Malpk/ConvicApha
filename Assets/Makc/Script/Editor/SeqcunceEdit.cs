using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Underworld.Editor
{
    public class SeqcunceEdit : GraphView
    {
        private int _countNode = 0;
        private Sequnce _secunce;
        private GraphElement _mainElement = null;


        public SeqcunceEdit()
        {
            styleSheets.Add((StyleSheet)Resources.Load("EditorViewStyle"));
            var grid = new GridBackground();
            Insert(0, grid);
            AddManipulate();
        }
        public delegate void ChoiseNode(SerializedObject scriptableObject);
        public delegate void UnSelectNode();

        public event ChoiseNode ChoiseNodeAction;
        public event UnSelectNode UnSelectNodeAction;
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if (startPort.node == port.node)
                {
                    return;
                }
                if (startPort.direction == port.direction)
                {
                    return;
                }
                compatiblePorts.Add(port);
            });
            return compatiblePorts;
        }
        public void OutputSecunce(Sequnce secunce)
        {
            _countNode = secunce.Elements.Count;
            if(_secunce != null)
                SaveSecunce();
            _secunce = secunce;
            graphElements.ForEach(node =>
            {
                RemoveElement(node);
            });
            for (int i = 0; i < secunce.Elements.Count; i++)
            {
                LoadNode(secunce.Elements[i]);
            }
            for (int i = 0; i < secunce.Elements.Count; i++)
            {
                if (secunce.Elements[i].outputContainer[0] is Port port && secunce.Elements[i].Edge != null)
                {
                    AddElement(secunce.Elements[i].Edge);
                    port.Connect(secunce.Elements[i].Edge);
                }
            }
        }
        private void SaveSecunce()
        {
            UnSelect();
            var list = new List<UnderWorldNode>();
            graphElements.ForEach(node =>
            {
                if (node is UnderWorldNode underworldNode)
                {
                    if(underworldNode.outputContainer[0] is Port output)
                    {
                        var conection = output.connections.ToList();
                        if (conection.Count > 0)
                        {
                            underworldNode.Edge = conection[0];
                        }
                    }
                    underworldNode.SelectNodeAction -= SelectNode;
                    underworldNode.UnSelectNodeAction -= UnSelect;
                    list.Add(underworldNode);
                }
            });
            _secunce.Elements = list;
        }
        private void SelectNode(NodeSetting setting)
        {
            if (ChoiseNodeAction == null || setting == null)
                return;
            ChoiseNodeAction(GetSerilizeSetting(setting));
        }
        private void UnSelect()
        {
            if (UnSelectNodeAction != null)
                UnSelectNodeAction();
        }
        private SerializedObject GetSerilizeSetting(NodeSetting setting)
        {
            switch (setting.type)
            {
                case ModeTypeNew.BaseMode:
                    if (setting is BaseNodeSetting baseSetting)
                        return new SerializedObject(baseSetting);
                    throw new System.NullReferenceException("BaseNodeSetting is null");
                case ModeTypeNew.IslandMode:
                    if (setting is IslandNodeSetting islandSetting)
                        return new SerializedObject(islandSetting);
                    throw new System.NullReferenceException("BaseNodeSetting is null");
            }
            throw new System.NullReferenceException();
        }
        public override EventPropagation DeleteSelection()
        {
            _mainElement = graphElements.ToList()[0];
            if (selection.Count > 0)
            {
                RemoveFromSelection(_mainElement);
            }
            return base.DeleteSelection();
        }
        private void AddManipulate()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(CreateSecunceNode());
        }
        private IManipulator CreateSecunceNode()
        {
            var seqcunceNode = new ContextualMenuManipulator(mainMenu =>
                 mainMenu.menu.AppendAction("Add Mode", addNode => CreateNode(addNode.eventInfo.mousePosition)));
            return seqcunceNode;
        }
        private void CreateNode(Vector2 position)
        {
            var node = new UnderWorldNode(position, ModeTypeNew.BaseMode ,_countNode == 0);
            AddElement(node);
            node.SelectNodeAction += SelectNode;
            _countNode++;
        }
        private UnderWorldNode CreateNode(NodeSetting setting,bool firstElement)
        {
            var node = new UnderWorldNode(setting.Position, setting.type, firstElement);
            node.SetMode(setting);
            node.SelectNodeAction += SelectNode;
            node.UnSelectNodeAction += UnSelect;
            AddElement(node);
            return node;
        }

        private void LoadNode(UnderWorldNode node)
        {
            AddElement(node);
            node.SelectNodeAction += SelectNode;
            _countNode++;
        }
    }
}