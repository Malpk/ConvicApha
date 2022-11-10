using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    public class ToolSet : MonoBehaviour
    {
        [SerializeField] private ToolDisplay _toolDisplay;

        private List<ToolRepairs> _tools = new List<ToolRepairs>();

        public bool IsAccessTools => _tools.Count > 0;

        public void UseTool()
        {
            if (_tools.Count > 0)
            {
                _toolDisplay.DeleyIcon(_tools.Count - 1);
                _tools.Remove(_tools[_tools.Count - 1]);
            }
        }
        public void ShowHint()
        {
            _toolDisplay.ShowHint();
        }
        public void Restart()
        {
            _tools.Clear();
            _toolDisplay.Restart();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out ToolRepairs tool))
            {
                _toolDisplay.Display(tool.Icon);
                tool.Pick();
                _tools.Add(tool);
            }
        }
    }
}
