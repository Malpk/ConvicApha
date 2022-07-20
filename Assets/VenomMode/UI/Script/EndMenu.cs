using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.GameInteface;
using TMPro;

namespace MainMode.Mode1921
{
    public class EndMenu : UserInterface
    {
        [SerializeField] private TextMeshProUGUI _lable;
        public override UserInterfaceType Type => UserInterfaceType.EndMenu;

        public delegate void Command();
        public event Command Restart;

        public void SetMessange(string text, Color frontColor)
        {
            _lable.text = text;
            _lable.color = frontColor;
        }
        public void OnResetGame()
        {
            if (Restart != null)
                Restart();
        }
    }
}
