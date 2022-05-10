using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Underworld.Editors
{
    public class SeqcunceEdit : GraphView
    {
        private int _countNode = 0;
        private Seqcunce _secunce;
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
        public void OutputSecunce(Seqcunce secunce)
        {
            _countNode = 0;
            if(_secunce != null)
                SaveSecunce();
            _secunce = secunce;
            graphElements.ForEach(node =>
            {
                RemoveElement(node);
            });
            var nods = new List<UnderWorldNode>();
            for (int i = 0; i < secunce.Elements.Count; i++)
            {
                nods.Add(LoadNode(secunce.Elements[i]));
            }
            for (int i = 0; i < nods.Count; i++)
            {
                try
                {
                    if (nods[i].outputContainer[0] is Port output &&
                   nods[secunce.Elements[i].NextNode].inputContainer[0] is Port input)
                    {
                        var edge = new Edge();
                        edge.output = output;
                        edge.input = input;
                        output.Connect(edge);
                        Add(edge);
                    }
                }
                catch
                {
                }
               
            }
        }
        private void SaveSecunce()
        {
            UnSelect();
            var list = new List<NodeSetting>();
            graphElements.ForEach(node =>
            {
                if (node is UnderWorldNode underworldNode)
                {
                    if (underworldNode.outputContainer[0] is Port output)
                    {
                        var conection = output.connections.ToList();
                        if (conection.Count > 0)
                        {
                            underworldNode.NodeSetting.NextNode = (conection[0].input.node as UnderWorldNode).id;
                        }
                        else
                        {
                            underworldNode.NodeSetting.NextNode = -1;
                        }
                    }
                    underworldNode.NodeSetting.Position = underworldNode.GetPosition().position;
                    underworldNode.SelectNodeAction -= SelectNode;
                    underworldNode.UnSelectNodeAction -= UnSelect;
                    list.Add(underworldNode.NodeSetting);
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
            switch (setting.TypeMode)
            {
                case ModeTypeNew.BaseMode:
                    if (setting is BaseNodeSetting baseSetting)
                        return new SerializedObject(baseSetting);
                    throw new System.NullReferenceException("BaseNodeSetting is null");
                case ModeTypeNew.IslandMode:
                    if (setting is IslandNodeSetting islandSetting)
                        return new SerializedObject(islandSetting);
                    throw new System.NullReferenceException("IslandNodeSetting is null");
                case ModeTypeNew.BoxMode:
                    if (setting is BoxNodeSetting boxNode)
                        return new SerializedObject(boxNode);
                    throw new System.NullReferenceException("BoxNodeSetting is null");
                case ModeTypeNew.CoruselMode:
                    if (setting is CoruselNodeSetting coruselNode)
                        return new SerializedObject(coruselNode);
                    throw new System.NullReferenceException("CoruselNodeSetting is null");
                case ModeTypeNew.PaternCreater:
                    if (setting is PaternCreaterNodeSetting paternCreaterNode)
                        return new SerializedObject(paternCreaterNode);
                    throw new System.NullReferenceException("PaternCreaterNodeSetting is null");
                case ModeTypeNew.TrindetMode:
                    if (setting is TridenNodeSetting tridenNode)
                        return new SerializedObject(tridenNode);
                    throw new System.NullReferenceException("TridenNodeSetting is null"); ;
                case ModeTypeNew.RayMode:
                    if (setting is RayNodeSetting rayNode)
                        return new SerializedObject(rayNode);
                    throw new System.NullReferenceException("RayNodeSetting is null");
                case ModeTypeNew.ViseMode:
                    if (setting is ViseNodeSetting viseNode)
                        return new SerializedObject(viseNode);
                    throw new System.NullReferenceException("ViseNodeSetting is null");
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
                 mainMenu.menu.AppendAction("Add Mode", addNode => CreateNode(
                     contentContainer.WorldToLocal(addNode.eventInfo.mousePosition))));
            return seqcunceNode;
        }
        private void CreateNode(Vector2 position)
        {
            var node = new UnderWorldNode(position, ModeTypeNew.BaseMode ,_countNode == 0, _countNode);
            CreateNodeBase(node);
        }
        private void CreateNodeBase(UnderWorldNode node)
        {
            AddElement(node);
            node.SelectNodeAction += SelectNode;
            node.UnSelectNodeAction += UnSelect;
            _countNode++;
        }
        private UnderWorldNode LoadNode(NodeSetting nodSetting)
        {
            var node = new UnderWorldNode(nodSetting.Position, nodSetting, _countNode == 0, _countNode);
            CreateNodeBase(node);
            return node;
        }
    }
}