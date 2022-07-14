using MainMode.GameInteface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode.Mode1921
{
    public class ToolDisplay : Receiver
    {
        [SerializeField] private ToolCell[] _toolsDislplay;

        public override TypeDisplay DisplayType => TypeDisplay.ToolSetUI;

        public void Display(Sprite icon)
        {
            foreach (var cell in _toolsDislplay)
            {
                if (!cell.IsBusy)
                {
                    cell.SetSprite(icon);
                    return;
                }
            }
        }
        public void ShowHint()
        {
            for (int i = 0; i < _toolsDislplay.Length; i++)
            {
                if(!_toolsDislplay[i].IsBusy)
                    _toolsDislplay[i].ShowHint();
            }
        }
    }
}