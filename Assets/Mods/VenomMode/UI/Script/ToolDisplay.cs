using MainMode.GameInteface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode.Mode1921
{
    public class ToolDisplay : MonoBehaviour
    {
        [SerializeField] private ToolCell[] _toolsDislplay;

        public void Restart()
        {
            foreach (var tool in _toolsDislplay)
            {
                tool.SetSprite(null);
            }
        }
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
        public bool DeleyIcon(int index)
        {
            if (index < _toolsDislplay.Length)
            {
                _toolsDislplay[index].SetSprite(null);
                return true;
            }
            return false;
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