using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Underworld.Editor
{
    public class UnderWorldNode : Node
    {
        private ModeType curretType = ModeType.BaseMode;
        public UnderWorldNode(Vector2 position)
        {
            SetPosition(new Rect(position, Vector2.zero));
            DrawNode();
        }
        public string NodeName { get; set; }


        private void DrawNode()
        {
            var modeType = new EnumField(curretType);
            titleContainer.Add(modeType);
            var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input,
                Port.Capacity.Single, typeof(bool));
            var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output,
               Port.Capacity.Single, typeof(bool));
            inputPort.portName = "Previus mode";
            outputPort.portName = "Next mode";
            inputContainer.Insert(0, inputPort);
            outputContainer.Insert(0, outputPort);
        }
    }
}
