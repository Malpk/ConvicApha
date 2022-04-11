using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Underworld.Editor
{
    public class DisplaySequnce : GraphView
    {
        public DisplaySequnce()
        {
            styleSheets.Add((StyleSheet)Resources.Load("EditorViewStyle"));
            var grid = new GridBackground();
            Insert(0, grid);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(CreateSecunceNode());
        }

        private IManipulator CreateSecunceNode()
        {
            var seqcunceNode =new ContextualMenuManipulator(mainMenu => 
                mainMenu.menu.AppendAction("Add Mode", addNode => CreateNode(addNode.eventInfo.mousePosition)));
            return seqcunceNode;
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                {
                    return;
                }

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
        public void CreateNode(Vector2 position)
        {
            AddElement(new UnderWorldNode(position));
        }
    }
}