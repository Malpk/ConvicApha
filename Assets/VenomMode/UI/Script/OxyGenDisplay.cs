using MainMode.GameInteface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode.Mode1921
{
    public class OxyGenDisplay : Receiver
    {
        [Header("Requred Reference")]
        [SerializeField] private Image _fieldImage;

        public override TypeDisplay DisplayType => TypeDisplay.OxyGenUI;

        public void UpdateField(float value)
        {
            _fieldImage.fillAmount =Mathf.Clamp01(value);
        }
    }
}